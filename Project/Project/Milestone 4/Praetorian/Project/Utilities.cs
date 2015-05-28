using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Project
{
    public class Q1
    {
        [System.ComponentModel.DisplayName("Business ID")]
        public string b_id { get; set; }

        [System.ComponentModel.DisplayName("Business Name")]
        public string name { get; set; }

        [System.ComponentModel.DisplayName("Business Latitude")]
        public double latitude { get; set; }

        [System.ComponentModel.DisplayName("Business Longitude")]
        public double longitude { get; set; }
    }

    public class Q2
    {
        [System.ComponentModel.DisplayName("Business ID")]
        public string b_id { get; set; }

        [System.ComponentModel.DisplayName("Business Name")]
        public string name { get; set; }

        [System.ComponentModel.DisplayName("Category")]
        public string cname { get; set; }

        [System.ComponentModel.DisplayName("Rating")]
        public float rating { get; set; }
    }

    public class Q3
    {
        [System.ComponentModel.DisplayName("Business ID")]
        public string b_id { get; set; }

        [System.ComponentModel.DisplayName("Business Name")]
        public string name { get; set; }

        [System.ComponentModel.DisplayName("Average Rating")]
        public float stars { get; set; }

        [System.ComponentModel.DisplayName("Time")]
        public string time { get; set; }

        [System.ComponentModel.DisplayName("Count")]
        public int count { get; set; }
    }

    public class Q4
    {
        [System.ComponentModel.DisplayName("User ID")]
        public string u_id { get; set; }

        [System.ComponentModel.DisplayName("User Name")]
        public string name { get; set; }

        [System.ComponentModel.DisplayName("Favorite Category")]
        public string category { get; set; }

        [System.ComponentModel.DisplayName("Average Rating")]
        public float stars { get; set; }
    }

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

    public class Q5AR
    {
        [System.ComponentModel.DisplayName("Category")]
        public string category { get; set; }

        [System.ComponentModel.DisplayName("Average Rating")]
        public float avgstars { get; set; }

        [System.ComponentModel.DisplayName("Review Count")]
        public float count { get; set; }
    }

    public class Business
    {
        public string b_id { get; set; }
        public string name { get; set; }
        public float stars { get; set; }
        public int r_count { get; set; }
        public bool is_open { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string us_state { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Utilities
    {
        public string server { get; set; }
        public string database { get; set; }

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

            // CPTS451PROJECT
            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
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

        /// <summary>
        /// For each business category, find the businesses that were rated best in June 2011.
        /// </summary>
        /// <returns></returns>
        public List<Q2> Query2I()
        {
            string querystring = "SELECT Business.b_id, Business.name, Category.name as 'cname', AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND DATEPART(mm,Reviews.date)=6 AND DATEPART(yyyy, Reviews.date)=2011 GROUP BY Business.b_id, Business.name, Category.name";

            List<Q2> results = new List<Q2>();
            List<Q2> tempresults = new List<Q2>();

            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q2 temp = new Q2
                    {
                        b_id = reader["b_id"].ToString(),
                        name = reader["name"].ToString(),
                        cname = reader["cname"].ToString(),
                        rating = float.Parse(reader["rating"].ToString()),
                    };

                    tempresults.Add(temp);
                }

                reader.Close();
            }

            foreach (Q2 o in tempresults)
            {
                bool exists = false;
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].cname == o.cname)
                    {
                        if (results[i].rating < o.rating)
                            results[i] = o;
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    results.Add(o);
                }
            }

            return results;
        }

        /// <summary>
        /// Find the restaurants that steadily improved their ratings during the year of 2012.
        /// </summary>
        /// <returns></returns>
        public List<Q2> Query2II()
        {
            string querystring = "SELECT Business.b_id, Business.name, AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND Category.name='Food' AND DATEPART(mm,Reviews.date)=1 AND DATEPART(yyyy, Reviews.date)=2012 GROUP BY Business.b_id, Business.name";
            string querystring2 = "SELECT Business.b_id, Business.name, AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND Category.name='Food' AND DATEPART(mm,Reviews.date)=12 AND DATEPART(yyyy, Reviews.date)=2011 GROUP BY Business.b_id, Business.name";

            List<Q2> results = new List<Q2>();
            List<Q2> q1results = new List<Q2>();
            List<Q2> q2results = new List<Q2>();

            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q2 temp = new Q2
                    {
                        b_id = reader["b_id"].ToString(),
                        name = reader["name"].ToString(),
                        cname = "Food",
                        rating = float.Parse(reader["rating"].ToString()),
                    };
                    q1results.Add(temp);
                }

                reader.Close();

                query = new SqlCommand(querystring2, connection);
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q2 temp = new Q2
                    {
                        b_id = reader["b_id"].ToString(),
                        name = reader["name"].ToString(),
                        cname = "Food",
                        rating = float.Parse(reader["rating"].ToString()),
                    };
                    q2results.Add(temp);
                }

                reader.Close();
            }

            foreach (Q2 r1 in q1results)
            {
                foreach (Q2 r2 in q2results)
                {
                    if (r1.rating < r2.rating)
                        results.Add(r2);
                }
            }

            return results;
        }

        /// <summary>
        /// Given a business category and a time period (of the day), find the most popular businesses (during that time period) and their rating.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<Q3> Query3(string category, string hourStart)
        {
            string querystring = "SELECT Business.b_id, Business.name, Business.stars, CheckIn.time, CheckIn.count FROM Business, Category, CheckIn WHERE Business.b_id=Category.b_id AND Business.b_id=CheckIn.b_id AND Category.name='" + category + "' ORDER BY CheckIn.count DESC";

            List<Q3> results = new List<Q3>();

            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q3 temp = new Q3
                    {
                        b_id = reader["b_id"].ToString(),
                        name = reader["name"].ToString(),
                        stars = float.Parse(reader["stars"].ToString()),
                        time = reader["time"].ToString(),
                        count = int.Parse(reader["count"].ToString()),
                    };

                    if (hourStart == temp.time.Split('-')[0])
                    {
                        bool exists = false;
                        for (int i = 0; i < results.Count; i++)
                        {
                            if (results[i].b_id == temp.b_id)
                            {
                                Q3 newtemp = temp;
                                newtemp.count += results[i].count;
                                results[i] = newtemp;
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                            results.Add(temp);
                    }
                }

                reader.Close();
            }

            results.Sort((x, y) => x.count.CompareTo(y.count));
            results.Reverse();

            return results;
        }

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

            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
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

        /// <summary>
        /// Given a location, find the most popular business category within 5 miles (# of businesses in that category) and the average rating of that business category.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="curLatitude"></param>
        /// <param name="curLongitude"></param>
        /// <returns></returns>
        public Q5AR Query5(double curLatitude, double curLongitude)
        {
            string querystring = "SELECT Business.b_id, Business.latitude, Business.longitude, Business.stars, Category.name as 'cname' FROM Business, Category WHERE Business.b_id=Category.b_id";

            List<Q5R> results = new List<Q5R>();
            List<Q5> tempresults = new List<Q5>();

            using (SqlConnection connection = new SqlConnection("server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q5 temp = new Q5
                    {
                        b_id = reader["b_id"].ToString(),
                        category = reader["cname"].ToString(),
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

            Q5AR actualresult = null;
            foreach (Q5R o in results)
            {
                Q5AR temp = new Q5AR();
                temp.avgstars = o.rawstars / o.rawcount;
                temp.category = o.category;
                temp.count = o.rawcount;

                if (actualresult == null || actualresult.count < temp.count)
                    actualresult = temp;
            }

            return actualresult;
        }

        public DataTable CustomQuery(string querystring)
        {
            DataTable results = new DataTable();

            try
            {
                string connstring = "server=" + server + ";database=" + database + ";pooling=false;Connect Timeout=60;Integrated Security=SSPI;";

                SqlDataAdapter dAdapter = new SqlDataAdapter(querystring, connstring);
                SqlCommandBuilder cBuiler = new SqlCommandBuilder(dAdapter);

                dAdapter.Fill(results);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Invalid query!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
