using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Q4
    {
        public string u_id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public float stars { get; set; }
    }
	
    public class Utilities
    {
        /// <summary>
        /// Given a user, find their favorite business category to review for and their average review score.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="curLatitude"></param>
        /// <param name="curLongitude"></param>
        /// <returns></returns>
        public List<Q4> Query4(string user)
        {
            string querystring = "SELECT Users.u_id as 'u_id', Users.name as 'name', Users.stars as 'stars', Category.name as 'cname', Count(Distinct Category.name) as 'count' FROM Users, Reviews, Category WHERE Users.u_id=Reviews.u_id AND Reviews.b_id=Category.b_id AND Users.u_id='" + user + "' GROUP BY Users.u_id, Users.name, Users.stars, Category.name";

            List<Q4> results = new List<Q4>();

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q4 temp = new Q4
                    {
                        u_id = reader["u_id"].ToString(),
                        name = reader["name"].ToString(),
                        stars = float.Parse(reader["stars"].ToString()),
                        category = reader["cname"].ToString()
                    };
                    results.Add(temp);
                }

                reader.Close();
            }

            return results;
        }
    }
}
