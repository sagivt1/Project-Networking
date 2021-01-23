using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace Networking_Project.Models
{
    public class User
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("Email")]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Bad email format")]
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Must contain only digit ")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        [DisplayName("Birthdate")]
        [Required]
        [RegularExpression("(^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$)")]
        public DateTime BirthDate { get; set; }


        public void AddToDB()
        {  
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO user (first_name, last_name, email, phone_number, password, is_admin, birth_date)  " +
                "values(?FirstName, ?LastName, ?Email, ?PhoneNumber, ?Password, ?false, ?BirthDate);";
            command.Parameters.AddWithValue("?FirstName", FirstName);
            command.Parameters.AddWithValue("?LastName", LastName);
            command.Parameters.AddWithValue("?Email", Email);
            command.Parameters.AddWithValue("?PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("?Password", Password);
            command.Parameters.AddWithValue("?false", false);
            command.Parameters.AddWithValue("?BirthDate", BirthDate);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void GetUser(string email)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM user where email = ?email";
            command.Parameters.AddWithValue("?email", email);
            MySqlDataReader rdr = command.ExecuteReader();

            if (rdr.Read())
            {
                FirstName = Convert.ToString(rdr[0]);
                LastName = Convert.ToString(rdr[1]);
                Email = Convert.ToString(rdr[2]);
                PhoneNumber = Convert.ToString(rdr[3]);
                Password = Convert.ToString(rdr[4]);
                isAdmin = Convert.ToBoolean(rdr[5]);
                BirthDate = Convert.ToDateTime(rdr[6]);
                connection.Close();
                rdr.Close();             
            }
            connection.Close();
            rdr.Close();
           
        }

        public void UpdateInDb()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE user SET " +
                "first_name = ?FirstName, " +
                "last_name = ?LastName, " +
                "phone_number = ?PhoneNumber, " +
                "password = ?Password, " +
                "birth_date = ?BirthDate " +
                "WHERE email = ?EMAIL;";
                
            
            command.Parameters.AddWithValue("?Email", Email);
            command.Parameters.AddWithValue("?FirstName", FirstName);
            command.Parameters.AddWithValue("?LastName", LastName);
            command.Parameters.AddWithValue("?PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("?Password", Password);
            command.Parameters.AddWithValue("?BirthDate", BirthDate);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}