using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Q5
    {
        public string b_id { get; set; }
        public string category { get; set; }
        public float stars { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Q5R
    {
        public string category { get; set; }
        public float rawstars { get; set; }
        public float rawcount { get; set; }
    }

    public class Utilities
    {
        /// <summary>
        /// Given a location, find the most popular business category within 5 miles (# of businesses in that category) and the average rating of that business category.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="curLatitude"></param>
        /// <param name="curLongitude"></param>
        /// <returns></returns>
        public List<Q5R> Query5(double curLatitude, double curLongitude)
        {
            string querystring = "SELECT Business.b_id, Business.latitude, Business.longitude, Business.stars, Category.name as 'cname' FROM Business, Category WHERE Business.b_id=Category.b_id";

            List<Q5R> results = new List<Q5R>();
            List<Q5> tempresults = new List<Q5>();

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q5 temp = new Q5
                    {
                        b_id = reader["b_id"].ToString(),
                        category = reader["name"].ToString(),
                        stars = float.Parse(reader["stars"].ToString()),
                        latitude = double.Parse(reader["latitude"].ToString()),
                        longitude = double.Parse(reader["longitude"].ToString()),
                    };

                    if (Distance(curLatitude, curLongitude, temp.latitude, temp.longitude) < 10.0)
                        tempresults.Add(temp);
                }

                reader.Close();
            }

            foreach (Q5 o in tempresults)
            {
                bool exists = false;
                Q5R temp = new Q5R
                {
                    category = o.category,
                    rawcount = 1,
                    rawstars = o.stars
                };

                for (int i = 0; i<results.Count;i++)
                {
                    if (results[i].category==o.category)
                    {
                        Q5R temp2 = results[i];
                        temp2.rawstars += temp.rawstars;
                        temp2.rawcount++;

                        results[i] = temp2;
                    }
                }
                if (!exists)
                    results.Add(temp);
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
