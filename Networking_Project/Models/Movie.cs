using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Networking_Project.Models
{
    public class Movie
    {
        [DisplayName("Movie Name")]
        [Required]
        public string MovieName { get; set; }
        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Only positive number allowed")]
        public float Price { get; set; }
        [DisplayName("Discount In Percentage")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Discount { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [DisplayName("Movie Length")]
        public int MovieLength { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{1,2}$")]
        [DisplayName("Age Limit")]
        public int AgeLimit { get; set; }
        [DisplayName("Sale")]
        public bool isOnSale { get; set; }
        public DateTime LastShow { get; set; }
        [DisplayName("Upload File")]
        public string PosterPath { get; set; }
        [DisplayName("Category")]
        public string Category { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public int ID { get; set; }
        public List<String> ListOfCategory { get; set; }
        public int TotalTicket { get; set; }



        public void AddMovie()
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "" +
                "INSERT INTO movie(movie_name, price, discount, movie_length, age_limit, is_on_sale, last_show, poster_path)" +
                "VALUES(?MovieName, ?Price, ?Discount, ?MovieLength, ?AgeLimit, ?isOnSale, ?LastShow, ?PosterPath);";
            command.Parameters.AddWithValue("?MovieName", MovieName);
            command.Parameters.AddWithValue("?Price",Price);
            command.Parameters.AddWithValue("?Discount", Discount);
            command.Parameters.AddWithValue("?MovieLength", MovieLength);
            command.Parameters.AddWithValue("?AgeLimit", AgeLimit);
            command.Parameters.AddWithValue("?isOnSale", isOnSale);
            command.Parameters.AddWithValue("?LastShow", LastShow);
            command.Parameters.AddWithValue("?PosterPath", PosterPath);
            command.ExecuteNonQuery();

            command.CommandText = "SELECT id FROM movie WHERE movie_name = ?MovieName;";
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            ID = Convert.ToInt32(rdr[0]);
            rdr.Close();
            

            List<string> ListOfCategory = Category.Split(',').ToList();
            
            foreach(var cat in ListOfCategory)
            {
                string temp = string.Join("", cat.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                command.Parameters.Clear();
                command.CommandText = "INSERT IGNORE INTO category(description) VALUES(?catg)";
                command.Parameters.AddWithValue("?catg", temp);  
                command.ExecuteNonQuery();

                command.CommandText = "SELECT id FROM category WHERE description = ?catg;";
                rdr = command.ExecuteReader();
                rdr.Read();
                int CategoryID = Convert.ToInt32(rdr[0]);
                rdr.Close();

                command.CommandText = "INSERT INTO movie_category(category_id, movie_id)" +
                    "VALUES(?catID,?ID)";
                command.Parameters.AddWithValue("?catID", CategoryID);
                command.Parameters.AddWithValue("?ID", ID);
                command.ExecuteNonQuery();

            }

            connection.Close();
        }

        public static void RemoveMovie(string name)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM movie WHERE id = ?name";
            command.Parameters.AddWithValue("?name", name);
            command.ExecuteNonQuery();
            connection.Close();
            
        }

        public void SetMovie(int id)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM movie WHERE id = ?movie_id;";
            command.Parameters.AddWithValue("?movie_id", id);
            MySqlDataReader rdr = command.ExecuteReader();
            while(rdr.Read())
            {
                ID = id;
                MovieName = Convert.ToString(rdr[1]);
                Price = float.Parse(rdr[2].ToString());
                Discount = Convert.ToInt32(rdr[3]);
                MovieLength = Convert.ToInt32(rdr[4]);
                AgeLimit = Convert.ToInt32(rdr[5]);
                isOnSale = Convert.ToBoolean(rdr[6]);
                LastShow = Convert.ToDateTime(rdr[7]);
                PosterPath = Convert.ToString(rdr[8]);
            }
            rdr.Close();
            command.Parameters.Clear();
            command.CommandText = "SELECT description FROM category INNER JOIN movie_category " +
               "ON category.id = movie_category.category_id INNER JOIN movie " +
               "ON movie.id = movie_category.movie_id " +
               "WHERE movie.id = ?movieID;";
            command.Parameters.AddWithValue("?movieID", ID);
            rdr = command.ExecuteReader();
            ListOfCategory = new List<string>();
            while (rdr.Read())
            {
                string temp = Convert.ToString(rdr[0]);
                ListOfCategory.Add(temp);
            }
            rdr.Close();
            connection.Close();
        }

        public void GetMovieById(int id)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM movie WHERE id = ?movieid;";
            command.Parameters.AddWithValue("?movieid", id);
            MySqlDataReader rdr = command.ExecuteReader();

            rdr.Read();
            
            ID = Convert.ToInt32(rdr[0]);
            MovieName = Convert.ToString(rdr[1]);
            Price = float.Parse(rdr[2].ToString());
            Discount = Convert.ToInt32(rdr[3]);
            MovieLength = Convert.ToInt32(rdr[4]);
            AgeLimit = Convert.ToInt32(rdr[5]);
            isOnSale = Convert.ToBoolean(rdr[6]);
            LastShow = Convert.ToDateTime(rdr[7]);
            PosterPath = Convert.ToString(rdr[8]);
          
            connection.Close();
            rdr.Close();
        }

        public static int GetIdByName(string name)
        {
            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM movie WHERE movie_name = ?moviename;";
            command.Parameters.AddWithValue("?moviename", name);
            MySqlDataReader rdr = command.ExecuteReader();

            rdr.Read();

            int i_d = Convert.ToInt32(rdr[0]);

            connection.Close();
            rdr.Close();

            return i_d;
        }





    }

}