using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TODOLISTSMART;

namespace TODOLISTSMART_V1
{
    public partial class HlamanLogin : Form
    {
        public HlamanLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (iduser.Text == "Cutie" && password.Text == "103034")
            {
                new HalamanUtama().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username atau password yang anda masukan tidak sesuai");
                iduser.Clear();
                password.Clear();
                iduser.Focus();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
