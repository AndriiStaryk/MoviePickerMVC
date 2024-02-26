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

public class MoviesActorsController : Controller
{
    private readonly MoviePickerContext _context;

    public MoviesActorsController(MoviePickerContext context)
    {
        _context = context;
    }

    // GET: MoviesActors
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.MoviesActors.Include(m => m.Actor).Include(m => m.Movie);
        return View(await moviePickerContext.ToListAsync());
    }

    // GET: MoviesActors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesActor = await _context.MoviesActors
            .Include(m => m.Actor)
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesActor == null)
        {
            return NotFound();
        }

        return View(moviesActor);
    }

    // GET: MoviesActors/Create
    public IActionResult Create()
    {
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
        return View();
    }

    // POST: MoviesActors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,ActorId,Id")] MoviesActor moviesActor)
    {
        if (ModelState.IsValid)
        {
            if (!await IsMoviesActorsExist(moviesActor.MovieId, moviesActor.ActorId))
            {
                _context.Add(moviesActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie-actor relation already exists.");
            }
        }
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", moviesActor.ActorId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesActor.MovieId);
        return View(moviesActor);
    }

    // GET: MoviesActors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesActor = await _context.MoviesActors.FindAsync(id);
        if (moviesActor == null)
        {
            return NotFound();
        }
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", moviesActor.ActorId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesActor.MovieId);
        return View(moviesActor);
    }

    // POST: MoviesActors/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MovieId,ActorId,Id")] MoviesActor moviesActor)
    {
        if (id != moviesActor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMoviesActorsExist(moviesActor.MovieId, moviesActor.ActorId))
            {

                try
                {
                    _context.Update(moviesActor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesActorExists(moviesActor.Id))
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
                ModelState.AddModelError(string.Empty, "This movie-actor relation already exists.");
            }
        }
        ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", moviesActor.ActorId);
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesActor.MovieId);
        return View(moviesActor);
    }

    // GET: MoviesActors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesActor = await _context.MoviesActors
            .Include(m => m.Actor)
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesActor == null)
        {
            return NotFound();
        }

        return View(moviesActor);
    }

    // POST: MoviesActors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var moviesActor = await _context.MoviesActors.FindAsync(id);
        if (moviesActor != null)
        {
            _context.MoviesActors.Remove(moviesActor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MoviesActorExists(int id)
    {
        return _context.MoviesActors.Any(e => e.Id == id);
    }

    public async Task<bool> IsMoviesActorsExist(int movieID, int actorID)
    {
        var moviesActors = await _context.MoviesActors
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.ActorId == actorID);

        return moviesActors != null;
    }

}
