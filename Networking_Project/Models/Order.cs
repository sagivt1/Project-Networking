using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public float Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime TimeOut { get; set; }

        public void CreateNewOrder()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO orders(user_email, price,is_paid,time_out) VALUES(?email,?Price,?paid,?TimeOut);";
            command.Parameters.AddWithValue("?email", Email);
            command.Parameters.AddWithValue("?Price", Price);
            command.Parameters.AddWithValue("?paid", false);
            command.Parameters.AddWithValue("?TimeOut", TimeOut);
            command.ExecuteNonQuery();

            command.CommandText = "SELECT LAST_INSERT_ID()";
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            OrderId = Convert.ToInt32(rdr[0]);
            connection.Close();
        }

        public void GetOrder(int orderId)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM orders WHERE id = ?id;";
            command.Parameters.AddWithValue("?id", orderId);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            OrderId = Convert.ToInt32(rdr[0]);
            Email = Convert.ToString(rdr[1]);
            Price = float.Parse(rdr[2].ToString());
            IsPaid = Convert.ToBoolean(rdr[3]);
            rdr.Close();
            connection.Close();
        }

        public void AddTicket(Ticket ticket)
        {
            Price += ticket.Price;
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE orders SET price = ?price WHERE id=?id;";
            command.Parameters.AddWithValue("?price", Price);
            command.Parameters.AddWithValue("?id", OrderId);
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO ticket VALUES(?showtimeid,?row,?col,?id,?tprice);";
            command.Parameters.AddWithValue("?showtimeid", ticket.ShowId);
            command.Parameters.AddWithValue("?row", ticket.SeatRow);
            command.Parameters.AddWithValue("?col", ticket.SeatCol);
            command.Parameters.AddWithValue("?tprice", ticket.Price);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void RemoveUnpaidOrders()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM orders WHERE time_out < NOW() AND is_paid = false;";
            MySqlDataReader rdr = command.ExecuteReader();
            List<int> MyList = new List<int>();
            while(rdr.Read())
            {
                MyList.Add(Convert.ToInt32(rdr[0]));
            }
            rdr.Close();
            foreach(var i in MyList)
            {
                command.Parameters.Clear();
                command.CommandText = "DELETE FROM orders WHERE id = ?orderid;";
                command.Parameters.AddWithValue("?orderid", i);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void RemoveTicket(int showid, int row, int col)
        {
            Ticket ticket = new Ticket();
            ticket.GetTicket(showid, row, col);
            Price = Price - ticket.Price;
            if(Price < 0 )
                Price = 0;
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE orders SET PRICE = ?price WHERE id = ?id;";
            command.Parameters.AddWithValue("?price", Price);
            command.Parameters.AddWithValue("id", OrderId);
            command.ExecuteNonQuery();
            command.Parameters.Clear();
            command.CommandText = "DELETE FROM ticket " +
                "WHERE showtime_id = ?showid AND seat_row = ?row AND seat_col = ?col ;";
            command.Parameters.AddWithValue("?showid", showid);
            command.Parameters.AddWithValue("?row", row);
            command.Parameters.AddWithValue("?col", col);
            command.ExecuteNonQuery();
        }

        public void SetOrderPaid()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE orders SET is_paid = true WHERE id = ?orderId; ";
            command.Parameters.AddWithValue("?orderId", OrderId);
            command.ExecuteNonQuery();

        }
    }
}