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

public class MoviesReviewsController : Controller
{
    private readonly MoviePickerContext _context;

    public MoviesReviewsController(MoviePickerContext context)
    {
        _context = context;
    }

    // GET: MoviesReviews
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.MoviesReviews.Include(m => m.Movie).Include(m => m.Review);
        return View(await moviePickerContext.ToListAsync());
    }

    // GET: MoviesReviews/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesReview = await _context.MoviesReviews
            .Include(m => m.Movie)
            .Include(m => m.Review)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesReview == null)
        {
            return NotFound();
        }

        return View(moviesReview);
    }

    // GET: MoviesReviews/Create
    public IActionResult Create()
    {
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
        ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Title");
        return View();
    }

    // POST: MoviesReviews/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,ReviewId,Id")] MoviesReview moviesReview)
    {
        if (ModelState.IsValid)
        {
            if (!await IsMoviesReviewsExist(moviesReview.MovieId, moviesReview.ReviewId))
            {
                _context.Add(moviesReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie-review relation already exists.");
            }
        }
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesReview.MovieId);
        ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Title", moviesReview.ReviewId);
        return View(moviesReview);
    }

    // GET: MoviesReviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesReview = await _context.MoviesReviews.FindAsync(id);
        if (moviesReview == null)
        {
            return NotFound();
        }
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesReview.MovieId);
        ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Title", moviesReview.ReviewId);
        return View(moviesReview);
    }

    // POST: MoviesReviews/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MovieId,ReviewId,Id")] MoviesReview moviesReview)
    {
        if (id != moviesReview.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMoviesReviewsExist(moviesReview.MovieId, moviesReview.ReviewId))
            {

                try
                {
                    _context.Update(moviesReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesReviewExists(moviesReview.Id))
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
                ModelState.AddModelError(string.Empty, "This movie-review relation already exists.");
            }
        }
        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesReview.MovieId);
        ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Title", moviesReview.ReviewId);
        return View(moviesReview);
    }

    // GET: MoviesReviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var moviesReview = await _context.MoviesReviews
            .Include(m => m.Movie)
            .Include(m => m.Review)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moviesReview == null)
        {
            return NotFound();
        }

        return View(moviesReview);
    }

    // POST: MoviesReviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var moviesReview = await _context.MoviesReviews.FindAsync(id);
        if (moviesReview != null)
        {
            _context.MoviesReviews.Remove(moviesReview);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MoviesReviewExists(int id)
    {
        return _context.MoviesReviews.Any(e => e.Id == id);
    }

    public async Task<bool> IsMoviesReviewsExist(int movieID, int reviewID)
    {
        var moviesReviews = await _context.MoviesReviews
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.ReviewId == reviewID);

        return moviesReviews != null;
    }

}
