using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Networking_Project.Models;
using Networking_Project.Dal;


namespace Networking_Project.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminHomePage()
        {
            MovieShow Movie = new MovieShow();
            Movie.GetFirstShow();
            return View(Movie);
        }

        [HttpGet]
        public ActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMovie(Movie movie)
        {
            string filename = movie.MovieName;
            string extenstion = System.IO.Path.GetExtension(movie.ImageFile.FileName);
            filename = filename + extenstion;
            movie.PosterPath = "/Images/" + filename;
            filename = System.IO.Path.Combine(Server.MapPath("~/Images/"), filename);
            movie.ImageFile.SaveAs(filename);
            if(movie.Discount != 0)
            {
                movie.isOnSale = true;
            }
            movie.AddMovie();
            return View("AdminHomePage");
        }

        [HttpGet]
        public ActionResult RemoveMovie()
        {
            MovieDal movies = new MovieDal();
            movies.GetAllMovies();
            return View(movies);
        }

        [HttpPost]
        public ActionResult RemoveMovie(string MovieN)
        {
            Movie.RemoveMovie(MovieN);
            return View("AdminHomePage");
        }



        public ActionResult EditMovie()
        {
            return View();
        }
        [Route("Admin/AddShow/{msg}")]
        [HttpGet]
        public ActionResult AddShow(string msg)
        {
            if (msg != null)
            {
                ViewData["msg"] = msg;
            }
            MovieDal movies = new MovieDal();
            movies.GetAllMovies();        
            HallDal halls = new HallDal();
            halls.GetAllHalls();
            MoviesAndHallsDal data = new MoviesAndHallsDal();
            data.Halls = halls;
            data.Movies = movies;
            return View(data);
        }
        [HttpPost]
        public ActionResult AddShow(int hallId, string MovieName, DateTime time = default)
        {

            if(hallId == 0 || MovieName == "0" || time == null)
            {
                string msg = "You need to fill the details of the new show";
                return RedirectToAction("AddShow", "Admin", new { msg });
            }
           
            if (time < DateTime.Now.AddDays(1))
            {
                string msg = "Adding show time need to be 24 Hour before show!!";
                return RedirectToAction("AddShow", "Admin", new { msg });
            }
            ShowDal sd = new ShowDal();
            sd.GetAllHallShow(hallId);
            foreach (Show s in sd.MyList)
            {
                Movie m = new Movie();
                m.GetMovieById(s.movie);
                
                if (Math.Abs(s.dateTime.Subtract(time).TotalMinutes) < m.MovieLength + 30)
                {
                    string msg = "Invalid Date Or Time";
                    return RedirectToAction("AddShow", "Admin", new { msg });
                }

            }
             Show show = new Show
            {
                hall = hallId,
                movie = Movie.GetIdByName(MovieName),
                dateTime = time
            };
            show.AddToDb();

            return RedirectToAction("AdminHomePage","Admin");
        }

        public ActionResult EditShow()
        {
            return View();
        }

        public ActionResult RemoveShow()
        {
            ShowDal shows = new ShowDal();
            shows.GetAllShows();
            return View(shows);
        }

        [HttpPost]
        public ActionResult RemoveShow(int ShowId)
        {
            Show.RemoveFromDbById(ShowId);
            return View("AdminHomePage");
        }

        

    }
}