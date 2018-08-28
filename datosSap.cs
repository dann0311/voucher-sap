using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vaucher
{
    public partial class datosSap : Form
    {
        public datosSap()
        {
            InitializeComponent();
        }

        public String sb1U { get; set; }
        public String sb1P { get; set; }

        public String okClose { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            sb1U = txtSB1U.Text;
            sb1P = txtSB1P.Text;

            if (sb1U == "" || sb1P == "")
            {
                MessageBox.Show("Favor ingrese datos", "Error");
                txtSB1P.Clear();
                txtSB1U.Clear();
            }else
                {
                    this.Close();
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            okClose = "1";
            Application.Exit();
        }
    }
}
