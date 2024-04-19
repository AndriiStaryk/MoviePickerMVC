using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

[Authorize(Roles = Accessibility.Roles)]
public class CountriesController : Controller
{
    private readonly MoviePickerV2Context _context;

    public CountriesController(MoviePickerV2Context context)
    {
        _context = context;
    }

    // GET: Countries
    public async Task<IActionResult> Index()
    {
        return View(await _context.Countries.ToListAsync());
    }

    // GET: Countries/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        return View(country);
    }

    // GET: Countries/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Countries/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Id")] Country country)
    {
        if (ModelState.IsValid)
        {
            if (!await IsCountryExist(country.Name))
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                ModelState.AddModelError(string.Empty, "This country already exists.");
            }
        }
        return View(country);
    }

    // GET: Countries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }
        return View(country);
    }

    // POST: Countries/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Country country)
    {
        if (id != country.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsCountryExist(country.Name))
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
                ModelState.AddModelError(string.Empty, "This country already exists.");
            }
        }
        return View(country);
    }

    // GET: Countries/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        return View(country);
    }

    // POST: Countries/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country != null)
        {
            _context.Countries.Remove(country);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CountryExists(int id)
    {
        return _context.Countries.Any(e => e.Id == id);
    }

    public async Task<bool> IsCountryExist(string name)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(m => m.Name == name);

        return country != null;
    }
}
