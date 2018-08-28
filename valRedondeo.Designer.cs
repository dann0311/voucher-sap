namespace Vaucher
{
    partial class valRedondeo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtRedondeo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRedondeoOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRedondeo
            // 
            this.txtRedondeo.Location = new System.Drawing.Point(111, 15);
            this.txtRedondeo.Name = "txtRedondeo";
            this.txtRedondeo.Size = new System.Drawing.Size(124, 20);
            this.txtRedondeo.TabIndex = 0;
            this.txtRedondeo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRedondeo_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Monto Redondeo:";
            // 
            // btnRedondeoOK
            // 
            this.btnRedondeoOK.Location = new System.Drawing.Point(241, 12);
            this.btnRedondeoOK.Name = "btnRedondeoOK";
            this.btnRedondeoOK.Size = new System.Drawing.Size(75, 23);
            this.btnRedondeoOK.TabIndex = 2;
            this.btnRedondeoOK.Text = "Aceptar";
            this.btnRedondeoOK.UseVisualStyleBackColor = true;
            this.btnRedondeoOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // valRedondeo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 53);
            this.Controls.Add(this.btnRedondeoOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRedondeo);
            this.MaximumSize = new System.Drawing.Size(344, 91);
            this.MinimumSize = new System.Drawing.Size(344, 91);
            this.Name = "valRedondeo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ley de Redondeo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRedondeoOK;
        public System.Windows.Forms.TextBox txtRedondeo;
    }
}