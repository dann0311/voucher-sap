namespace Vaucher
{
    partial class Vaucher
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Vaucher));
            this.lblFechaSist1 = new System.Windows.Forms.Label();
            this.lblHoraSist1 = new System.Windows.Forms.Label();
            this.lblFechaSist2 = new System.Windows.Forms.Label();
            this.lblHoraSist2 = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.lblLocal = new System.Windows.Forms.Label();
            this.lblfecha = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.dgvDatosVaucher = new System.Windows.Forms.DataGridView();
            this.txtTotalDebe = new System.Windows.Forms.TextBox();
            this.txtTotalHaber = new System.Windows.Forms.TextBox();
            this.lblDebe = new System.Windows.Forms.Label();
            this.lblHaber = new System.Windows.Forms.Label();
            this.lblDiferencia = new System.Windows.Forms.Label();
            this.txtDiferenciaDH = new System.Windows.Forms.TextBox();
            this.txtDia = new System.Windows.Forms.TextBox();
            this.txtMes = new System.Windows.Forms.TextBox();
            this.txtAño = new System.Windows.Forms.TextBox();
            this.lblComprobanteContable = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sALIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeVoucherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.btnSAP = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVaucher)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFechaSist1
            // 
            this.lblFechaSist1.AutoSize = true;
            this.lblFechaSist1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaSist1.Location = new System.Drawing.Point(1030, 63);
            this.lblFechaSist1.Name = "lblFechaSist1";
            this.lblFechaSist1.Size = new System.Drawing.Size(97, 15);
            this.lblFechaSist1.TabIndex = 1;
            this.lblFechaSist1.Text = "Fecha Contable: ";
            // 
            // lblHoraSist1
            // 
            this.lblHoraSist1.AutoSize = true;
            this.lblHoraSist1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoraSist1.Location = new System.Drawing.Point(1086, 86);
            this.lblHoraSist1.Name = "lblHoraSist1";
            this.lblHoraSist1.Size = new System.Drawing.Size(41, 15);
            this.lblHoraSist1.TabIndex = 2;
            this.lblHoraSist1.Text = "Hora: ";
            // 
            // lblFechaSist2
            // 
            this.lblFechaSist2.AutoSize = true;
            this.lblFechaSist2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaSist2.Location = new System.Drawing.Point(1131, 63);
            this.lblFechaSist2.Name = "lblFechaSist2";
            this.lblFechaSist2.Size = new System.Drawing.Size(11, 15);
            this.lblFechaSist2.TabIndex = 3;
            this.lblFechaSist2.Text = "-";
            // 
            // lblHoraSist2
            // 
            this.lblHoraSist2.AutoSize = true;
            this.lblHoraSist2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoraSist2.Location = new System.Drawing.Point(1131, 86);
            this.lblHoraSist2.Name = "lblHoraSist2";
            this.lblHoraSist2.Size = new System.Drawing.Size(11, 15);
            this.lblHoraSist2.TabIndex = 4;
            this.lblHoraSist2.Text = "-";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(690, 139);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 5;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtLocal
            // 
            this.txtLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocal.Location = new System.Drawing.Point(564, 140);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(100, 22);
            this.txtLocal.TabIndex = 7;
            this.txtLocal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLocal.TextChanged += new System.EventHandler(this.txtLocal_TextChanged);
            this.txtLocal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocal_KeyPress);
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocal.Location = new System.Drawing.Point(572, 124);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(75, 15);
            this.lblLocal.TabIndex = 8;
            this.lblLocal.Text = "Numero Local";
            // 
            // lblfecha
            // 
            this.lblfecha.AutoSize = true;
            this.lblfecha.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfecha.Location = new System.Drawing.Point(452, 124);
            this.lblfecha.Name = "lblfecha";
            this.lblfecha.Size = new System.Drawing.Size(90, 15);
            this.lblfecha.TabIndex = 9;
            this.lblfecha.Text = "Fecha de Proceso";
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(1142, 470);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 35);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(1052, 470);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 35);
            this.btnLimpiar.TabIndex = 23;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(690, 470);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(130, 35);
            this.btnGuardar.TabIndex = 24;
            this.btnGuardar.Text = "Crear archivo XX";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // dgvDatosVaucher
            // 
            this.dgvDatosVaucher.AllowUserToAddRows = false;
            this.dgvDatosVaucher.AllowUserToDeleteRows = false;
            this.dgvDatosVaucher.AllowUserToResizeColumns = false;
            this.dgvDatosVaucher.AllowUserToResizeRows = false;
            this.dgvDatosVaucher.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDatosVaucher.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDatosVaucher.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDatosVaucher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDatosVaucher.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDatosVaucher.Location = new System.Drawing.Point(15, 190);
            this.dgvDatosVaucher.Name = "dgvDatosVaucher";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDatosVaucher.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDatosVaucher.RowHeadersVisible = false;
            this.dgvDatosVaucher.Size = new System.Drawing.Size(1202, 266);
            this.dgvDatosVaucher.TabIndex = 25;
            this.dgvDatosVaucher.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatosVaucher_CellValueChanged);
            // 
            // txtTotalDebe
            // 
            this.txtTotalDebe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalDebe.Location = new System.Drawing.Point(78, 476);
            this.txtTotalDebe.Name = "txtTotalDebe";
            this.txtTotalDebe.ReadOnly = true;
            this.txtTotalDebe.Size = new System.Drawing.Size(138, 22);
            this.txtTotalDebe.TabIndex = 27;
            // 
            // txtTotalHaber
            // 
            this.txtTotalHaber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalHaber.Location = new System.Drawing.Point(299, 476);
            this.txtTotalHaber.Name = "txtTotalHaber";
            this.txtTotalHaber.ReadOnly = true;
            this.txtTotalHaber.Size = new System.Drawing.Size(157, 22);
            this.txtTotalHaber.TabIndex = 28;
            // 
            // lblDebe
            // 
            this.lblDebe.AutoSize = true;
            this.lblDebe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebe.Location = new System.Drawing.Point(12, 481);
            this.lblDebe.Name = "lblDebe";
            this.lblDebe.Size = new System.Drawing.Size(60, 13);
            this.lblDebe.TabIndex = 29;
            this.lblDebe.Text = "Total Debe";
            // 
            // lblHaber
            // 
            this.lblHaber.AutoSize = true;
            this.lblHaber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHaber.Location = new System.Drawing.Point(230, 481);
            this.lblHaber.Name = "lblHaber";
            this.lblHaber.Size = new System.Drawing.Size(63, 13);
            this.lblHaber.TabIndex = 30;
            this.lblHaber.Text = "Total Haber";
            // 
            // lblDiferencia
            // 
            this.lblDiferencia.AutoSize = true;
            this.lblDiferencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiferencia.Location = new System.Drawing.Point(472, 481);
            this.lblDiferencia.Name = "lblDiferencia";
            this.lblDiferencia.Size = new System.Drawing.Size(55, 13);
            this.lblDiferencia.TabIndex = 31;
            this.lblDiferencia.Text = "Diferencia";
            // 
            // txtDiferenciaDH
            // 
            this.txtDiferenciaDH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiferenciaDH.Location = new System.Drawing.Point(533, 476);
            this.txtDiferenciaDH.Name = "txtDiferenciaDH";
            this.txtDiferenciaDH.ReadOnly = true;
            this.txtDiferenciaDH.Size = new System.Drawing.Size(146, 22);
            this.txtDiferenciaDH.TabIndex = 32;
            // 
            // txtDia
            // 
            this.txtDia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDia.Location = new System.Drawing.Point(440, 140);
            this.txtDia.Name = "txtDia";
            this.txtDia.Size = new System.Drawing.Size(35, 22);
            this.txtDia.TabIndex = 33;
            this.txtDia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDia.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            this.txtDia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDia_KeyPress);
            // 
            // txtMes
            // 
            this.txtMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMes.Location = new System.Drawing.Point(477, 140);
            this.txtMes.Name = "txtMes";
            this.txtMes.Size = new System.Drawing.Size(36, 22);
            this.txtMes.TabIndex = 34;
            this.txtMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMes.TextChanged += new System.EventHandler(this.txt2_TextChanged);
            this.txtMes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMes_KeyPress);
            // 
            // txtAño
            // 
            this.txtAño.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAño.Location = new System.Drawing.Point(514, 140);
            this.txtAño.Name = "txtAño";
            this.txtAño.Size = new System.Drawing.Size(45, 22);
            this.txtAño.TabIndex = 35;
            this.txtAño.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAño.TextChanged += new System.EventHandler(this.txt3_TextChanged);
            this.txtAño.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAño_KeyPress);
            // 
            // lblComprobanteContable
            // 
            this.lblComprobanteContable.AutoSize = true;
            this.lblComprobanteContable.Font = new System.Drawing.Font("Times New Roman", 10.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprobanteContable.Location = new System.Drawing.Point(1036, 30);
            this.lblComprobanteContable.Name = "lblComprobanteContable";
            this.lblComprobanteContable.Size = new System.Drawing.Size(160, 17);
            this.lblComprobanteContable.TabIndex = 41;
            this.lblComprobanteContable.Text = "Comprobante Contable";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1229, 24);
            this.menuStrip1.TabIndex = 44;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sALIRToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // sALIRToolStripMenuItem
            // 
            this.sALIRToolStripMenuItem.Name = "sALIRToolStripMenuItem";
            this.sALIRToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.sALIRToolStripMenuItem.Text = "Salir";
            this.sALIRToolStripMenuItem.Click += new System.EventHandler(this.sALIRToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeVoucherToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeVoucherToolStripMenuItem
            // 
            this.acercaDeVoucherToolStripMenuItem.Name = "acercaDeVoucherToolStripMenuItem";
            this.acercaDeVoucherToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.acercaDeVoucherToolStripMenuItem.Text = "Acerca de Voucher";
            this.acercaDeVoucherToolStripMenuItem.Click += new System.EventHandler(this.acercaDeVoucherToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Vaucher.Properties.Resources.logoFS;
            this.pictureBox1.Location = new System.Drawing.Point(440, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 62);
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(1089, 139);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(128, 23);
            this.btnImprimir.TabIndex = 45;
            this.btnImprimir.Text = "Imprimir Comprobante";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Visible = false;
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(32, 88);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(0, 13);
            this.lbl.TabIndex = 46;
            this.lbl.Visible = false;
            // 
            // btnSAP
            // 
            this.btnSAP.Location = new System.Drawing.Point(786, 140);
            this.btnSAP.Name = "btnSAP";
            this.btnSAP.Size = new System.Drawing.Size(132, 23);
            this.btnSAP.TabIndex = 47;
            this.btnSAP.Text = "Ingresar a SAP B1";
            this.btnSAP.UseVisualStyleBackColor = true;
            this.btnSAP.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Vaucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 512);
            this.Controls.Add(this.btnSAP);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.lblComprobanteContable);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtAño);
            this.Controls.Add(this.txtMes);
            this.Controls.Add(this.txtDia);
            this.Controls.Add(this.txtDiferenciaDH);
            this.Controls.Add(this.lblDiferencia);
            this.Controls.Add(this.lblHaber);
            this.Controls.Add(this.lblDebe);
            this.Controls.Add(this.txtTotalHaber);
            this.Controls.Add(this.txtTotalDebe);
            this.Controls.Add(this.dgvDatosVaucher);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblfecha);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.txtLocal);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lblHoraSist2);
            this.Controls.Add(this.lblFechaSist2);
            this.Controls.Add(this.lblHoraSist1);
            this.Controls.Add(this.lblFechaSist1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1245, 550);
            this.MinimumSize = new System.Drawing.Size(1245, 550);
            this.Name = "Vaucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Voucher";
            this.Load += new System.EventHandler(this.Vaucher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVaucher)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFechaSist1;
        private System.Windows.Forms.Label lblHoraSist1;
        private System.Windows.Forms.Label lblFechaSist2;
        private System.Windows.Forms.Label lblHoraSist2;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.Label lblfecha;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox txtTotalDebe;
        private System.Windows.Forms.TextBox txtTotalHaber;
        private System.Windows.Forms.Label lblDebe;
        private System.Windows.Forms.Label lblHaber;
        private System.Windows.Forms.Label lblDiferencia;
        private System.Windows.Forms.TextBox txtDiferenciaDH;
        public System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblComprobanteContable;
        public System.Windows.Forms.DataGridView dgvDatosVaucher;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sALIRToolStripMenuItem;
        public System.Windows.Forms.TextBox txtDia;
        public System.Windows.Forms.TextBox txtMes;
        public System.Windows.Forms.TextBox txtAño;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeVoucherToolStripMenuItem;
        private System.Windows.Forms.Button btnImprimir;
        public System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Button btnSAP;
        public System.Windows.Forms.Button btnSalir;
    }
}

