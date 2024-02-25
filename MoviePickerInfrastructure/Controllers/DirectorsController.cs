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
    public class DirectorsController : Controller
    {
        private readonly MoviePickerContext _context;

        public DirectorsController(MoviePickerContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            var moviePickerContext = _context.Directors.Include(d => d.BirthCountry);
            return View(await moviePickerContext.ToListAsync());
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(long? id)
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
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,BirthCountryId")] Director director)
        {
            //ViewBag.BirthCountryId = new SelectList(_context.Countries, "Id", "Name");
            //ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);

            //director.BirthCountryId = 1;
            if (ModelState.IsValid)
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(long? id)
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,BirthDate,BirthCountryId")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
            ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(long? id)
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(long id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
