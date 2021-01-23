using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Ticket
    {
        public int OrderId { get; set; }
        public int ShowId { get; set; }
        public float Price { get; set; }
        public int SeatRow { get; set; }
        public int SeatCol { get; set; }


        public void GetTicket(int showid, int row, int col)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ticket " +
                "WHERE showtime_id = ?showid AND seat_row = ?row AND seat_col = ?col; ";
            command.Parameters.AddWithValue("?showid", showid);
            command.Parameters.AddWithValue("?row", row);
            command.Parameters.AddWithValue("?col", col);
            MySqlDataReader rdr = command.ExecuteReader();
            if(rdr.Read())
            {
                ShowId = showid;
                SeatRow = row;
                SeatCol = col;
                OrderId = Convert.ToInt32(rdr[3]);
                Price = float.Parse(rdr[4].ToString());
            }
            rdr.Close();
            connection.Close();
        }

    }

   

    
}