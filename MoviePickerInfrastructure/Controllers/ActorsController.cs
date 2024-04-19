using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;
using MoviePickerInfrastructure.Services;

namespace MoviePickerInfrastructure.Controllers;


public class ActorsController : Controller
{

    private readonly MoviePickerV2Context _context;
    private ActorViewModel _actorViewModel;
    private Actor _actor = new Actor();
    private ActorDataPortServiceFactory _actorDataPortServiceFactory;

    public ActorsController(MoviePickerV2Context context)
    {
        _context = context;
        _actorViewModel = new ActorViewModel(context, _actor);
        _actorDataPortServiceFactory = new ActorDataPortServiceFactory(context);
    }

    // GET: Actors
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Actors.Include(a => a.BirthCountry);
        return View(await moviePickerContext.ToListAsync());
    }


    public IActionResult MovieInfo(int movieId)
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

        //return View(actor);
    }

    // GET: Actors/Create

    [Authorize(Roles = Accessibility.Roles)]
    public IActionResult Create()
    {
        ViewData["BirthCountryId"] = new SelectList(_context.Countries, "Id", "Name");
        return View();
    }

    // POST: Actors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize(Roles = Accessibility.Roles)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,BirthDate,BirthCountryId,Id")] Actor actor, IFormFile? actorImage)
    {
        if (ModelState.IsValid)
        {
            if (!await ActorViewModel.IsActorExist(actor.Name, actor.BirthDate, actor.BirthCountryId, actorImage, _context))
            {

                if (actorImage != null && actorImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await actorImage.CopyToAsync(memoryStream);
                        actor.ActorImage = memoryStream.ToArray();
                    }
                }
                else
                {
                    string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
                    if (System.IO.File.Exists(imagePath))
                    {
                        byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                        actor.ActorImage = defaultImageBytes;
                    }
                }



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
    [Authorize(Roles = Accessibility.Roles)]

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
    [Authorize(Roles = Accessibility.Roles)]

    public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,BirthCountryId,Id")] Actor actor, IFormFile? actorImage)
    {
        if (id != actor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            bool isEditing = actorImage == null ? false : true;

            if (!await ActorViewModel.IsActorExist(actor.Name, actor.BirthDate, actor.BirthCountryId, actorImage, _context))
            {
                try
                {
                    var existingActor = await _context.Actors.FindAsync(id);

                    if (existingActor == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingActor).State = EntityState.Detached;

                    if (actorImage != null && actorImage.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await actorImage.CopyToAsync(memoryStream);
                            actor.ActorImage = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        actor.ActorImage = existingActor.ActorImage;
                    }

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
    [Authorize(Roles = Accessibility.Roles)]
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
    [Authorize(Roles = Accessibility.Roles)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var actor = await _context.Actors.FindAsync(id);
        

        if (actor != null)
        {
            _actorViewModel.Actor = actor;
            _actorViewModel.DeleteActor();
            //_context.Actors.Remove(actor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ActorExists(int id)
    {
        return _context.Actors.Any(e => e.Id == id);
    }



    [HttpGet]
    [Authorize(Roles = Accessibility.Roles)]
    public IActionResult Import()
    {

        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
    {
        
        try
        {
            if (fileExcel == null)
            {
                throw new Exception("Choose the file");
            }
            var importService = _actorDataPortServiceFactory.GetImportService(fileExcel.ContentType);

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
        var exportService = _actorDataPortServiceFactory.GetExportService(contentType);

        var memoryStream = new MemoryStream();

        await exportService.WriteToAsync(memoryStream, cancellationToken);

        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Position = 0;


        return new FileStreamResult(memoryStream, contentType)
        {
            FileDownloadName = $"actors_{DateTime.UtcNow.ToShortDateString()}.xlsx"
        };
    }
}
