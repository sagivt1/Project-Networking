using MySql.Data.MySqlClient;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class UserOrdersDal
    {
        public User user { get; set; }
        public List<Order> orders { get; set; }

        public void GetUserOrder()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM orders WHERE user_email = ?user;";
            command.Parameters.AddWithValue("?user", user.Email);
            MySqlDataReader rdr = command.ExecuteReader();
            orders = new List<Order>();
            while(rdr.Read())
            {
                Order order = new Order
                {
                    OrderId = Convert.ToInt32(rdr[0]),
                    Email = Convert.ToString(rdr[1]),
                    Price = float.Parse(rdr[2].ToString()),
                    IsPaid = Convert.ToBoolean(rdr[3]),
                    TimeOut = Convert.ToDateTime(rdr[4])
                };

                orders.Add(order);
            }
        }

    }
}