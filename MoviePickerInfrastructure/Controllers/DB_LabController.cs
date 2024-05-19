using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Controllers;

public class DB_LabController : Controller
{
    private readonly MoviePickerV2Context _context;


    public DB_LabController(MoviePickerV2Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Request1(int? minAge)
    {
        //Фільми, в яких грає принаймні один актор, якому більше за {minAge} років

        var movies = await _context.Movies   
            .ToListAsync();

        if (minAge == null)
        {
            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Фільми, в яких грає принаймні один актор, якому більше за 0 років\"";
            return View();
        }

        string sql = @"
        SELECT DISTINCT m. *
        FROM Movies m
        JOIN MoviesActors ma ON m.Id = ma.MovieID
        JOIN Actors a ON ma.ActorID = a.Id
        WHERE DATEDIFF(YEAR, a.BirthDate, GETDATE()) > @minAge";


        movies = await _context.Movies
                        .FromSqlRaw(sql, new SqlParameter("@minAge", minAge.Value))
                        .ToListAsync();

        ViewBag.Movies = movies;
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Фільми, в яких грає принаймні один актор, якому більше за {minAge} років\"";

        return View();

    }



    public async Task<IActionResult> Request2(int? countryId)
    {
        //Фільми, в яких грає принаймні один актор, з {country.Name}

        ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");

        var movies = await _context.Movies.ToListAsync();

        if (countryId == null)
        {
            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Фільми, в яких грає принаймні один актор, з будь-якої країни\"";
            return View();
        }

        string sql = @"
        SELECT DISTINCT m. *
        FROM Movies m
        JOIN MoviesActors ma ON m.Id = ma.MovieID
        JOIN Actors a ON ma.ActorID = a.Id
        JOIN Countries c ON a.BirthCountryID = c.Id
        WHERE c.Id = @countryId";


        movies = await _context.Movies
                        .FromSqlRaw(sql, new SqlParameter("@countryId", countryId.Value))
                        .ToListAsync();

        ViewBag.Movies = movies;

        var country = await _context.Countries.FindAsync(countryId);
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Фільми, в яких грає принаймні один актор, з {country.Name}\"";

        return View();
    }
    public async Task<IActionResult> Request3(int? languageId)
    {
        //Фільми, які дубльовані {language.Name} мовою

        ViewBag.Languages = new SelectList(_context.Languages, "Id", "Name");

        var movies = await _context.Movies.ToListAsync();

        if (languageId == null)
        {
            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Фільми, які дубльовані будь-якою мовою\"";
            return View();
        }

        string sql = @"
        SELECT DISTINCT m. *
        FROM Movies m
        JOIN MoviesLanguages ml ON m.Id = ml.MovieID
        JOIN Languages l ON ml.LanguageID = l.Id
        WHERE l.Id = @languageId";


        movies = await _context.Movies
                        .FromSqlRaw(sql, new SqlParameter("@languageId", languageId.Value))
                        .ToListAsync();
        ViewBag.Movies = movies;

        var language = await _context.Languages.FindAsync(languageId);
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Фільми, які дубльовані {language.Name} мовою\"";

        return View();
    }
    public async Task<IActionResult> Request4(int? reviewsCount)
    {
        //Фільми, в яких кількість відгуків більше за {reviewsCount}

        var movies = await _context.Movies
            .ToListAsync();

        if (reviewsCount == null)
        {
            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Фільми, в яких кількість відгуків більше за 0\"";
            return View();
        }

        string sql = @"
        SELECT
        m.*
        FROM
            Movies m
        JOIN
            Reviews r ON m.Id = r.MovieID
        GROUP BY
            m.Id, m.Title, m.ReleaseDate, m.DirectorID, m.Budget, m.BoxOfficeRevenue, m.Duration, m.Rating, m.MovieImage, m.Description
        HAVING
            COUNT(r.MovieID) > @reviewsCount;
        ";


        movies = await _context.Movies
                        .FromSqlRaw(sql, new SqlParameter("@reviewsCount", reviewsCount.Value))
                        .ToListAsync();

        ViewBag.Movies = movies;
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Фільми, в яких кількість відгуків більше за {reviewsCount}\"";

        return View();
    }
    public async Task<IActionResult> Request5(int? moviesCount)
    {
        //Актори, які знімались у {moviesCount} або більше фільмах

        var actors = await _context.Actors
            .ToListAsync();

        if (moviesCount == null)
        {
            ViewBag.Actors = actors;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Актори, які знімались у 0 або більше фільмах\"";
            return View();
        }

        string sql = @"
       SELECT
        a.*
        FROM
            Actors a
        JOIN
            MoviesActors ma ON a.Id = ma.ActorID
        JOIN
            Movies m ON ma.MovieID = m.Id
        GROUP BY
            a.Id, a.Name, a.BirthDate, a.BirthCountryID, a.ActorImage 
        HAVING
        COUNT(DISTINCT m.Id) >= @moviesCount;
        ";


        actors = await _context.Actors
                        .FromSqlRaw(sql, new SqlParameter("@moviesCount", moviesCount.Value))
                        .ToListAsync();

        ViewBag.Actors = actors;
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Актори, які знімались у {moviesCount} або більше фільмах\"";

        return View();
    }
    public async Task<IActionResult> Request6(int? movieId)
    {
        //назви фільмів, в яких знімались точно такі ж актори, як в фільмі "Х"

        ViewBag.AllMovies = new SelectList(_context.Movies, "Id", "Title");

        var movies = await _context.Movies
            .ToListAsync();

        if (movieId == null)
        {
            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Усі фільми\"";
            return View();
        }

            string sql = @"
            WITH MovieActors AS (
            SELECT ma.ActorID
            FROM MoviesActors ma
            WHERE ma.MovieID = @movieId
            ),
            MatchingMovies AS (
                SELECT ma.MovieID
                FROM MoviesActors ma
                JOIN MovieActors ma2 ON ma.ActorID = ma2.ActorID
                GROUP BY ma.MovieID
                HAVING COUNT(DISTINCT ma.ActorID) = (SELECT COUNT(*) FROM MovieActors)
            )
            SELECT m.*
            FROM Movies m
            JOIN MatchingMovies mm ON m.Id = mm.MovieID
            WHERE m.Id <> @movieId;
            ";


            movies = await _context.Movies
                            .FromSqlRaw(sql, new SqlParameter("@movieId", movieId.Value))
                            .ToListAsync();

            var movie = await _context.Movies.FindAsync(movieId);

            ViewBag.Movies = movies;
            ViewBag.RequestTextRaw = $"Результати за запитом: \"Фільми, в яких знімались точно такі ж актори, як в фільмі {movie.Title}\"";

        return View();


    }
    public async Task<IActionResult> Request7(int? directorId)
    {
        //імена акторів, що знімались в усіх фільмах режисера "Х"

        ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name");

        var actors = await _context.Actors
            .ToListAsync();

        if (directorId == null)
        {
            ViewBag.Actors = actors;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Усі фільми\"";
            return View();
        }

        string sql = @"
        WITH DirectorMovies AS (
            SELECT m.Id AS MovieID
            FROM Movies m
            WHERE m.DirectorID = @directorId
        ),
        ActorInDirectorMovies AS (
            SELECT ma.ActorID, dm.MovieID
            FROM MoviesActors ma
            JOIN DirectorMovies dm ON ma.MovieID = dm.MovieID
        ),
        ActorMovieCounts AS (
            SELECT ActorID, COUNT(DISTINCT MovieID) AS MovieCount
            FROM ActorInDirectorMovies
            GROUP BY ActorID
        ),
        TotalDirectorMovies AS (
            SELECT COUNT(DISTINCT MovieID) AS TotalMovies
            FROM DirectorMovies
        )
        SELECT a.*
        FROM Actors a
        JOIN ActorMovieCounts amc ON a.Id = amc.ActorID
        JOIN TotalDirectorMovies tdm ON amc.MovieCount = tdm.TotalMovies;
        ";

        
        actors = await _context.Actors
                        .FromSqlRaw(sql, new SqlParameter("@directorId", directorId))
                        .ToListAsync();


        var director = await _context.Directors.FindAsync(directorId);

        ViewBag.Actors = actors;
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Актори, які знімались в усіх фільмах режисера {director.Name}\"";

        return View();

    }
    public async Task<IActionResult> Request8(int? movieId)
    {
        //Режисери, які зняли принаймні один фільм такого ж жанру як жанри фільму "Х"

        ViewBag.AllMovies = new SelectList(_context.Movies, "Id", "Title");

        var directors = await _context.Directors
            .ToListAsync();

        if (movieId == null)
        {
            ViewBag.Directors = directors;
            ViewBag.RequestTextRaw = "Результати за запитом: \"Усі режисери\"";
            return View();
        }

        string sql = @"
        WITH MovieGenres AS (
            SELECT mg.GenreID
            FROM MoviesGenres mg
            WHERE mg.MovieID = @movieId
        ),
        DirectorMovies AS (
            SELECT DISTINCT m.DirectorID
            FROM Movies m
            JOIN MoviesGenres mg ON m.Id = mg.MovieID
            WHERE mg.GenreID IN (SELECT GenreID FROM MovieGenres)
        )
        SELECT d.*
        FROM Directors d
        JOIN DirectorMovies dm ON d.Id = dm.DirectorID;
        ";

        
        directors = await _context.Directors
                        .FromSqlRaw(sql, new SqlParameter("@movieId", movieId))
                        .ToListAsync();


        var movie = await _context.Movies.FindAsync(movieId);

        ViewBag.Directors = directors;
        ViewBag.RequestTextRaw = $"Результати за запитом: \"Режисери, які зняли принаймні один фільм такаких самих жанрів як і фільм {movie.Title}\"";

        return View();
    }
    public async Task<IActionResult> Request9()
    {
        return View();
    }
}
