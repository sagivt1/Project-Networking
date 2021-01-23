using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class MovieShow
    {
        public string MovieName { get; set; }
        public string Path { get; set; }
        public int Hall { get; set; }
        public DateTime Time { get; set; }


        public void GetFirstShow()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT movie_name, poster_path, hall_id, date_time " +
                "FROM movie INNER JOIN showtime " +
                "ON movie.id = showtime.movie_id " +
                "WHERE date_time > NOW() " +
                "ORDER BY date_time " +
                "LIMIT 1;";

            MySqlDataReader rdr = command.ExecuteReader();

            if (rdr.Read()) 
            { 
                MovieName = Convert.ToString(rdr[0]);
                Path = Convert.ToString(rdr[1]);
                Hall = Convert.ToInt32(rdr[2]);
                Time = Convert.ToDateTime(rdr[3]);
            }

            rdr.Close();
            connection.Close();
        }
    }

    
}