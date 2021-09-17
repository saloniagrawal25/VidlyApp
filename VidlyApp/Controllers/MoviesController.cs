using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidlyApp.Models;
using VidlyApp.ViewModels;
using System.Data.Entity;

namespace VidlyApp.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Movies
        public ActionResult Index()
        {
            return View(_context.Movies.Include(m => m.Genre).ToList());
        }
        public ActionResult Details(int id)
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();
            if (movies != null && movies.Count() > 0)
            {
                var movie = movies.SingleOrDefault(x => x.Id == id);
                if (movie != null)
                {
                    return View(movie);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
                return HttpNotFound();
        }

        public ActionResult MovieForm()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);
            }
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Today;
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(x => x.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumInStock = movie.NumInStock;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(x => x.Id == id);
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = genres
            };
            return View("MovieForm", viewModel);
        }
    }
}