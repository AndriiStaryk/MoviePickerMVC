using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;
using MoviePickerInfrastructure.Services;

namespace MoviePickerInfrastructure.Controllers;

public class DirectorsController : Controller
{
    private readonly MoviePickerV2Context _context;
    private DirectorViewModel _directorViewModel;
    private Director _director = new Director();
    private DirectorDataPortServiceFactory _directorDataPortServiceFactory;

    public DirectorsController(MoviePickerV2Context context)
    {
        _context = context;
        _directorViewModel = new DirectorViewModel(context, _director);
        _directorDataPortServiceFactory = new DirectorDataPortServiceFactory(context);
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
    [Authorize(Roles = Accessibility.Roles)]

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
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Create([Bind("Name,BirthDate,BirthCountryId,Id")] Director director, IFormFile? directorImage)
    {
        if (ModelState.IsValid)
        {
            if (!await DirectorViewModel.IsDirectorExist(director.Name,
                                                         director.BirthDate,
                                                         director.BirthCountryId,
                                                         directorImage,
                                                         _context))
            {

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
                    string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
                    if (System.IO.File.Exists(imagePath))
                    {
                        byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                        director.DirectorImage = defaultImageBytes;
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
    [Authorize(Roles = Accessibility.Roles)]

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
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Director director, IFormFile? directorImage)
    {
        if (id != director.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await DirectorViewModel.IsDirectorExist(director.Name,
                                                         director.BirthDate,
                                                         director.BirthCountryId,
                                                         directorImage,
                                                         _context))
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


    // GET: Directors/Delete/5
    [Authorize(Roles = Accessibility.Roles)]

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
    [Authorize(Roles = Accessibility.Roles)]

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


    [HttpGet]
    [Authorize(Roles = Accessibility.Roles)]

    public IActionResult Import()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Accessibility.Roles)]


    public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
    {

        try
        {
            if (fileExcel == null)
            {
                throw new Exception("Choose the file");
            }
            var importService = _directorDataPortServiceFactory.GetImportService(fileExcel.ContentType);

            using var stream = fileExcel.OpenReadStream();

            await importService.ImportFromStreamAsync(stream, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {

            ViewBag.ErrorMessage = ex.Message;
            return View();
        }


    }

    [HttpGet]
    public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    CancellationToken cancellationToken = default)
    {
        var exportService = _directorDataPortServiceFactory.GetExportService(contentType);

        var memoryStream = new MemoryStream();

        await exportService.WriteToAsync(memoryStream, cancellationToken);

        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Position = 0;


        return new FileStreamResult(memoryStream, contentType)
        {
            FileDownloadName = $"directors_{DateTime.UtcNow.ToShortDateString()}.xlsx"
        };
    }


    public IActionResult MovieInfo(int movieId)
    {
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}
