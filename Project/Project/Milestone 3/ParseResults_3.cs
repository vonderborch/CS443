using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Q3
    {
        public string u_id { get; set; }
        public string name { get; set; }
        public float stars { get; set; }
        public string time { get; set; }
        public int count { get; set; }
    }
	
    public class Utilities
    {
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

            using (SqlConnection connection = new SqlConnection("CPTS451PROJECT"))
            {
                connection.Open();

                SqlCommand query = new SqlCommand(querystring, connection);
                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Q3 temp = new Q3
                    {
                        u_id = reader["u_id"].ToString(),
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
                            if (results[i].u_id == temp.u_id)
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

            return results;
        }
    }
}
