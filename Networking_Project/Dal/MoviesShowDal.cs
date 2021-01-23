using MySql.Data.MySqlClient;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class MoviesShowDal
    {
        public Movie movie { get; set; }
        public List<Show> MyShows { get; set; }


        public void GetAllShows()
        {
            MyShows = new List<Show>();
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime WHERE date_time > NOW()" +
                "AND movie_id = ?MovieId;";
            command.Parameters.AddWithValue("?MovieId", movie.ID);
            MySqlDataReader rdr = command.ExecuteReader();

            while(rdr.Read())
            {
                Show show = new Show
                {
                    ID = Convert.ToInt32(rdr[0]),
                    hall = Convert.ToInt32(rdr[1]),
                    movie = Convert.ToInt32(rdr[2]),
                    dateTime = Convert.ToDateTime(rdr[3])
                };
                MyShows.Add(show);
            }
            rdr.Close();
            connection.Close();

        }
    }
}