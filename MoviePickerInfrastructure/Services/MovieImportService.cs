using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;
using System.IO;
using System.Threading;

namespace MoviePickerInfrastructure.Services;

public class MovieImportService : IImportService<Movie>
{
    private readonly MoviePickerV2Context _context;

    public MovieImportService(MoviePickerV2Context context)
    {
        _context = context;
    }


    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
        }

        const int BasicInfo = 1;
        const int Genres = 2;
        const int Actors = 3;
        const int Languages = 4;
        const int Reviews = 5;

        try
        {

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                var movie = await GetBasicInfo(workBook.Worksheet(BasicInfo), cancellationToken);

                await GetGenresAsync(workBook.Worksheet(Genres), movie, cancellationToken);
                await GetActorsAsync(workBook.Worksheet(Actors), movie, cancellationToken);
                await GetLanguagesAsync(workBook.Worksheet(Languages), movie, cancellationToken);
                await GetReviewsAsync(workBook.Worksheet(Reviews), movie, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during import: " + ex.Message, ex);
        }
    }



    private async Task<Movie> GetBasicInfo(IXLWorksheet basicInfoSheet, CancellationToken cancellationToken)
    {
        const int InfoRow = 2;
        const int MovieTitleColumn = 1;
        const int ReleaseDateColumn = 2;
        const int DirectorColumn = 3;
        const int BudgetColumn = 4;
        const int BoxOfficeRevenueColumn = 5;
        const int DurationColumn = 6;
        const int DescriptionColumn = 7;

        var infoRow = basicInfoSheet.Row(InfoRow);

        string movieTitle = infoRow.Cell(MovieTitleColumn).Value.ToString();
        string releaseDateTimeString = infoRow.Cell(ReleaseDateColumn).Value.ToString();
        string directorName = infoRow.Cell(DirectorColumn).Value.ToString();
        string budgetString = infoRow.Cell(BudgetColumn).Value.ToString();
        string boxOfficeRevenueString = infoRow.Cell(BoxOfficeRevenueColumn).Value.ToString();
        string durationString = infoRow.Cell(DurationColumn).Value.ToString();
        string description = infoRow.Cell(DescriptionColumn).Value.ToString();

        if (string.IsNullOrEmpty(movieTitle))
        {
            throw new Exception("Movie doesn't have a name");
        }


        if (string.IsNullOrEmpty(directorName) ||
          string.IsNullOrEmpty(releaseDateTimeString))
        {
            throw new Exception("Your file is missing some of critically important fields.");
        }

        if (!DateTime.TryParse(releaseDateTimeString, out DateTime releaseDateTime))
        {
            throw new Exception($"Movie: '{movieTitle}' have invalid release date format.");
        }

        DateOnly releaseDate = new DateOnly(releaseDateTime.Year, releaseDateTime.Month, releaseDateTime.Day);

        var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == directorName);

        if (director == null)
        {
            throw new Exception($"Director: {directorName} doesn't exist");
        }

        long budget;

        if (!long.TryParse(budgetString, out budget))
        {
            budget = 0;
        }

        long boxOfficeRevenue;

        if (!long.TryParse(boxOfficeRevenueString, out boxOfficeRevenue))
        {
            boxOfficeRevenue = 0;
        }

        int duration;

        if (!int.TryParse(durationString, out duration))
        {
            duration = 0;
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);

        if (movie == null)
        {
            movie = new Movie();
            movie.Title = movieTitle;
            movie.DirectorId = director.Id;
            movie.ReleaseDate = releaseDate;
            movie.Description = description;
            movie.Budget = budget;
            movie.BoxOfficeRevenue = boxOfficeRevenue;
            movie.Duration = duration;

            string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_movie_image.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                movie.MovieImage = defaultImageBytes;
            }

            _context.Movies.Add(movie);
        }
        else
        {
            MovieViewModel.DeleteMovieRelations(movie, _context);
        }


        return movie;

    }


    private async Task GetGenresAsync(IXLWorksheet genresWorkSheet, Movie movie, CancellationToken cancellationToken)
    {
        if (genresWorkSheet.RowsUsed().Count() <= 1)
        {
            throw new Exception("The genres worksheet is empty");
        }

        foreach (var row in genresWorkSheet.RowsUsed().Skip(1))
        {
            try
            {
                await AddGenre(row, movie, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }

    private async Task AddGenre(IXLRow row, Movie movie, CancellationToken cancellationToken)
    {
        var genreName = row.Cell(1).Value.ToString();

        if (string.IsNullOrEmpty(genreName))
        {
            throw new Exception("You have empty genre");
        }

        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

        if (genre == null)
        {
            genre = new Genre();
            genre.Name = genreName;
            _context.Genres.Add(genre);
        }

        MoviesGenre mg = new MoviesGenre { MovieId = movie.Id, GenreId = genre.Id };
        _context.MoviesGenres.Add(mg);
    }


    private async Task GetActorsAsync(IXLWorksheet actorsWorkSheet, Movie movie, CancellationToken cancellationToken)
    {

        if (actorsWorkSheet.RowsUsed().Count() <= 1)
        {
            throw new Exception("The actors worksheet is empty");
        }

        foreach (var row in actorsWorkSheet.RowsUsed().Skip(1))
        {
            try
            {
                await AddActorAsync(row, movie, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }

    private async Task AddActorAsync(IXLRow row, Movie movie, CancellationToken cancellationToken)
    {
        const int NameColumn = 1;
        const int BirthDateColumn = 2;
        const int BirthCountryColumn = 3;

        var actorName = row.Cell(NameColumn).Value.ToString();
        var actorBirthDateTimeString = row.Cell(BirthDateColumn).Value.ToString();
        var actorBirthCountryString = row.Cell(BirthCountryColumn).Value.ToString();


        if (string.IsNullOrEmpty(actorName))
        {
            throw new Exception("Actor have no name");
        }

        if (string.IsNullOrEmpty(actorBirthDateTimeString) ||
            string.IsNullOrEmpty(actorBirthCountryString))
        {
            throw new Exception("Some of required fields are empty.");
        }

        var actorBirthCountry = _context.Countries.FirstOrDefault(c => c.Name == actorBirthCountryString);
        if (actorBirthCountry == null)
        {
            throw new Exception($"Actor: '{actorName}' have invalid birth country.");
        }

        if (!DateTime.TryParse(actorBirthDateTimeString, out DateTime actorBirthDateTime))
        {
            throw new Exception($"Actor: '{actorName}' have invalid birth date format.");
        }

        DateOnly actorBirthDate = new DateOnly(actorBirthDateTime.Year, actorBirthDateTime.Month, actorBirthDateTime.Day);

        var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorName);

        if (actor == null)
        {
            actor = new Actor();
            actor.Name = actorName;
            actor.BirthDate = actorBirthDate;
            actor.BirthCountryId = actorBirthCountry.Id;
            ;
            string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                actor.ActorImage = defaultImageBytes;
            }
            _context.Actors.Add(actor);
        }

        MoviesActor ma = new MoviesActor { MovieId = movie.Id, ActorId = actor.Id };
        _context.MoviesActors.Add(ma);

    }


    private async Task GetLanguagesAsync(IXLWorksheet languagesWorkSheet, Movie movie, CancellationToken cancellationToken)
    {
        if (languagesWorkSheet.RowsUsed().Count() <= 1)
        {
            throw new Exception("The languages worksheet is empty");
        }

        foreach (var row in languagesWorkSheet.RowsUsed().Skip(1))
        {
            try
            {
                await AddLanguage(row, movie, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }

    private async Task AddLanguage(IXLRow row, Movie movie, CancellationToken cancellationToken)
    {
        var languageName = row.Cell(1).Value.ToString();

        if (string.IsNullOrEmpty(languageName))
        {
            throw new Exception("You have empty language");
        }

        var language = await _context.Languages.FirstOrDefaultAsync(l => l.Name == languageName);

        if (language == null)
        {
            language = new Language();
            language.Name = languageName;
            _context.Languages.Add(language);
        }

        MoviesLanguage ml = new MoviesLanguage { MovieId = movie.Id, LanguageId = language.Id };
        _context.MoviesLanguages.Add(ml);
    }


    private async Task GetReviewsAsync(IXLWorksheet reviewsWorkSheet, Movie movie, CancellationToken cancellationToken)
    {
        if (reviewsWorkSheet.RowsUsed().Count() < 1)
        {
            return;
        }

        foreach (var row in reviewsWorkSheet.RowsUsed().Skip(1))
        {
            try
            {
                await AddReview(row, movie, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }

    private async Task AddReview(IXLRow row, Movie movie, CancellationToken cancellationToken)
    {
        const int TitleColumn = 1;
        const int TextColumn = 2;
        const int RatingColumn = 3;


        string reviewTitle = row.Cell(TitleColumn).Value.ToString();
        string reviewText = row.Cell(TextColumn).Value.ToString();
        string reviewRatingString = row.Cell(RatingColumn).Value.ToString();

        if (string.IsNullOrEmpty(reviewTitle))
        {
            throw new Exception("Title for review is missing.");
        }

        double reviewRating;

        if (double.TryParse(reviewRatingString, out reviewRating))
        {
            if (reviewRating < 0.0 || reviewRating > 10.0)
            {
                throw new Exception($"Invalid rating for review: {reviewTitle}. It should be form 0.0 to 10.0");
            }
        }
        else
        {

            throw new Exception("Invalid format for review rating.");
        }

        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Title == reviewTitle);


        if (review == null)
        {
            review = new Review();
            review.Title = reviewTitle;
            review.Text = reviewText;
            review.Rating = reviewRating;
        }

        review.MovieId = movie.Id;
        _context.Reviews.Add(review);
    }
}



