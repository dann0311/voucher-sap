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
    public partial class valRedondeo : Form
    {
        
        public valRedondeo()
        {
            InitializeComponent();
        }

        public string vRed { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            vRed = txtRedondeo.Text;
            this.Close();
        }

        private void txtRedondeo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                btnRedondeoOK.PerformClick();
            }            
        }
    }
}
