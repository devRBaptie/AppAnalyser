using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _17599075_PROG7312_Task1
{
    public partial class StartPage : Form
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            assemblyAnalyser assembly = new assemblyAnalyser();
            assembly.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inheritance inheritance = new Inheritance();
            inheritance.Show(); 
        }
    }
}
