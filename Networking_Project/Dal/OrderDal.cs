using MySql.Data.MySqlClient;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class OrderDal
    {
        public Order order { get; set; }
        public List<MovieTicket> MyList { get; set; }


        public void GetTicketOfOrder(int id)
        {
            order = new Order();
            MyList = new List<MovieTicket>();
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM orders WHERE id = ?id;";
            command.Parameters.AddWithValue("?id", id);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            order.OrderId = Convert.ToInt32(rdr[0]);
            order.Email = Convert.ToString(rdr[1]);
            order.Price = float.Parse(rdr[2].ToString());
            order.IsPaid = Convert.ToBoolean(rdr[3]);
            order.TimeOut = Convert.ToDateTime(rdr[4]);
            rdr.Close();

            command.CommandText = "SELECT * FROM ticket WHERE order_id = ?id;";
            rdr = command.ExecuteReader();
            while(rdr.Read())
            {
                MovieTicket mt = new MovieTicket();
                Ticket tic = new Ticket
                {
                    OrderId = Convert.ToInt32(rdr[3]),
                    ShowId = Convert.ToInt32(rdr[0]),
                    Price = order.Price = float.Parse(rdr[4].ToString()),
                    SeatRow = Convert.ToInt32(rdr[1]),
                    SeatCol = Convert.ToInt32(rdr[2])
                };
                mt.ticket = tic;
                mt.GetMovieTicket();
                MyList.Add(mt);
            }
            rdr.Close();
            connection.Close();
        }
    }
}