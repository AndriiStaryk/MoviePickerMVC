using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

public class DirectorsController : Controller
{
    private readonly MoviePickerV2Context _context;
    private DirectorViewModel _directorViewModel;
    private Director _director = new Director();

    public DirectorsController(MoviePickerV2Context context)
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
    public async Task<IActionResult> Create([Bind("Name,BirthDate,BirthCountryId,Id")] Director director, IFormFile directorImage)
    {
        if (ModelState.IsValid)
        {
            if (!await IsDirectorExist(director.Name, director.BirthDate, director.BirthCountryId, directorImage))
            {

                if (directorImage != null && directorImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await directorImage.CopyToAsync(memoryStream);
                        director.DirectorImage = memoryStream.ToArray();
                    }
                }

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

    public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Director director, IFormFile? directorImage)
    {
        if (id != director.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsDirectorExist(director.Name, director.BirthDate, director.BirthCountryId, directorImage))
            {
                try
                {
                    var existingDirector = await _context.Directors.FindAsync(id);

                    if (existingDirector == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingDirector).State = EntityState.Detached;

                    if (directorImage != null && directorImage.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await directorImage.CopyToAsync(memoryStream);
                            director.DirectorImage = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        director.DirectorImage = existingDirector.DirectorImage;
                    }

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




    //public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Director director, IFormFile directorImage)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        if (!await IsDirectorExist(director.Name, director.BirthDate, director.BirthCountryId, directorImage))
    //        {
    //            try
    //            {
    //                var existingDirector = await _context.Directors.FindAsync(id);

    //                if (existingDirector == null)
    //                {
    //                    return NotFound();
    //                }

    //                _context.Entry(existingDirector).State = EntityState.Detached;

    //                if (directorImage != null && directorImage.Length > 0)
    //                {
    //                    using (var memoryStream = new MemoryStream())
    //                    {
    //                        await directorImage.CopyToAsync(memoryStream);
    //                        director.DirectorImage = memoryStream.ToArray();
    //                    }
    //                }
    //                else
    //                {
    //                    director.DirectorImage = existingDirector.DirectorImage;
    //                }

    //                _context.Update(director);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!DirectorExists(director.Id))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        else
    //        {
    //            ModelState.AddModelError(string.Empty, "This director already exists.");
    //        }
    //    }

    //    ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name", director.BirthCountryId);
    //    return View(director);
    //}

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


    public async Task<bool> IsDirectorExist(string name, DateOnly birthDate, int birthCountryID, IFormFile? directorImage)
    {
        byte[]? image = [];
        if (directorImage != null && directorImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await directorImage.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }

        var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == name &&
                                                                     d.BirthDate == birthDate &&
                                                                     d.BirthCountryId == birthCountryID);

        if (director != null && image != null && director.DirectorImage.SequenceEqual(image))
        {
            return true; 
        }

        return false;
    }


    public IActionResult MovieInfo(int movieId)
    {
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}
