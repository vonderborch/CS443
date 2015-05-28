/*WSU EECS CptS 451*/
/*Instructor: Sakire Arslan Ay*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parse_yelp
{
    class Parser
    {
        //initialize the input/output data directory. Currently set to execution folder. 
        public static String dataDir = @"C:\Users\Ricky\SkyDrive\School\Spring 2014\CptS 451\Project\yelp_phoenix_academic_dataset\";
        static void Main(string[] args)
        {
            JSONParser my_parser =  new JSONParser();
            my_parser.parseJSONFile(dataDir+"yelp_academic_dataset_user.json",dataDir+"user.sql");
            my_parser.parseJSONFile(dataDir+"yelp_academic_dataset_review.json",dataDir+"review.sql");
            my_parser.parseJSONFile(dataDir+"yelp_academic_dataset_checkin.json",dataDir+"checkin.sql");
            my_parser.parseJSONFile(dataDir + "yelp_academic_dataset_business.json", dataDir + "business.sql");
        }
    }
}
