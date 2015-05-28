using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Q1
    {
        public string b_id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Utilities
    {
        /// <summary>
        /// Given a “business category” and the current location, find the businesses within 10 miles.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="curLatitude"></param>
        /// <param name="curLongitude"></param>
        /// <returns></returns>
        public List<Q1> Query1(string category, double curLatitude, double curLongitude)
        {
            string querystring = "SELECT Business.b_id, Business.name, Business.latitude, Business.longitude FROM Business, Category WHERE Business.b_id=Category.b_id AND Category.name='" + category + "'";

            List<Q1> results = new List<Q1>();

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q1 temp = new Q1
                    {
                        b_id = reader["b_id"].ToString(),
                        name = reader["name"].ToString(),
                        latitude = double.Parse(reader["latitude"].ToString()),
                        longitude = double.Parse(reader["longitude"].ToString()),
                    };

                    if (Distance(curLatitude, curLongitude, temp.latitude, temp.longitude) < 10.0)
                        results.Add(temp);
                }

                reader.Close();
            }

            return results;
        }

        public static double Distance(double latitude1, double longitude1,
                                      double latitude2, double longitude2)
        {
            const double earth = 3956.087107103049 * 2;

            double latitude1Rad = latitude1 / 180 * Math.PI,
                   longitude1Rad = longitude1 / 180 * Math.PI,
                   latitude2Rad = latitude2 / 180 * Math.PI,
                   longitude2Rad = longitude2 / 180 * Math.PI;

            return earth * Math.Asin(Math.Sqrt(Math.Pow(
                           Math.Sin((latitude1Rad - latitude2Rad) / 2), 2) +
                           Math.Cos(latitude1Rad) * Math.Cos(latitude2Rad) *
                           Math.Pow(Math.Sin((longitude1Rad - longitude2Rad) / 2), 2)));
        }
    }
}
