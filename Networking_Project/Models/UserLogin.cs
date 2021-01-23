using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Networking_Project.Models
{
    public class UserLogin
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Bad email format")]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }

        public bool LogIn()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT password FROM user where email = ?email";
            command.Parameters.AddWithValue("?email", Email);
            MySqlDataReader rdr = command.ExecuteReader();
            
            if (rdr.Read())
            {
                string pass = Convert.ToString(rdr[0]);
                if (pass.Equals(Password))
                {
                    connection.Close();
                    rdr.Close();
                    return true;
                }
            }

            connection.Close();
            rdr.Close();
            return false;
        }

        public User GetUser()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM user where email = ?email";
            command.Parameters.AddWithValue("?email", Email);
            MySqlDataReader rdr = command.ExecuteReader();


            if (rdr.Read())
            {
                User user = new User();
                user.FirstName = Convert.ToString(rdr[0]);
                user.LastName = Convert.ToString(rdr[1]);
                user.Email = Convert.ToString(rdr[2]);
                user.PhoneNumber = Convert.ToString(rdr[3]);
                user.Password = Convert.ToString(rdr[4]);
                user.isAdmin = Convert.ToBoolean(rdr[5]);
                user.BirthDate = Convert.ToDateTime(rdr[6]);
                connection.Close();
                rdr.Close();
                return user;
            }
            connection.Close();
            rdr.Close();
            return null;
        }
    }
}