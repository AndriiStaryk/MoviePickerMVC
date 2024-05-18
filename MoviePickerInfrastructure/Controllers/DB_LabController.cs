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
    public async Task<IActionResult> Request6()
    {
        return View();
    }
    public async Task<IActionResult> Request7()
    {
        return View();
    }
    public async Task<IActionResult> Request8()
    {
        return View();
    }
    public async Task<IActionResult> Request9()
    {
        return View();
    }
}
