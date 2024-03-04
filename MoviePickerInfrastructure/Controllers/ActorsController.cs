using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

public class ActorsController : Controller
{

    private readonly MoviePickerContext _context;
    private ActorViewModel _actorViewModel;

    public ActorsController(MoviePickerContext context)
    {
        _context = context;
    }

    // GET: Actors
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Actors.Include(a => a.BirthCountry);
        return View(await moviePickerContext.ToListAsync());
    }


    public async Task<IActionResult> MovieInfo(int movieId)
    {
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }


    // GET: Actors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actors
            .Include(a => a.BirthCountry)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (actor == null)
        {
            return NotFound();
        }

        _actorViewModel = new ActorViewModel(_context, actor);

        return View(_actorViewModel);

        return View(actor);
    }

    // GET: Actors/Create
    public IActionResult Create()
    {
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name");
        return View();
    }

    // POST: Actors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,BirthDate,BirthCountryId,Id")] Actor actor)
    {
        if (ModelState.IsValid)
        {
            if (!await IsActorExist(actor.Name, actor.BirthDate, actor.BirthCountryId))
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This actor already exists.");
            }
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", actor.BirthCountryId);
        return View(actor);
    }

    // GET: Actors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", actor.BirthCountryId);
        return View(actor);
    }

    // POST: Actors/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Actor actor)
    {
        if (id != actor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsActorExist(actor.Name, actor.BirthDate, actor.BirthCountryId))
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
                ModelState.AddModelError(string.Empty, "This actor already exists.");
            }
        }
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", actor.BirthCountryId);
        return View(actor);
    }

    // GET: Actors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actors
            .Include(a => a.BirthCountry)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (actor == null)
        {
            return NotFound();
        }

        return View(actor);
    }

    // POST: Actors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor != null)
        {
            _context.Actors.Remove(actor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ActorExists(int id)
    {
        return _context.Actors.Any(e => e.Id == id);
    }

    public async Task<bool> IsActorExist(string name, DateOnly birthDate, int birthCountryID)
    {
        var actor = await _context.Actors
            .FirstOrDefaultAsync(m => m.Name == name && m.BirthDate == birthDate && m.BirthCountryId == birthCountryID);

        return actor != null;
    }

}
