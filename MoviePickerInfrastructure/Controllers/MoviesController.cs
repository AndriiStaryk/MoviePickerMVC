using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;
using System.IO;

namespace MoviePickerInfrastructure.Controllers;

public class MoviesController : Controller
{
    private readonly MoviePickerV2Context _context;
    private MovieViewModel _movieViewModel;
    private Movie _movie = new Movie();
    public MoviesController(MoviePickerV2Context context)
    {
        _context = context;
        _movieViewModel = new MovieViewModel(context, _movie);
    }

    // GET: Movies
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Movies.Include(m => m.Director);
        return View(await moviePickerContext.ToListAsync());
    }

    //public async Task<IActionResult> Index(int genreId)
    //{
    //    var moviesByGenreContext = _context.MoviesGenres
    //        .Where(mg => mg.GenreId == genreId)
    //        .Select(mg => mg.Movie);

    //    return View(await moviesByGenreContext.ToListAsync());
    //}


    public async Task<IActionResult> MoviesByGenre(int genreId)
    {
        var moviesByGenreContext = _context.MoviesGenres
            .Where(mg => mg.GenreId == genreId)
            .Select(mg => mg.Movie);

        return View(await moviesByGenreContext.ToListAsync());
    }


    public IActionResult ShowReviewInfo(int reviewId)
    {
        return RedirectToAction("Details", "Reviews", new { id = reviewId });
    }

    public async Task<IActionResult> MoviesByActor(int actorId)
    {
        //Actor actor = _context.Actors.Find(actorId)!;

        //ActorViewModel actorViewModel = new ActorViewModel(_context, actor);
        //return View(actorViewModel);

        var moviesByActorContext = _context.MoviesActors
            .Where(ma => ma.ActorId == actorId)
            .Select(ma => ma.Movie);

        return View(await moviesByActorContext.ToListAsync());
    }


    public IActionResult ActorInfo(int actorId)
    {
        return RedirectToAction("Details", "Actors", new { id = actorId });
    }


    public IActionResult DirectorInfo(int directorId)
    {
        return RedirectToAction("Details", "Directors", new { id = directorId });
    }

    public async Task<IActionResult> MoviesByLanguage(int languageId)
    {
        var moviesByLanguageContext = _context.MoviesLanguages
            .Where(ml => ml.LanguageId == languageId)
            .Select(ml => ml.Movie);

        return View(await moviesByLanguageContext.ToListAsync());
    }


    public async Task<IActionResult> MoviesByDirector(int directorId)
    {
        var moviesByDirectorContext = _context.Movies
            .Where(m => m.DirectorId == directorId);

        return View(await moviesByDirectorContext.ToListAsync());
    }
 
    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        
        var movie = await _context.Movies
            .Include(m => m.Director)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        _movieViewModel = new MovieViewModel(_context, movie);

        return View(_movieViewModel);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        _movieViewModel = new MovieViewModel(_context, _movie);

        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
        ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");

        return View(_movieViewModel);
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie, int[] genres, int[] actors, int[] languages/*, [FromForm] MovieViewModel movieViewModel*/ /*[Bind("Movie, Genres")] MovieViewModel movieViewModel*/)
    {
        //_movieViewModel.Movie = movie;
        
        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId, movie.Budget, movie.BoxOfficeRevenue, movie.Rating))
            {
                _movieViewModel.Movie = movie;

                _context.Add(movie);

                foreach (var genreId in genres)
                {
                    _movieViewModel.Movie.MoviesGenres.Add(new MoviesGenre { MovieId = movie.Id, GenreId = genreId });
                }

                foreach (var actorId in actors)
                {
                    _movieViewModel.Movie.MoviesActors.Add(new MoviesActor { MovieId = movie.Id, ActorId = actorId });
                }

                foreach (var languageId in languages)
                {
                    _movieViewModel.Movie.MoviesLanguages.Add(new MoviesLanguage { MovieId = movie.Id, LanguageId = languageId });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie already exists.");
            }
        }
        //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);

        ViewData["GenreId"] = new SelectList(_context.Genres.Include(g => g.Id), "Id", "Name");

        //ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
        return View(_movieViewModel);
    }


    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _movieViewModel = new MovieViewModel(_context, movie);

        //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);

        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
        ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");

        return View(_movieViewModel);
       // return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie, int[] genres, int[] actors, int[] languages)
    //{
    //    if (id != movie.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId, movie.Budget, movie.BoxOfficeRevenue, movie.Rating))
    //        {
    //            try
    //            {
    //                _movieViewModel.Movie = movie;

    //                _context.Update(movie);

    //                foreach (var genreId in genres)
    //                {
    //                    _movieViewModel.Movie.MoviesGenres.Add(new MoviesGenre { MovieId = movie.Id, GenreId = genreId });
    //                }

    //                foreach (var actorId in actors)
    //                {
    //                    _movieViewModel.Movie.MoviesActors.Add(new MoviesActor { MovieId = movie.Id, ActorId = actorId });
    //                }

    //                foreach (var languageId in languages)
    //                {
    //                    _movieViewModel.Movie.MoviesLanguages.Add(new MoviesLanguage { MovieId = movie.Id, LanguageId = languageId });
    //                }

    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!MovieExists(movie.Id))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        else
    //        {
    //            ModelState.AddModelError(string.Empty, "This movie already exists.");
    //        }
    //    }
    //    ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
    //    return View(_movieViewModel);
    //}



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] MovieViewModel movieViewModel, int[] genres, int[] actors, int[] languages)
    {
        if (id != movieViewModel.Movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movieViewModel.Movie.Title, movieViewModel.Movie.ReleaseDate, movieViewModel.Movie.DirectorId, movieViewModel.Movie.Budget, movieViewModel.Movie.BoxOfficeRevenue, movieViewModel.Movie.Rating))
            {
                try
                {

                    var movie = movieViewModel.Movie; // Get the main movie entity from the view model

                    movieViewModel = new MovieViewModel(_context, movie);

                    _context.Update(movie);

                    foreach (var genreId in genres)
                    {
                        movie.MoviesGenres.Add(new MoviesGenre { MovieId = movie.Id, GenreId = genreId });
                    }

                    foreach (var actorId in actors)
                    {
                        movie.MoviesActors.Add(new MoviesActor { MovieId = movie.Id, ActorId = actorId });
                    }

                    foreach (var languageId in languages)
                    {
                        movie.MoviesLanguages.Add(new MoviesLanguage { MovieId = movie.Id, LanguageId = languageId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movieViewModel.Movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie already exists.");
            }
        }

        // Repopulate ViewData for dropdowns
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movieViewModel.Movie.DirectorId);
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
        ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");

        return View(movieViewModel);
    }



    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .Include(m => m.Director)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
       
        if (movie != null)
        {
            //_movieViewModel.Movie = movie;
            MovieViewModel.DeleteMovie(movie, _context);
           // _context.Movies.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }

    public async Task<bool> IsMovieExist(string title, DateOnly releaseDate, int directorID, long? budget, long? boxOfficeRevenue, double? rating)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(
                m => m.Title == title &&
                m.ReleaseDate == releaseDate &&
                m.DirectorId == directorID &&
                m.Budget == budget &&
                m.BoxOfficeRevenue == boxOfficeRevenue &&
                m.Rating == rating);

        return movie != null;
    }
}

