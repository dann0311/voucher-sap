using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Vaucher
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            int folio = Convert.ToInt32(textBox1.Text);

            SqlConnection cn = new SqlConnection("Data Source=FS-SV-INFODES;Initial Catalog=Clipper;User ID=sa;Password=zxASqw1234");

            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_rVoucher", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlParameter fol = new SqlParameter("@folio", SqlDbType.Int);
            fol.Value = folio;

            cmd.Parameters.Add(fol);

            DataSet ds = new DataSet();            

            da.Fill(ds, "SP_rVoucher");

            cn.Close();

            if (textBox1.Text != "")
            {
                try
                {
                    CrystalReport1 cr = new CrystalReport1();
                    cr.Load("CrystalReport1.rpt");
                    cr.SetDataSource(ds);

                    crvReporte.ReportSource = cr;
                    
                }
                catch (Exception ex)
                    {
                        MessageBox.Show("Error: "+ex);
                    }
            }else
                {
                    MessageBox.Show("Favor Ingresar numero de comprobante");
                }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo puede ingresar numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}
