using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Networking_Project.Models;


namespace Networking_Project.Dal
{
    public class HallDal
    {
        public List<Hall> MyList { get; set; }

        public void GetAllHalls()
        {
            MyList = new List<Hall>();
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM hall;";
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                Hall hall = new Hall();
                hall.ID = Convert.ToInt32(rdr[0]);
                hall.TotalRow = Convert.ToInt32(rdr[1]);
                hall.TotalCol = Convert.ToInt32(rdr[2]);
                MyList.Add(hall);
            }
            connection.Close();
            rdr.Close();
        }

    }
}