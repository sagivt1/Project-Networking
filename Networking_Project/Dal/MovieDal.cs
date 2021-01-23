using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Networking_Project.Models;

namespace Networking_Project.Dal
{
    public class MovieDal
    {
        public List<Movie> MyList { get; set; }
        public HashSet<string> Categores { get; set; }

        public void GetAllMovies(string OrderBy = "movie_name")
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM movie ORDER BY ?order;";
         
            
            Categores = new HashSet<string>();
            MyList = new List<Movie>();
            command.Parameters.AddWithValue("?order", OrderBy);
            MySqlDataReader rdr = command.ExecuteReader();
            
            while (rdr.Read())
            {
                Movie movie = new Movie();
                movie.ID = Convert.ToInt32(rdr[0]);
                movie.MovieName = Convert.ToString(rdr[1]);
                movie.Price = float.Parse(rdr[2].ToString());
                movie.Discount = Convert.ToInt32(rdr[3]);
                movie.MovieLength = Convert.ToInt32(rdr[4]);
                movie.AgeLimit = Convert.ToInt32(rdr[5]);
                movie.isOnSale = Convert.ToBoolean(rdr[6]);
                movie.LastShow = Convert.ToDateTime(rdr[7]);
                movie.PosterPath = Convert.ToString(rdr[8]);

                MyList.Add(movie);
            }
            rdr.Close();

            foreach(var movie in MyList)
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT description FROM category INNER JOIN movie_category " +
                   "ON category.id = movie_category.category_id INNER JOIN movie " +
                   "ON movie.id = movie_category.movie_id " +
                   "WHERE movie.id = ?movieID;";
                command.Parameters.AddWithValue("?movieID", movie.ID);
                rdr = command.ExecuteReader();
                movie.ListOfCategory = new List<string>();
                while (rdr.Read())
                {                  
                    string temp = Convert.ToString(rdr[0]);
                    movie.ListOfCategory.Add(temp);
                    Categores.Add(temp);
                }
                rdr.Close();
            }

            if(Categores.Contains(OrderBy))
            {
                List<Movie> movies = new List<Movie>();
      
                foreach(var movie in MyList)
                {
                    if(movie.ListOfCategory.Contains(OrderBy))
                    {
                        movies.Add(movie);
                    }
                }
                MyList = movies;
            }


            if(OrderBy == "Low To High" || OrderBy == "High To Low")
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    if(MyList[i].Discount > 0)
                    {
                        MyList[i].Price = MyList[i].Discount / 100 * MyList[i].Price;
                    }
                }
                if (OrderBy == "High To Low")
                {
                    MyList = MyList.OrderByDescending(x => x.Price).ToList();
                }
                else
                {
                    MyList = MyList.OrderBy(x => x.Price).ToList();
                }

            }

            if(OrderBy == "Most Popular" || OrderBy == "Less Popular")
            {
                for(int i = 0; i < MyList.Count; i++)
                {
                    command.Parameters.Clear();
                    command.CommandText = "SELECT count(*) FROM ticket INNER JOIN showtime " +
                        "ON ticket.showtime_id = showtime.id " +
                        "WHERE showtime.movie_id = ?movidId;";
                    command.Parameters.AddWithValue("?movidId", MyList[i].ID);
                    rdr = command.ExecuteReader();
                    rdr.Read();
                    MyList[i].TotalTicket = Convert.ToInt32(rdr[0]);
                    rdr.Close();
                }
                if(OrderBy == "Most Popular")
                {
                    MyList = MyList.OrderByDescending(x => x.TotalTicket).ToList();    
                }
                else
                {
                    MyList = MyList.OrderBy(x => x.TotalTicket).ToList();
                }
            }
            
            connection.Close();
        }
    }
}