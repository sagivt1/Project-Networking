using MySql.Data.MySqlClient;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class MovieShowDal
    {
        public Movie movie { get; set; }
        public Show showtime { get; set; }
        public Hall hall { get; set; }
        public List<Ticket> tickets { get; set; }
        public Boolean[,] arr { get; set; }
        public float Price { get; set; }


        public void GetShow(int ShowId)
        {

            MySqlConnection connection = new MySqlConnection(Global.MyServer);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM showtime " +
                "INNER JOIN movie ON movie.id = showtime.movie_id " +
                "INNER JOIN hall ON hall.id = showtime.hall_id WHERE showtime.id = ?id; ";
            command.Parameters.AddWithValue("?id",ShowId);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            showtime = new Show
            {
                ID = Convert.ToInt32(rdr[0]),
                hall = Convert.ToInt32(rdr[1]),
                movie = Convert.ToInt32(rdr[2]),
                dateTime = Convert.ToDateTime(rdr[3])
            };
            movie = new Movie
            {
                ID = Convert.ToInt32(rdr[4]),
                MovieName = Convert.ToString(rdr[5]),
                Price = float.Parse(rdr[6].ToString()),
                Discount = Convert.ToInt32(rdr[7]),
                MovieLength = Convert.ToInt32(rdr[8]),
                AgeLimit = Convert.ToInt32(rdr[9]),
                isOnSale = Convert.ToBoolean(rdr[10]),
                LastShow = Convert.ToDateTime(rdr[11]),
                PosterPath = Convert.ToString(rdr[12])
            };
            hall = new Hall
            {
                ID = Convert.ToInt32(rdr[13]),
                TotalRow = Convert.ToInt32(rdr[14]),
                TotalCol = Convert.ToInt32(rdr[15])
            };
            rdr.Close();
            Price = (float)(movie.Price * (1 - movie.Discount/100.0));
            command.CommandText = "SELECT * FROM ticket WHERE showtime_id = ?id";
            rdr = command.ExecuteReader();
            tickets = new List<Ticket>();
            while(rdr.Read())
            {
                Ticket ticket = new Ticket
                {
                    ShowId = showtime.ID,
                    Price = this.Price,
                    SeatRow = Convert.ToInt32(rdr[1]),
                    SeatCol = Convert.ToInt32(rdr[2])
                };
                tickets.Add(ticket);
            }

            rdr.Close();
            connection.Close();

            arr = new Boolean[hall.TotalRow, hall.TotalCol];
            foreach (var ticket in tickets)
            {
                arr[ticket.SeatRow, ticket.SeatCol] = true;
            }
        }
    }

   
}