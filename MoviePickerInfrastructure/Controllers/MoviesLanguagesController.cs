using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;

namespace MoviePickerInfrastructure.Controllers
{
    public class MoviesLanguagesController : Controller
    {
        private readonly MoviePickerContext _context;

        public MoviesLanguagesController(MoviePickerContext context)
        {
            _context = context;
        }

        // GET: MoviesLanguages
        public async Task<IActionResult> Index()
        {
            var moviePickerContext = _context.MoviesLanguages.Include(m => m.Language).Include(m => m.Movie);
            return View(await moviePickerContext.ToListAsync());
        }

        // GET: MoviesLanguages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moviesLanguage = await _context.MoviesLanguages
                .Include(m => m.Language)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moviesLanguage == null)
            {
                return NotFound();
            }

            return View(moviesLanguage);
        }

        // GET: MoviesLanguages/Create
        public IActionResult Create()
        {
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: MoviesLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,LanguageId,Id")] MoviesLanguage moviesLanguage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moviesLanguage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", moviesLanguage.LanguageId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesLanguage.MovieId);
            return View(moviesLanguage);
        }

        // GET: MoviesLanguages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moviesLanguage = await _context.MoviesLanguages.FindAsync(id);
            if (moviesLanguage == null)
            {
                return NotFound();
            }
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", moviesLanguage.LanguageId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesLanguage.MovieId);
            return View(moviesLanguage);
        }

        // POST: MoviesLanguages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,LanguageId,Id")] MoviesLanguage moviesLanguage)
        {
            if (id != moviesLanguage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moviesLanguage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesLanguageExists(moviesLanguage.Id))
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
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", moviesLanguage.LanguageId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", moviesLanguage.MovieId);
            return View(moviesLanguage);
        }

        // GET: MoviesLanguages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moviesLanguage = await _context.MoviesLanguages
                .Include(m => m.Language)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moviesLanguage == null)
            {
                return NotFound();
            }

            return View(moviesLanguage);
        }

        // POST: MoviesLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moviesLanguage = await _context.MoviesLanguages.FindAsync(id);
            if (moviesLanguage != null)
            {
                _context.MoviesLanguages.Remove(moviesLanguage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoviesLanguageExists(int id)
        {
            return _context.MoviesLanguages.Any(e => e.Id == id);
        }
    }
}
