using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Controllers;

public class MoviesController : Controller
{
    private readonly MoviePickerContext _context;
    private Movie _movie = new Movie();
    private readonly List<Genre> _genres = new List<Genre>();

    public MoviesController(MoviePickerContext context)
    {
        _context = context;
        _genres = _context.Genres.ToList();
    }

    // GET: Movies
    public async Task<IActionResult> Index()
    {
        var moviePickerContext = _context.Movies.Include(m => m.Director);
        return View(await moviePickerContext.ToListAsync());
    }

    public async Task<IActionResult> MoviesByGenre(int genreId)
    {
        // Find all movies associated with the given genreId
        var movies = await _context.Movies
            .Where(m => m.MoviesGenres.Any(mg => mg.GenreId == genreId))
            .ToListAsync();

        return View(movies);
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .Include(m => m.Director)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        MovieViewModel viewModel = new MovieViewModel();
        viewModel.Movie = _movie;
        viewModel.Genres = _genres;
        // viewModel.Context = _context;
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
        //return View();
        return View(viewModel);
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie/*, [FromForm] MovieViewModel movieViewModel*/ /*[Bind("Movie, Genres")] MovieViewModel movieViewModel*/)
    {
        var movieViewModel = new MovieViewModel();
        movieViewModel.Movie = movie;
        movieViewModel.Genres = _genres;

        //var genres = movieViewModel.Genres.ToList();


        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId))
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();


                foreach (var genre in movieViewModel.SelectedGenres)
                {
                    movie.MoviesGenres.Add(new MoviesGenre { GenreId = genre.Id });
                    //post.PostCategories.Add(new PostCategory { CategoryId = CategoryId });
                }
                _context.SaveChanges();

                //movieViewModel.AddSelectedGenres(_context);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This movie already exists.");
            }
        }
        //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
        return View(movieViewModel);
    }


    



    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Title,ReleaseDate,DirectorId,Budget,BoxOfficeRevenue,Duration,Rating,Id")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await IsMovieExist(movie.Title, movie.ReleaseDate, movie.DirectorId))
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
                ModelState.AddModelError(string.Empty, "This movie already exists.");
            }
        }
        ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
        return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .Include(m => m.Director)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }

    public async Task<bool> IsMovieExist(string title, DateOnly releaseDate, int directorID)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Title == title && m.ReleaseDate == releaseDate && m.DirectorId == directorID);

        return movie != null;
    }
}



//}

//public class MoviesController : Controller
//{

//    private readonly AppDbContext dbContext;
//    private readonly IWebHostEnvironment hostingEnvironment;
//    private readonly IFileService fileService;

//    public IWebHostEnvironment HostingEnvironment => hostingEnvironment;

//    public PostsController(AppDbContext dbContext, IWebHostEnvironment environment,
//        IFileService fileService)
//    {
//        this.dbContext = dbContext;
//        hostingEnvironment = environment;
//        this.fileService = fileService;
//    }
//    public static string getpath;
//    public IActionResult Index()
//    {

//        var post = dbContext.Posts.Include(p => p.PostCategories)
//            .ThenInclude(c => c.Category)
//            .OrderByDescending(p => p.PostId);

//        return View(post);
//    }

//    [HttpGet]
//    public IActionResult Details(int? id)
//    {
//        var post = dbContext.Posts.Include(p => p.PostCategories)
//            .ThenInclude(c => c.Category).FirstOrDefault(m => m.PostId == id);
//        return View(post);
//    }

//    [HttpGet]
//    public IActionResult Delete(int? id)
//    {
//        var Post = dbContext.Posts.FirstOrDefaultAsync(c => c.PostId == id);
//        return View();

//    }

//    [HttpPost]
//    public IActionResult Delete(int id)
//    {
//        var post = dbContext.Posts.Find(id);
//        dbContext.Posts.Remove(post);
//        dbContext.SaveChanges();
//        return RedirectToAction("Index");

//    }

//    [HttpGet]
//    public IActionResult Create(int? id)
//    {
//        if (id != null)
//        {
//            var post = dbContext.Posts
//                .Include(p => p.PostCategories)
//                    .Where(p => p.PostId == id).FirstOrDefault();
//            if (post == null) return View();

//            var PostVM = new PostCreateVM()
//            {
//                PostId = post.PostId,
//                Title = post.Title,
//                Description = post.Description,
//                EditImagePath = post.DisplayImage,
//                Categories = dbContext.Categories.ToList(),
//                SelectedCategory = post.PostCategories.Select(pc => pc.CategoryId).ToList()

//            };
//            ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryId", "CategoryName");
//            return View(PostVM);
//        }
//        else
//        {
//            var post = new PostCreateVM();
//            post.Categories = dbContext.Categories.ToList();
//            ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryId", "CategoryName");
//            return View(post);

//        }
//    }

//    [HttpPost]

//    public IActionResult Create([FromForm] PostCreateVM vm, int? id)
//    {
//        if (id != null)
//        {
//            var post = dbContext.Posts.Include(p => p.PostCategories)
//                .Where(p => p.PostId == id).FirstOrDefault();

//            if (vm.DisplayImage != null)
//            {
//                post.DisplayImage = fileService.Upload(vm.DisplayImage);
//                var fileName = fileService.Upload(vm.DisplayImage);
//                post.DisplayImage = fileName;
//                getpath = fileName;
//            }
//            post.Title = vm.Title;
//            post.Description = vm.Description;
//            post.PostCategories = new List<PostCategory>();

//            foreach (var CategoryId in vm.SelectedCategory)
//            {
//                post.PostCategories.Add(new PostCategory { CategoryId = CategoryId });
//            }
//            dbContext.SaveChanges();
//            TempData["Editmessage"] = "Edited Successfully";
//            return RedirectToAction("Index");
//        }
//        else
//        {
//            Post post = new Post()
//            {

//                PostId = vm.PostId,
//                Description = vm.Description,
//                Title = vm.Title,
//                DisplayImage = vm.DisplayImage.FileName,
//            };
//            if (ModelState.IsValid)
//            {
//                var fileName = fileService.Upload(vm.DisplayImage);
//                post.DisplayImage = fileName;
//                getpath = fileName;

//                dbContext.Add(post);
//                dbContext.SaveChanges();
//            }
//            foreach (var cat in vm.SelectedCategory)
//            {

//                PostCategory postCategory = new PostCategory();
//                postCategory.PostId = post.PostId;
//                postCategory.CategoryId = cat;
//                dbContext.Add(postCategory);
//                dbContext.SaveChanges();
//            }
//            TempData["message"] = "Successfully Added";
//            return RedirectToAction("Index");
//        }

//    }
//}