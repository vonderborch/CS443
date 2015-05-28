using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Changelog : Form
    {
        public Changelog()
        {
            InitializeComponent();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Changelog_Load(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("changelog.txt");
            text_txt.Text = reader.ReadToEnd();
            reader.Close();
        }
    }
}
