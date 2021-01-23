using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class MovieTicket
    {
        public Ticket ticket { get; set; }
        public Show show { get; set; }
        public Movie movie { get; set; }

        public void GetMovieTicket()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime " +
                "INNER JOIN movie ON movie.id = showtime.movie_id " +
                "WHERE showtime.id = ?tshowid ;";
            command.Parameters.AddWithValue("?tshowid", ticket.ShowId);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            show = new Show();
            movie = new Movie();
            show.ID = Convert.ToInt32(rdr[0]);
            show.hall = Convert.ToInt32(rdr[1]);
            show.movie = Convert.ToInt32(rdr[2]);
            show.dateTime = Convert.ToDateTime(rdr[3]);
            movie.ID = Convert.ToInt32(rdr[4]);
            movie.MovieName = Convert.ToString(rdr[5]);
            movie.Price = float.Parse(rdr[6].ToString());
            movie.Discount = Convert.ToInt32(rdr[7]);
            movie.MovieLength = Convert.ToInt32(rdr[8]);
            movie.AgeLimit = Convert.ToInt32(rdr[9]);
            movie.isOnSale = Convert.ToBoolean(rdr[10]);
            movie.LastShow = Convert.ToDateTime(rdr[11]);
            movie.PosterPath = Convert.ToString(rdr[12]);
            rdr.Close();
            connection.Close();
        }
    }
}