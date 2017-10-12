using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Allegiance.CommunitySecuritySystem.Client
{
    public partial class EndOfLife : Form
    {
        public EndOfLife()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://store.steampowered.com/app/700480/Microsoft_Allegiance/");
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
