using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Networking_Project.Models;
using Networking_Project.Dal;


namespace Networking_Project.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserHomePage()
        {
            HttpCookie MyCookie = Request.Cookies["UserEmail"];
            if(MyCookie.Value == null || MyCookie.Value == "")
            {
                return RedirectToAction("Login", "Home");
            }
            User u = new User();
            u.GetUser(MyCookie.Value);
            MovieShow ms = new MovieShow();
            ms.GetFirstShow();
            UserMovieShowDal umsd = new UserMovieShowDal
            {
                user = u,
                movieShow = ms
            };

            return View(umsd);
        }

        public ActionResult Account()
        {
            HttpCookie MyCookie = Request.Cookies["UserEmail"];
            User user = new User();
            user.GetUser(MyCookie.Value);
            UserOrdersDal uo = new UserOrdersDal();
            uo.user = user;
            uo.GetUserOrder();
            return View(uo);
        }

        public ActionResult Cart()
        {
            HttpCookie MyCookie = Request.Cookies["OrderID"];
            if (MyCookie == null)
            {
                return View();
            }
            OrderDal od = new OrderDal();
            od.GetTicketOfOrder(Convert.ToInt32(MyCookie.Value));
            if (od.order.IsPaid)
            {

                return View();
            }
            return View(od);
        }
        [HttpGet]
        public ActionResult Movies(string OrderBy = "movie_name")
        {
            if (OrderBy == "0")
                OrderBy = "movie_name";
            Order.RemoveUnpaidOrders();
            MovieDal m = new MovieDal();
            m.GetAllMovies(OrderBy);
            return View(m);
        }

        public ActionResult Movie(int id)
        {
            Movie movie = new Movie();
            movie.SetMovie(id);
            if (movie.isOnSale)
                ViewData["DescountPrice"] = movie.Price * (float)(1.0 - (movie.Discount / 100.0));
            MoviesShowDal MovieAndShows = new MoviesShowDal();
            MovieAndShows.movie = movie;
            MovieAndShows.GetAllShows();
            return View(MovieAndShows);
        }

        public ActionResult EditUserDetails(User user)
        {
            user.UpdateInDb();
            return View("Account",user);
        }

        public ActionResult Edit()
        {
            HttpCookie MyCookie = Request.Cookies["UserEmail"];
            User user = new User();
            user.GetUser(MyCookie.Value);
            return View(user);
        }

        public ActionResult LogOut()
        {
            HttpCookie MyCookie = Request.Cookies["UserEmail"];
            if(MyCookie == null)
            {
                return RedirectToAction("Home", "Home");
            }
            MyCookie.Value = "";
            MyCookie.Expires = DateTime.Now.AddSeconds(1);
            Response.Cookies.Add(MyCookie);
            return RedirectToAction("HomePage", "Home");
        }

        [HttpGet]
        public ActionResult BuyTicket(int ShowId)
        {
            MovieShowDal ms = new MovieShowDal();
            ms.GetShow(ShowId);
            return View(ms);
        }

        [HttpPost]
        public ActionResult BuyTicket()
        {
            string selected = Request.Form["mobil[1][]"];
            string[] values = new string[3];
            values = selected.Split(',');
            int row = Convert.ToInt32(values[0]);
            int col = Convert.ToInt32(values[1]);
            int ShowId = Convert.ToInt32(values[2]);
            float price = float.Parse(values[3].ToString());
            Ticket ticket = new Ticket
            {
                ShowId = ShowId,
                SeatCol = col,
                SeatRow = row,
                Price = price
            };
            HttpCookie MyUser = Request.Cookies["UserEmail"];
            Order order;
            HttpCookie MyCookie = Request.Cookies["OrderID"];
            if (MyCookie == null || MyCookie.Value == "")
            {
                MyCookie = new HttpCookie("OrderID");
                order = new Order
                {
                    Email = MyUser.Value,
                    Price = 0,
                    TimeOut = DateTime.Now.AddMinutes(15)
                };
                order.CreateNewOrder();
                MyCookie.Value = order.OrderId.ToString();
                Response.Cookies.Add(MyCookie);
            }
            else
            {
                order = new Order();
                order.GetOrder(Convert.ToInt32(MyCookie.Value));
            }
            MyCookie.Expires = DateTime.Now.AddMinutes(15);
            order.AddTicket(ticket);

            return RedirectToAction("Cart");
        }

        public ActionResult RemoveFromCart(int ShowId, int SeatRow, int SeatCol)
        {
            HttpCookie MyCookie = Request.Cookies["OrderID"];
            if (MyCookie == null || MyCookie.Value == "")
            {
                return RedirectToAction("Cart");
            }
            Order order = new Order();
            order.GetOrder(Convert.ToInt32(MyCookie.Value));
            order.RemoveTicket(ShowId, SeatRow, SeatCol);

            return RedirectToAction("Cart");
        }

        public ActionResult Pay()
        {
            return View();
        }
        public ActionResult Payed(CreditCard card)
        {
            HttpCookie MyCookie = Request.Cookies["OrderID"];
            if (MyCookie == null)
            {
                return RedirectToAction("UserHomePage");
            }


            Order order = new Order();
            order.GetOrder(Convert.ToInt32(MyCookie.Value));
            order.SetOrderPaid();
            MyCookie.Expires = DateTime.Now.AddSeconds(1);
            MyCookie.Value = "";
            Response.Cookies.Add(MyCookie);


            return View("ThankYou");
        }

        public ActionResult PayFailed()
        {
            return View();
        }



    }
}