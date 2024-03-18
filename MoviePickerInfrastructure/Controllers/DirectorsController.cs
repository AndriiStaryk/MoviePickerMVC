using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

public class DirectorsController : Controller
{
    private readonly MoviePickerContext _context;
    private DirectorViewModel _directorViewModel;
    private Director _director = new Director();

    public DirectorsController(MoviePickerContext context)
    {
        _context = context;
        _directorViewModel = new DirectorViewModel(context, _director);
    }

    // GET: Directors
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Directors.Include(d => d.BirthCountry);
        return View(await moviePickerContext.ToListAsync());
    }

    // GET: Directors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directors
            .Include(d => d.BirthCountry)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (director == null)
        {
            return NotFound();
        }

        _directorViewModel = new DirectorViewModel(_context, director);

        return View(_directorViewModel);
    }

    // GET: Directors/Create
    public IActionResult Create()
    {
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name");
        return View();
    }

    // POST: Directors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,BirthDate,BirthCountryId,Id")] Director director)
    {
        if (ModelState.IsValid)
        {
            if (!await IsDirectorExist(director.Name, director.BirthDate, director.BirthCountryId))
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This director already exists.");
            }
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
        return View(director);
    }

    // GET: Directors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directors.FindAsync(id);
        if (director == null)
        {
            return NotFound();
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
        return View(director);
    }

    // POST: Directors/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Director director)
    {
        if (id != director.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsDirectorExist(director.Name, director.BirthDate, director.BirthCountryId))
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
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
                ModelState.AddModelError(string.Empty, "This director already exists.");
            }
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
        return View(director);
    }

    // GET: Directors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directors
            .Include(d => d.BirthCountry)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (director == null)
        {
            return NotFound();
        }

        return View(director);
    }

    // POST: Directors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director != null)
        {
            _directorViewModel.Director = director;
            _directorViewModel.DeleteDirector();
            //_context.Directors.Remove(director);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DirectorExists(int id)
    {
        return _context.Directors.Any(e => e.Id == id);
    }


    public async Task<bool> IsDirectorExist(string name, DateOnly birthDate, int birthCountryID)
    {
        var director = await _context.Directors
            .FirstOrDefaultAsync(m => m.Name == name && m.BirthDate == birthDate && m.BirthCountryId == birthCountryID);

        return director != null;
    }


    public IActionResult MovieInfo(int movieId)
    {
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}
