using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;

namespace MoviePickerInfrastructure.Controllers;

public class MoviesGenresController : Controller
{
    private readonly MoviePickerV2Context _context;

    public MoviesGenresController(MoviePickerV2Context context)
    {
        _context = context;
    }

    // GET: MoviesGenres
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.MoviesGenres.Include(m => m.Genre).Include(m => m.Movie);
        return View(await moviePickerContext.ToListAsync());
    }

    // GET: MoviesGenres/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesGenre = await _context.MoviesGenres
            .Include(m => m.Genre)
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesGenre == null)
        {
            return NotFound();
        }

        return View(moviesGenre);
    }

    // GET: MoviesGenres/Create
    public IActionResult Create()
    {
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
        return View();
    }

    // POST: MoviesGenres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,GenreId,Id")] MoviesGenre moviesGenre)
    {
        if (ModelState.IsValid)
        {
            if (!await IsMoviesGenresExist(moviesGenre.MovieId, moviesGenre.GenreId))
            {
                _context.Add(moviesGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie-genre relation already exists.");
            }
        }
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", moviesGenre.GenreId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesGenre.MovieId);
        return View(moviesGenre);
    }

    // GET: MoviesGenres/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesGenre = await _context.MoviesGenres.FindAsync(id);
        if (moviesGenre == null)
        {
            return NotFound();
        }
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", moviesGenre.GenreId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesGenre.MovieId);
        return View(moviesGenre);
    }

    // POST: MoviesGenres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MovieId,GenreId,Id")] MoviesGenre moviesGenre)
    {
        if (id != moviesGenre.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMoviesGenresExist(moviesGenre.MovieId, moviesGenre.GenreId))
            {
                try
                {
                    _context.Update(moviesGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesGenreExists(moviesGenre.Id))
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
                ModelState.AddModelError(string.Empty, "This movie-genre relation already exists.");
            }
        }
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", moviesGenre.GenreId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesGenre.MovieId);
        return View(moviesGenre);
    }

    // GET: MoviesGenres/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesGenre = await _context.MoviesGenres
            .Include(m => m.Genre)
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesGenre == null)
        {
            return NotFound();
        }

        return View(moviesGenre);
    }

    // POST: MoviesGenres/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var moviesGenre = await _context.MoviesGenres.FindAsync(id);
        if (moviesGenre != null)
        {
            _context.MoviesGenres.Remove(moviesGenre);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MoviesGenreExists(int id)
    {
        return _context.MoviesGenres.Any(e => e.Id == id);
    }

    public async Task<bool> IsMoviesGenresExist(int movieID, int genreID)
    {
        var moviesGenres = await _context.MoviesGenres
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.GenreId == genreID);

        return moviesGenres != null;
    }

}
