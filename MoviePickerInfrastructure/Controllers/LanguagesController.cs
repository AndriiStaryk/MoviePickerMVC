using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

[Authorize(Roles = Accessibility.Roles)]
public class LanguagesController : Controller
{
    private readonly MoviePickerV2Context _context;

    public LanguagesController(MoviePickerV2Context context)
    {
        _context = context;
    }

    // GET: Languages
    public async Task<IActionResult> Index()
    {
        return View(await _context.Languages.ToListAsync());
    }

    // GET: Languages/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var language = await _context.Languages
            .FirstOrDefaultAsync(m => m.Id == id);
        if (language == null)
        {
            return NotFound();
        }

        return View(language);
    }

    // GET: Languages/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Languages/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Id")] Language language)
    {
        if (ModelState.IsValid)
        {
            if (!await IsLanguageExist(language.Name))
            {
                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This language already exists.");
            }

        }
        return View(language);
    }

    // GET: Languages/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var language = await _context.Languages.FindAsync(id);
        if (language == null)
        {
            return NotFound();
        }
        return View(language);
    }

    // POST: Languages/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Language language)
    {
        if (id != language.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsLanguageExist(language.Name))
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.Id))
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
                ModelState.AddModelError(string.Empty, "This language already exists.");
            }
        }
        return View(language);

    }

    // GET: Languages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var language = await _context.Languages
            .FirstOrDefaultAsync(m => m.Id == id);
        if (language == null)
        {
            return NotFound();
        }

        return View(language);
    }

    // POST: Languages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var language = await _context.Languages.FindAsync(id);
        if (language != null)
        {
            _context.Languages.Remove(language);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LanguageExists(int id)
    {
        return _context.Languages.Any(e => e.Id == id);
    }

    public async Task<bool> IsLanguageExist(string name)
    {
        var language = await _context.Languages
            .FirstOrDefaultAsync(m => m.Name == name);

        return language != null;
    }

}
