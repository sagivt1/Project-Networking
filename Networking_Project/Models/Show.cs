using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Show
    {
        public int ID { get; set; }
        public int hall { get; set; }
        public int movie { get; set; }
        public DateTime dateTime { get; set; }


        public void GetShow(int ShowId)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime WHERE id = ?id;";
            command.Parameters.AddWithValue("?id", ShowId);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            ID = Convert.ToInt32(rdr[0]);
            hall = Convert.ToInt32(rdr[1]);
            movie = Convert.ToInt32(rdr[2]);
            dateTime = Convert.ToDateTime(rdr[3]);
            rdr.Close();
            connection.Close();
        }
        public void AddToDb()
        {         
            DateTime t = new DateTime();
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT date_time FROM showtime WHERE movie_id = ?MovieId ORDER BY date_time DESC LIMIT 1;";
            command.Parameters.AddWithValue("?MovieId", movie);
            MySqlDataReader rdr = command.ExecuteReader();
            if (rdr.Read())
            {
                t = Convert.ToDateTime(rdr[0]);
            }
            else
            {
                command.CommandText = "UPDATE movie SET last_show = ?showdate WHERE id = ?MovieId;";
                command.Parameters.AddWithValue("?showdate", dateTime);
            }
            rdr.Close();

                command.CommandText = "INSERT INTO showtime(hall_id, movie_id, date_time)" +
                "VALUES(?Hallid,?MovieId,?DateTime);";
            command.Parameters.AddWithValue("?Hallid", hall);
            
            command.Parameters.AddWithValue("?DateTime", dateTime);
            command.ExecuteNonQuery();

            if (t < dateTime)
            {
                command.CommandText = "UPDATE movie SET last_show = ?showdate WHERE id = ?MovieId;";
                command.ExecuteNonQuery();
            }
           
            connection.Close();
        }

        public static void RemoveFromDbById(int id)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM showtime WHERE id = ?id;";
            command.Parameters.AddWithValue("?id", id);
            command.ExecuteNonQuery();
            connection.Close();

        }

    }


}