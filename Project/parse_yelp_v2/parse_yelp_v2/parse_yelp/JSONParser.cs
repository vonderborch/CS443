/*WSU EECS CptS 451*/
/*Instructor: Sakire Arslan Ay*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;


namespace parse_yelp
{
    class JSONParser
    {
        private int maxLength = 5000;
        public JSONParser( )
        {
        }
        
        public void parseJSONFile(string jsonInput, string sqlOutput)
        {
            int counter;
            string line;
            System.IO.StreamReader jsonfile;
            System.IO.StreamWriter sqlscriptfile;
        
            try
            {
                Console.Write("Progress:");
                // Read the json data jsonfile.
                jsonfile = new System.IO.StreamReader(jsonInput);
                // Create the sql script file. The script file is formatted for MySQL. If using Miscroft SQL Server should change the format - see Appendix B in Milestone 2 description
                sqlscriptfile = new System.IO.StreamWriter(sqlOutput);
                counter = 0;

                while ((line = jsonfile.ReadLine()) != null)
                {
                    JsonObject my_jsonStr = (JsonObject)JsonObject.Parse(line);
                    string type = my_jsonStr["type"].ToString();
                    switch (type)
                    {
                        case "\"review\"": sqlscriptfile.WriteLine(ProcessReviews(my_jsonStr));
                                           
                            break;
                        case "\"user\"": sqlscriptfile.WriteLine(ProcessUsers(my_jsonStr));
                            break;
                        case "\"checkin\"": sqlscriptfile.WriteLine(ProcessCheckins(my_jsonStr));
                            break;
                        case "\"business\"": sqlscriptfile.WriteLine(ProcessBusiness(my_jsonStr));
                            sqlscriptfile.Write(ProcessBusinessCategories(my_jsonStr));
                            break;
                        default: Console.WriteLine("Unknown type : " + type);
                            break;
                    }
                    if ((counter % 5000) == 0)
                        Console.Write("■");
                    counter++;
                }
                jsonfile.Close();
                sqlscriptfile.Close();
                
            }
            catch (Exception e)
            {
                Console.Write("Exception:");
                Console.WriteLine(e.Message);
            }
            // Suspend the screen.
            Console.WriteLine("\n"+sqlOutput+": created");
            Console.ReadLine();
        
        }

        /* The INSERT statement for review tuples*/
        public string ProcessReviews(JsonObject my_jsonStr)
        {
            return "INSERT INTO Reviews (r_id, u_id, b_id, stars, text, date, funnyVotes, usefulVotes, coolVotes) VALUES ("
                + "'" + my_jsonStr["review_id"].ToString().Replace("\"","") +"' , "
                + "'" + my_jsonStr["user_id"].ToString().Replace("\"", "") + "' , "
                + "'" + my_jsonStr["business_id"].ToString().Replace("\"", "") + "' , "
                + my_jsonStr["stars"].ToString().Replace("\"", "") + " , "
                + "'" + cleanTextforSQL(my_jsonStr["text"].ToString()) + "' , "
                + "'" + my_jsonStr["date"].ToString().Replace("\"", "") + "' , "
                + my_jsonStr["votes"]["funny"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["votes"]["useful"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["votes"]["cool"].ToString().Replace("\"", "") + ")\nGO";            
        }

 
        /* The INSERT statement for business tuples*/
        public string ProcessBusiness(JsonObject my_jsonStr)
        {
            int openInt = 0;
            string open = my_jsonStr["open"].ToString().Replace("\"", "");
            if (open == "true")
                openInt = 1;

            return "INSERT INTO Business (b_id, name, stars, r_count, is_open, address, city, us_state, latitude, longitude) VALUES ("
                + "'" + my_jsonStr["business_id"].ToString().Replace("\"", "") + "' , "
                + "'" + cleanTextforSQL(my_jsonStr["name"].ToString()) + "' , "
                + my_jsonStr["stars"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["review_count"].ToString().Replace("\"", "") + " , "
                + openInt.ToString() + " , "
                + "'" + cleanTextforSQL(my_jsonStr["full_address"].ToString()) + "' , "
                + "'" + cleanTextforSQL(my_jsonStr["city"].ToString()) + "' , "
                + "'" + my_jsonStr["state"].ToString().Replace("\"", "") + "' , "
                + my_jsonStr["latitude"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["longitude"].ToString().Replace("\"", "") 
                + ")\nGO";
        }

        /* The INSERT statement for business tuples*/
        public string ProcessBusinessCategories(JsonObject my_jsonStr)
        {
            String insertString = "";
            JsonArray categories = (JsonArray) my_jsonStr["categories"];
            //append an INSERT statement to insertString for each category of the business 
            for (int i=0; i<categories.Count; i++)
                insertString = insertString + "INSERT INTO Category (b_id, name) VALUES ("
                                + "'" + my_jsonStr["business_id"].ToString().Replace("\"", "") + "' , "
                                + "'" + cleanTextforSQL(categories[i].ToString()) + "'"
                                + ")\nGO"
                                +"\n"; //append a new line
            return insertString;
        }
        
        
        /* The INSERT statement for user tuples*/
        public string ProcessUsers(JsonObject my_jsonStr)
        {
            return "INSERT INTO Users (u_id, name, stars, r_count, funnyVotes, usefulVotes, coolVotes) VALUES ("
                + "'" + my_jsonStr["user_id"].ToString().Replace("\"", "") + "' , "
                + "'" + my_jsonStr["name"].ToString().Replace("\"", "") + "' , "
                + "'" + my_jsonStr["average_stars"].ToString().Replace("\"", "") + "' , "
                + "'" + my_jsonStr["review_count"].ToString().Replace("\"", "") + "' , "
                + my_jsonStr["votes"]["funny"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["votes"]["useful"].ToString().Replace("\"", "") + " , "
                + my_jsonStr["votes"]["cool"].ToString().Replace("\"", "") + ")\nGO";   

        }

        /* The INSERT statement for checkin tuples*/
        public string ProcessCheckins(JsonObject my_jsonStr)
        {
            String insertString = "";
            var info = my_jsonStr["checkin_info"];
            //append an INSERT statement to insertString for each category of the business 
            foreach (KeyValuePair<string, System.Json.JsonValue> tuple in info)
            {
                insertString = insertString + "INSERT INTO CheckIn (b_id, time, count) VALUES ("
                                + "'" + my_jsonStr["business_id"].ToString().Replace("\"", "") + "' , "
                                + "'" + cleanTextforSQL(tuple.Key) + "' , "
                                + "'" + cleanTextforSQL(tuple.Value.ToString()) + "'"
                                + ")\nGO"
                                + "\n"; //append a new line*/
            }
            return insertString;
        }

        private string cleanTextforSQL(string inStr)
        {
            String outStr = inStr.Replace("\"", "").Replace("'", "''").Replace(@"\n"," ").Replace(@"\u000a"," ").Replace("\\"," ");
            //Only get he first maxLength chars. Set maxLength to the max length of your attribute.
            return outStr.Substring(0, Math.Min(outStr.Length, maxLength));
        }
    }

    
}
