using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {

        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies
        public ActionResult Index()
        {

            var movie = _context.Movies.Include(m => m.Genre);


            return View(movie);
        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel()
            {
                Genres = genres,
                Title = "New Movie"

            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid) {

                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList(),
                    Title = "Customer Form"
                };
                    
                return View("MovieForm", viewModel);       
            }

            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);

            }
            else
            {
                var movieIndb = _context.Movies.Single(m => m.Id == movie.Id);


                movieIndb.Name = movie.Name;
                movieIndb.ReleaseDate = movie.ReleaseDate;
                movieIndb.GenreId = movie.GenreId;
                movieIndb.NumberInStock = movie.NumberInStock;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");

        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                
                Genres = _context.Genres.ToList(),

                Title = "Edit Movie"
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(c => c.Id == id);

            if (movie == null) {
                return HttpNotFound();
            }
            return View(movie);
        }


        //private IEnumerable<Movie> GetMovies()
        //{

        //    return new List<Movie>()
        //    {
        //        new Movie { Id = 1, Name = "Game of Thrones" },
        //        new Movie { Id = 2, Name = "The Last Kingdom" }
        //    };


        //}

        //public ActionResult Random()
        //{
        //    //var movie = new Movie()
        //    //{
        //    //    Name = "Shrek!"
        //    //};

        //    var customers = new List<Customer>
        //    {
        //        new Customer { Name = "Customer 1" },
        //        new Customer { Name = "Customer 2" }
        //    };


        //    var viewModel = new RandomMovieViewModel()
        //    {
        //        //Movie = movie,
        //        Customers = customers
        //    };

        //    return View(viewModel);
        //}






        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue)
        //        pageIndex = 1;

        //    if (String.IsNullOrWhiteSpace(sortBy))
        //        sortBy = "Name";

        //    return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        //}


        //public ActionResult Edit(int id) {
        //    return Content("id = " + id);
        //}

        //[Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        //public ActionResult ByReleaseDate(int year, int month) {
        //return Content(year + "/" + month);
        //}

        //public ActionResult learing()
        //{
        //return View(movie);
        //return Content("Hello world!!");
        //return HttpNotFound();
        //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" } );

        //ViewData["Movie"] = movie;
        //}

    }
}