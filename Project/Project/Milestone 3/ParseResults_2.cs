using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Q2
    {
        public string b_id { get; set; }
        public string name { get; set; }
        public string cname { get; set; }
        public float rating { get; set; }
    }

    public class Utilities
    {
        /// <summary>
        /// For each business category, find the businesses that were rated best in June 2011.
        /// </summary>
        /// <returns></returns>
        public List<Q2> Query2I()
        {
            string querystring = "SELECT Business.b_id, Business.name, Category.name as 'cname', AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND DATEPART(mm,Reviews.date)=6 AND DATEPART(yyyy, Reviews.date)=2011 GROUP BY Business.b_id, Business.name, Category.name";

            List<Q2> results = new List<Q2>();
            List<Q2> tempresults = new List<Q2>();

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
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

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
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
    }
}
