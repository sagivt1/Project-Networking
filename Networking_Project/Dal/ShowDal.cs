using MySql.Data.MySqlClient;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class ShowDal
    {
        public List<Show> MyList { get; set; }

        public void GetAllHallShow(int hall)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime Where hall_id = ?HallId AND date_time > NOW();";
            command.Parameters.AddWithValue("?HallId", hall);
            MySqlDataReader rdr = command.ExecuteReader();
            MyList = new List<Show>();
            while(rdr.Read())
            {
                Show show = new Show();
                show.ID = Convert.ToInt32(rdr[0]);
                show.hall = Convert.ToInt32(rdr[1]);
                show.movie = Convert.ToInt32(rdr[2]);
                show.dateTime = Convert.ToDateTime(rdr[3]);
                MyList.Add(show);
            }
            rdr.Close();
            connection.Close();
        }

        public void GetAllShows()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime Where date_time > NOW();";
            MySqlDataReader rdr = command.ExecuteReader();
            MyList = new List<Show>();
            while (rdr.Read())
            {
                Show show = new Show();
                show.ID = Convert.ToInt32(rdr[0]);
                show.hall = Convert.ToInt32(rdr[1]);
                show.movie = Convert.ToInt32(rdr[2]);
                show.dateTime = Convert.ToDateTime(rdr[3]);
                MyList.Add(show);
            }
            rdr.Close();
            connection.Close();
        }
    }
}