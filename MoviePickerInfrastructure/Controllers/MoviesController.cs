using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;

namespace MoviePickerInfrastructure.Controllers;

public class MoviesController : Controller
{
    private readonly MoviePickerContext _context;

    public MoviesController(MoviePickerContext context)
    {
        _context = context;
    }

    // GET: Movies
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Movies.Include(m => m.Director);
        return View(await moviePickerContext.ToListAsync());
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

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
        return View();
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId))
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                ModelState.AddModelError(string.Empty, "This movie already exists.");
            }
        }
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        return View(movie);
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
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId))
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        return View(movie);
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
            _context.Movies.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }

    public async Task<bool> IsMovieExist(string title, DateOnly releaseDate, int directorID)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Title == title && m.ReleaseDate == releaseDate && m.DirectorId == directorID);

        return movie != null;
    }

}
