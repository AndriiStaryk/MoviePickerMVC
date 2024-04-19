using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;
using Newtonsoft.Json;

namespace MoviePickerInfrastructure.Controllers;

public class GenresController : Controller
{
    private readonly MoviePickerV2Context _context;

    public GenresController(MoviePickerV2Context context)
    {
        _context = context;
    }

    // GET: Genres
    public async Task<IActionResult> Index()
    {
        return View(await _context.Genres.ToListAsync());
    }


    public async Task<IActionResult> MoviesByGenre(int genreId)
    {
        var moviesByGenreContext = _context.MoviesGenres
            .Where(mg => mg.GenreId == genreId)
            .Select(mg => mg.Movie);

        return View(await moviesByGenreContext.ToListAsync());
    }

    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres.FirstOrDefaultAsync(m => m.Id == id);

        if (genre == null)
        {
            return NotFound();
        }

        var moviesByGenre = _context.MoviesGenres
            .Where(mg => mg.GenreId == id)
            .Select(mg => mg.Movie).ToList();


        return RedirectToAction("MoviesByGenre", "Movies", new { genreId = id });
    }




    // GET: Genres/Create
    [Authorize(Roles = Accessibility.Roles)]

    public IActionResult Create()
    {
        return View();
    }

    // POST: Genres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Create([Bind("Name,Id")] Genre genre)
    {
        if (ModelState.IsValid)
        {
            if (!await IsGenreExist(genre.Name))
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This genre already exists.");
            }
        }
        return View(genre);
    }

    // GET: Genres/Edit/5
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }
        return View(genre);
    }

    // POST: Genres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Genre genre)
    {
        if (id != genre.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsGenreExist(genre.Name))
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
                ModelState.AddModelError(string.Empty, "This genre already exists.");
            }
        }
            return View(genre);
        
    }

    // GET: Genres/Delete/5
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    // POST: Genres/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            var mgs = _context.MoviesGenres
                .Where(mg => mg.GenreId == genre.Id)
                .ToList();

            foreach(var mg in mgs)
            {
                _context.MoviesGenres.Remove(mg);
            }

            _context.Genres.Remove(genre);

        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(int id)
    {
        return _context.Genres.Any(e => e.Id == id);
    }

    public async Task<bool> IsGenreExist(string name)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.Name == name);

        return genre != null;
    }

}
