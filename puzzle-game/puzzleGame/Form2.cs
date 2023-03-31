using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzleGame
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                okey.Enabled = false;
            }
            else
            {
                okey.Enabled = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            okey.Enabled = false;
        }

        private void okey_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Form1 frm1 = new Form1(textBox1.Text);
                this.Hide();
                frm1.Show();
            }
        }
    }
}
