using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using Project;

namespace Project
{
    public partial class Main : Form
    {
        Utilities util = new Utilities();
        static string basequery = "",
                      baselatitude = "33",
                      baselongitude = "-112",
                      baseuser = "--65q1FpAL_UQtVZ2PTGew",
                      basecategory = "Food",
                      basehour = "12";
        string lastquery = basequery,
               lastlatitude = baselatitude,
               lastlongitude = baselongitude,
               lastuser = baseuser,
               lastcategory = basecategory,
               lasthour = basehour;

        // result_table

        public Main()
        {
            InitializeComponent();
            util.server = "R-VAIO";
            util.database = "CPTS451PROJECT";
            reset();
            this.result_table.AutoGenerateColumns = true;
            this.version_txt.Text = "Version 1.0.1";
            this.status_txt.Text = "Welcome!";
        }

        private void Main_Load(object sender, EventArgs e) { }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void server_btn_Click(object sender, EventArgs e)
        {
            util.server = Interaction.InputBox("New Server?", "Set Server", util.server);
        }

        private void db_btn_Click(object sender, EventArgs e)
        {
            util.database = Interaction.InputBox("New database?", "Set Database", util.database);
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void q1_btn_Click(object sender, EventArgs e)
        {
            string cat = Interaction.InputBox("Category?", "Query 1", lastcategory),
                   latitude = Interaction.InputBox("Current Latitude?", "Query 1", lastlatitude),
                   longitude = Interaction.InputBox("Current Longitude?", "Query 1", lastlongitude);
            double lat = Convert.ToDouble(latitude),
                   lon = Convert.ToDouble(longitude);

            try
            {
                this.status_txt.Text = "Working...";
                List<Q1> results = util.Query1(cat, lat, lon);

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";

                lastcategory = cat;
                lastlatitude = latitude;
                lastlongitude = longitude;
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void q2i_btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.status_txt.Text = "Working...";
                List<Q2> results = util.Query2I();

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void q2ii_btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.status_txt.Text = "Working...";
                List<Q2> results = util.Query2II();

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void q3_btn_Click(object sender, EventArgs e)
        {
            string cat = Interaction.InputBox("Category?", "Query 3", lastcategory), hour = "";

            while (hour == "")
            {
                hour = Interaction.InputBox("Hour Start (0-23)?", "Query 3", lasthour);

                int temp = Convert.ToInt32(hour);
                if (temp >= 0 && temp < 24)
                    hour = temp.ToString();
                else
                    hour = "";
            }

            try
            {
                this.status_txt.Text = "Working...";
                List<Q3> results = util.Query3(cat, hour);

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";

                lastcategory = cat;
                lasthour = hour;
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void q4_btn_Click(object sender, EventArgs e)
        {
            string user = Interaction.InputBox("User ID?", "Query 4", lastuser);

            try
            {
                this.status_txt.Text = "Working...";
                List<Q4> results = util.Query4(user);

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";

                lastuser = user;
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void q5_btn_Click(object sender, EventArgs e)
        {
            string latitude = Interaction.InputBox("Current Latitude?", "Query 5", lastlatitude),
                   longitude = Interaction.InputBox("Current Longitude?", "Query 5", lastlongitude);
            double lat = Convert.ToDouble(latitude),
                   lon = Convert.ToDouble(longitude);

            try
            {
                this.status_txt.Text = "Working...";
                Q5AR result = util.Query5(lat, lon);
                List<Q5AR> results = new List<Q5AR>();
                results.Add(result);

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results.ToArray();

                this.status_txt.Text = "Results displayed!";

                lastlatitude = latitude;
                lastlongitude = longitude;
            }
            catch
            {
                this.status_txt.Text = "Query Failed!";
            }
        }

        private void reset()
        {
            result_table.DataSource = null;
        }

        private void custom_btn_Click(object sender, EventArgs e)
        {
            string query = Interaction.InputBox("Query?", "Custom Query", lastquery);

            if (query != "")
            {
                this.status_txt.Text = "Working...";
                DataTable results = util.CustomQuery(query);

                this.status_txt.Text = "Query done, adding to table...";
                this.result_table.DataSource = results;

                this.status_txt.Text = "Results displayed!";
            }
            lastquery = query;
        }

        private void about_btn_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void changelog_btn_Click(object sender, EventArgs e)
        {
            Changelog change = new Changelog();
            change.Show();
        }
    }
}
