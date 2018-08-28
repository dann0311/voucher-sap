using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.Odbc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using Sap;
using Sap.Data;
using Sap.Data.Hana;
using SAPbobsCOM;




namespace Vaucher
{
    public partial class Vaucher : Form
    {
        String AQ = "AQ";
        String CP = "CP";
        String EC = "EC";
        String GF = "GF";
        String folio;           //folios arqueos
        string vaRED;           //variable redondeo
        double difRed;          //diferencia redondeo
        Boolean cOK;
        //Boolean todas =  true;
        Boolean okL = false;
        bool l16 = false;
        bool dl16 = true;
        bool drd = false;
        int gesC, gesD;         //ges credito, debito
        List<Consultor> lista = new List<Consultor>();
        //............. sql...............................
        //SqlConnection cn = new SqlConnection("Data Source=FS-SV-INFODES;Initial Catalog=Clipper;User ID=sa;Password=zxASqw1234");       //conexion a base de datos en servidor fs-sv-infodes
        //SqlCommand cmd;
        //SqlDataReader read;   
        //..........Variables SAP...............................................
        Company SBO = new Company();
        HanaConnection con = new HanaConnection("DRIVER={HDBODBC};UID=SYSTEM;PWD=Passw0rd;SERVERNODE=192.168.0.230:30015");             //conexion a base de datos en servidor 192.168.0.230:30015 (SAP)
        public int errSAPNum = 0;
        public String errSAPDesc = String.Empty;
        String cuenta = String.Empty;
        String ncDocumento, ncRUT;
        //int montoD, montoH;
        String a1, a2, b1, b2;
        //datosSap dsp = new datosSap();
        //public String usSAP, passSAP;
        //......................................................................

        public Boolean conExxis()
        {
            bool cex = true;
             
            SBO.Server = "192.168.0.230:30015";
            SBO.CompanyDB = empresa(Convert.ToInt32(txtLocal.Text)).Replace("\"",""); //empresa            
            SBO.UserName = "manager";                           //usuario sap 
            SBO.Password = "mngr";                              //pass sap
            SBO.DbUserName = "SYSTEM";                          //usuario DDBB
            SBO.DbPassword = "Passw0rd";                        //pass DDBB
            SBO.DbServerType = BoDataServerTypes.dst_HANADB;    //motor DDBB

            errSAPNum = SBO.Connect();

            try
            {
                if (!errSAPNum.Equals(0))
                {
                    MessageBox.Show("No se realizo conexion a Base de datos HANA, Error: " + errSAPNum);
                    cex = false;
                }
            }
            catch (Exception ex)
                { MessageBox.Show("error, " + ex); }

            return cex;
        }                                                    //Conexion a SAP Hana

        public void descExxis()
        {
            SBO.Disconnect();
        }                                                      //Desconexion a SAP Hana

        public Vaucher()
        { 
            InitializeComponent();
            //dsp.ShowDialog();
            //usSAP = dsp.sb1U;
            //passSAP = dsp.sb1P;
            lblFechaSist2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHoraSist2.Text = DateTime.Now.ToLongTimeString();
        }                                                             //fecha y hora sistema

        private void datosLocales(String fechaProc, int nLocal, String fechaNomb)       //Metodo validar directorio, archivos, crear temp y copiar archivos 
        {
            String dia, mes, año, fechaProc1, fechaProc2, fechaAr, fNom;

            //obtiene los valores dia - mes - año
            dia = txtDia.Text;
            mes = txtMes.Text;
            año = txtAño.Text;

            //crear formatos de fechas
            fechaProc1 = año + mes + dia;                                               //yyyyMMdd
            fechaProc2 = dia + "-" + mes + "-" + año;                                   //dd-MM-yyyy
            fechaAr = dia + "/" + mes + "/" + año;                                      //dd/MM/yyyy
            fNom = año.Remove(0, 2) + mes + dia;                                        //yyMMdd

            int nLocal1 = Convert.ToInt32(txtLocal.Text);

            String FSCierres = @"D:\FS\CIERRES";
            String Cierres = @"D:\CIERRES";                                                                                             //ruta carpeta cierres                                                                    
            String carpetaLocales = FSCierres + "\\" + fechaProc + "\\" + txtLocal.Text;                                                //ruta carpeta archivos locales

            DirectoryInfo archivosLocales = new DirectoryInfo(FSCierres + "\\" + fechaProc + "\\" + nLocal + "\\");                     //compueba archivos

            String archivosTemp = Cierres + "\\" + fechaProc + "\\" + txtLocal.Text + "\\";                                             //crear ruta carpeta temporal

            String nombreArchivos = fechaNomb + "." + txtLocal.Text;                                                                    //Crea nombre a los archivos añoMesDiaLocal

            // Nombre archivos con numero local
            String arcCierreAQ = AQ + nombreArchivos;
            String arcCierreCP = CP + nombreArchivos;
            String arcCierreEC = EC + nombreArchivos;
            String arcCierreGF = GF + nombreArchivos;

            // Nombre archivos sin numero local
            String arcCierreAQF = AQ + fechaNomb;
            String arcCierreCPF = CP + fechaNomb;
            String arcCierreECF = EC + fechaNomb;
            String arcCierreGFF = GF + fechaNomb;

            // Nombre archivos sin numero local
            String origAQ = carpetaLocales + "\\" + AQ + fechaNomb + "." + nLocal;
            String origCP = carpetaLocales + "\\" + CP + fechaNomb + "." + nLocal;
            String origEC = carpetaLocales + "\\" + EC + fechaNomb + "." + nLocal;
            String origGF = carpetaLocales + "\\" + GF + fechaNomb + "." + nLocal;

            //validacion de carpetas y archivos arqueos
            try
            {
                if (!Directory.Exists(FSCierres))
                { MessageBox.Show("No se encontro carpeta con archivos para el Arqueo ...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                else
                {
                    if (archivosLocales.GetFiles().Count() == 1)
                    { MessageBox.Show("No se encontraron los archivos para el Arqueo ...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                    else
                    {
                        if (archivosLocales.GetFiles().Count() <= 5)
                        {
                            if (File.Exists(origAQ))
                            {
                                if (File.Exists(origCP))
                                {
                                    if (File.Exists(origEC))
                                    {
                                        if (File.Exists(origGF))
                                        {
                                            if (Directory.Exists(archivosTemp))
                                            {

                                                System.IO.File.Copy(carpetaLocales + "\\" + arcCierreAQ, archivosTemp + arcCierreAQF + ".dbf", true);        //copia archivo AQ y agrega .dbf
                                                System.IO.File.Copy(carpetaLocales + "\\" + arcCierreCP, archivosTemp + arcCierreCPF + ".dbf", true);        //copia archivo CP y agrega .dbf
                                                System.IO.File.Copy(carpetaLocales + "\\" + arcCierreEC, archivosTemp + arcCierreECF + ".dbf", true);        //copia archivo EC y agrega .dbf
                                                System.IO.File.Copy(carpetaLocales + "\\" + arcCierreGF, archivosTemp + arcCierreGFF + ".dbf", true);        //copia archivo GF y agrega .dbf


                                                //DataGridView Archivos
                                                String nom = fNom + ".dbf";
                                                String ruta = Cierres + "\\" + fechaProc1 + "\\" + nLocal1 + "\\";

                                                MessageBox.Show("Cargando archivos de arqueo", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                datoDGV(nom, ruta, nLocal1, fechaAr);
                                            }
                                            else
                                                {
                                                    DirectoryInfo temp = Directory.CreateDirectory(archivosTemp);                                              //crea un directorio nuevo

                                                    Thread.Sleep(2000);

                                                    File.Copy(carpetaLocales + "\\" + arcCierreAQ, archivosTemp + arcCierreAQF + ".dbf");                      //copia archivo AQ y agrega .dbf
                                                    File.Copy(carpetaLocales + "\\" + arcCierreCP, archivosTemp + arcCierreCPF + ".dbf");                      //copia archivo CP y agrega .dbf
                                                    File.Copy(carpetaLocales + "\\" + arcCierreEC, archivosTemp + arcCierreECF + ".dbf");                      //copia archivo EC y agrega .dbf
                                                    File.Copy(carpetaLocales + "\\" + arcCierreGF, archivosTemp + arcCierreGFF + ".dbf");                      //copia archivo GF y agrega .dbf

                                                    //DataGridView Archivos
                                                    String nom = fNom + ".dbf";
                                                    String ruta = Cierres + "\\" + fechaProc1 + "\\" + nLocal1 + "\\";

                                                    MessageBox.Show("Cargando archivos de arqueo", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                    datoDGV(nom, ruta, nLocal1, fechaAr);
                                                }
                                        }
                                        else
                                            { MessageBox.Show("Falta archivo " + arcCierreGF, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                                    }
                                    else
                                        { MessageBox.Show("Falta archivo " + arcCierreEC, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                                }
                                else
                                    { MessageBox.Show("Falta archivo " + arcCierreCP, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                            }
                            else
                                { MessageBox.Show("Falta archivo " + arcCierreAQ, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
                        }
                        else
                            { MessageBox.Show("Faltan archivos para el Arqueo, favor comprobar archivos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);}
                    }
                }
            }
            catch (Exception)
                { MessageBox.Show("Fecha y Local no corresponde al Arqueo ...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);}
        }

        public void datoDGV(String nomb, String ruta, int Local, String fechaArc)       //DataGridView archivos 
        {
            String cred = "credito personal";
            String cred1 = "CREDITO PERSONAL";
            String cred2 = "creditos personales";
            String cred3 = "CREDITOS PERSONALES";
            //.................variables grilla..............
            byte th = 0;                            //para asiento
            String dFecha_pro = String.Empty;
            int nuLocal = 0;
            String CODCUENTA = String.Empty;
            String NOMBRECTA = String.Empty;
            int DEBE = 0;
            int HABER = 0;
            String RUT = String.Empty;

            //-----modificado 15.01.2018 dnavarrete-----------------------
            //String consulta = "SELECT * FROM bf_BANCOFOL WHERE bf_LOCAL=" + Local;
            //String Consulta2 = "Select cb_" + txtLocal.Text + " from cb_COMPROBANTE where cb_FECHA = '" + txtDia.Text + "-" + txtMes.Text + "-" + txtAño.Text + "'";         
            //------------------------------------------------------------
            bool meddica = false;

            //valida locales meddica
            if (Local.Equals(2) || Local.Equals(16) || Local.Equals(201) || Local.Equals(202) || Local.Equals(203) || Local.Equals(204) || Local.Equals(205) || Local.Equals(206))
            {meddica = true;}
            else
                { meddica = false;}
            //............... inicio try-catch datos AQ-CP-EC-GF...........................................................................................................
            try
            {
                OdbcConnection c1 = new OdbcConnection();
                c1.ConnectionString = "Driver={Microsoft dBASE Driver (*.dbf)}; SourceType=DBF; DBQ=" + ruta + ";DriverID=277;";

                String queryAQ = "SELECT * FROM " + AQ + nomb + " WHERE LOCAL = '" + Local + "'";
                String queryCP = "SELECT * FROM " + CP + nomb + " WHERE LOCAL = '" + Local + "'";
                String queryEC = "SELECT * FROM " + EC + nomb + " WHERE LOCAL = '" + Local + "'";
                String queryGF = "SELECT * FROM " + GF + nomb + " WHERE LOCAL = '" + Local + "'";

                c1.Open();

                OdbcDataAdapter a1 = new OdbcDataAdapter(queryAQ, c1);
                DataTable t1 = new DataTable();

                OdbcDataAdapter a2 = new OdbcDataAdapter(queryCP, c1);
                DataTable t2 = new DataTable();

                OdbcDataAdapter a3 = new OdbcDataAdapter(queryEC, c1);
                DataTable t3 = new DataTable();

                OdbcDataAdapter a4 = new OdbcDataAdapter(queryGF, c1);
                DataTable t4 = new DataTable();

                //................... Archivos AQ, CP, EC, GF en Datatables...................................................................................

                if (okL)                //limpia datos List, dataTales y variables
                {
                    t1.Clear();
                    t2.Clear();
                    t3.Clear();
                    t4.Clear();
                    lista.Clear();
                    difRed = 0;
                }

                //llena tablas con datos de consultas

                a1.Fill(t1);
                a2.Fill(t2);
                a3.Fill(t3);
                a4.Fill(t4);

                c1.Close();
                //............... inicio llenado datagridview ............................................................................................

                String GLOSA = "";
                String TIPO = "";
                String DOCUMENTO = "";

                Boolean abcdin = false;

                if (Local == 2 || Local == 16)          //valida local para cuenta abc/din
                {abcdin = true;}
                else
                    { abcdin = false;}
                //...........................Inicio Centro CC ................................................
                String CC = Local.ToString();
                if (CC.Length == 3)
                { CC = "01-0" + Local;}
                else if (CC.Length == 2)
                        { CC = "01-00" + Local;}
                        else if (CC.Length == 1)
                                { CC = "01-000" + Local;}
                //...........................Fin Centro CC ................................................
                //...........................Inicio Foreach AQ ................................................

                foreach (DataRow r in t1.Rows)  //busqueda y asignacion de datos archivo AQ
                {
                    //..............................inicio de CLIENTES CON BOLETA..........................................................................
                    if (Convert.ToInt32(r[5]) > 0)
                    {
                        th = 0;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-041-002";                                  //cuentasCOD(6);                                  //"1-1-041-002";
                        NOMBRECTA = "CLIENTES CON BOLETAS";                         //cuentasNOM(6);                                  //"CLIENTES CON BOLETAS";
                        DEBE = Convert.ToInt32(r[5]);
                        HABER = 0;
                        RUT = "";
                        TIPO = "BN";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; ;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    //..............................Fin de CLIENTES CON BOLETA.............................................................................
                    //..............................Inicio de VENTAS CON BOLETAS...........................................................................
                    if (Convert.ToInt32(r[7]) > 0)
                    {
                        th = 1;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "6-1-010-001";                                  //cuentasCOD(29); // "6-1-010-001";
                        NOMBRECTA = "VENTAS CON BOLETAS";                           //cuentasNOM(29);
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[7]);
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; ;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    //..............................Fin de VENTAS CON BOLETAS.........................................................................
                    //..............................inicio de IVA debito fiscal.......................................................................
                    if (Convert.ToInt32(r[8]) > 0)
                    {
                        th = 0;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "2-1-300-001"; //cuentasCOD(28);
                        NOMBRECTA = "IVA DEBITO FISCAL"; //cuentasNOM(28);
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[8]);
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    //..............................fin de IVA debito fiscal...............................................................................
                    //..............................inicio de CLIENTES CON BOLETA..........................................................................
                    if (Convert.ToInt32(r[5]) > 0)
                    {
                        th = 5;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-041-002"; // cuentasCOD(6); 
                        NOMBRECTA = "CLIENTES CON BOLETAS"; //cuentasNOM(6); 
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[5]);
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    //..............................Fin de CLIENTES CON BOLETA...............................................................................
                    //..............................Inicio de CLIENTES/VENTAS CON BOLETA EXENTAS.............................................................
                    if (Convert.ToInt32(r[6]) >= 0)
                    {
                        th = 0;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-041-003"; //cuentasCOD(7);
                        NOMBRECTA = "CLIENTES CON BOLETAS EXENTAS"; //cuentasNOM(7); 
                        DEBE = Convert.ToInt32(r[6]);
                        HABER = 0;
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }

                    if (Convert.ToInt32(r[6]) >= 0)
                    {
                        th = 2;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "6-1-010-002"; //cuentasCOD(30); 
                        NOMBRECTA = "VENTAS CON BOLETAS EXENTAS"; //cuentasNOM(30);
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[6]);
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }

                    if (Convert.ToInt32(r[6]) >= 0)
                    {
                        th = 0;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-041-003"; //cuentasCOD(7); 
                        NOMBRECTA = "CLIENTES CON BOLETAS EXENTAS"; //cuentasNOM(7); 
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[6]);
                        RUT = "";
                        DOCUMENTO = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    //..............................Fin de CLIENTES/VENTAS CON BOLETA EXENTAS................................................................
                    //..............................inicio de banco de chile1................................................................................
                    try
                    {
                        if (Local != 0)
                        {
                            if (Convert.ToInt32(r[6]) >= 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"].ToString());
                                CODCUENTA = cuentaBL();                                             
                                NOMBRECTA = bancosL();                                             
                                DEBE = Convert.ToInt32(r[6]);
                                HABER = 0;
                                RUT = "";
                                TIPO = "PI";
                                GLOSA = "ARQUEO LOCAL " + Convert.ToInt32(r["LOCAL"].ToString()) + " " + fechaArc + " PINES";
                                DOCUMENTO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                        }
                        else
                            {
                                if (Convert.ToInt32(r[6]) >= 0)
                                {
                                    th = 6;
                                    //th2 = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "1-1-012-001";
                                    NOMBRECTA = "BANCO DE CHILE";
                                    DEBE = Convert.ToInt32(r[6]);
                                    HABER = 0;
                                    RUT = "";
                                    TIPO = "PI";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " PINES";
                                    //folio = "1";
                                    DOCUMENTO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    limpiarV();
                                }
                            }
                    }
                    catch (Exception e)
                    { MessageBox.Show("Problema al obtener folio " + e.ToString());}
                    //..............................fin de banco de chile1................................................................                    
                    //..............................inicio de banco de chile2................................................................
                    String ef = "E";
                    String ch = "C";
                    foreach (DataRow r3 in t3.Rows)
                    {
                        //............................seteo variables............................................
                        th = 0;
                        dFecha_pro = String.Empty;
                        nuLocal = 0;
                        CODCUENTA = String.Empty;
                        NOMBRECTA = String.Empty;
                        DEBE = 0;
                        HABER = 0;
                        RUT = String.Empty;
                        TIPO = String.Empty;
                        //...........................................................................................

                        if (Local.ToString() != "16")     //modificado 18.01.2018 dnavarrete
                        {
                            if (r3[4].Equals(ef))
                            {
                                try
                                {
                                    if (Local != 0) //&& folio != "")
                                    {
                                        if (Convert.ToInt32(r3[3]) >= 0)
                                        {
                                            th = 6;
                                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                            nuLocal = Convert.ToInt32(r["LOCAL"].ToString());
                                            CODCUENTA = cuentaBL(); 
                                            NOMBRECTA = bancosL();  
                                            DEBE = Convert.ToInt32(r3[3]);
                                            HABER = 0;
                                            RUT = "";
                                            TIPO = "EF";
                                            DOCUMENTO = r3[2].ToString();
                                            GLOSA = "ARQUEO LOCAL " + Convert.ToInt32(r["LOCAL"].ToString()) + " " + fechaArc + " Dep. " + DOCUMENTO + " Ef.";

                                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                            lista.Add(c);
                                            limpiarV();
                                        }
                                    }
                                    else
                                        {
                                            if (Convert.ToInt32(r3[3]) >= 0)
                                            {
                                                th = 6;
                                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                                nuLocal = Convert.ToInt32(r3["LOCAL"]);
                                                CODCUENTA = "1-1-012-001";
                                                NOMBRECTA = "BANCO DE CHILE";
                                                DEBE = Convert.ToInt32(r3[3]);
                                                HABER = 0;
                                                RUT = "";
                                                TIPO = "EF";
                                                DOCUMENTO = r3[2].ToString();
                                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " Dep. " + DOCUMENTO + " Ef.";
                                                //folio = "1";

                                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                                lista.Add(c);
                                                limpiarV();
                                            }
                                        }
                                }
                                catch (Exception e)
                                    { MessageBox.Show("Problema al obtener folio " + e.ToString());}
                            }
                            if (r3[4].Equals(ch))
                            {
                                try
                                {
                                    if (Local != 0)
                                    {
                                        if (Convert.ToInt32(r3[3]) >= 0)
                                        {
                                            th = 6;
                                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                            nuLocal = Convert.ToInt32(r["LOCAL"].ToString());
                                            CODCUENTA = cuentaBL(); 
                                            NOMBRECTA = bancosL(); 
                                            DEBE = Convert.ToInt32(r3[3]);
                                            HABER = 0;
                                            RUT = "";
                                            TIPO = "CH";
                                            DOCUMENTO = r3[2].ToString();
                                            GLOSA = "ARQUEO LOCAL " + Convert.ToInt32(r["LOCAL"].ToString()) + " " + fechaArc + " Dep. " + DOCUMENTO + " Ch.";

                                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                            lista.Add(c);
                                            limpiarV();
                                        }
                                    }
                                    else
                                        {
                                            if (Convert.ToInt32(r3[3]) >= 0)
                                            {
                                                th = 6;
                                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                                nuLocal = Convert.ToInt32(r3["LOCAL"]);
                                                CODCUENTA = "1-1-012-001";
                                                NOMBRECTA = "BANCO DE CHILE";
                                                DEBE = Convert.ToInt32(r3[3]);
                                                HABER = 0;
                                                RUT = "";
                                                TIPO = "CH";
                                                DOCUMENTO = r3[2].ToString();
                                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " Dep. " + DOCUMENTO + " Ch.";
                                                folio = "1";

                                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                                lista.Add(c);
                                                limpiarV();
                                            }
                                        }
                                }
                                catch (Exception e)
                                    { MessageBox.Show("Problema al obtener folio " + e.ToString());}
                            }
                        }
                    }
                    //..............................Fin de banco de chile2................................................................
                    //..............................inicio de CUENTA CORRIENTE AL PERSONAL...............................................................
                    foreach (DataRow r1 in t1.Rows)
                    {
                        if (r1.ToString() != "")
                        {
                            String pers = r1[46].ToString();
                            if (pers.Contains(cred) || pers.Contains(cred1) || pers.Contains(cred2) || pers.Contains(cred3))
                            {
                                if (Convert.ToInt32(r1[44]) > 0)
                                {
                                    th = 6;
                                    dFecha_pro = r1["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "1-1-041-004";   
                                    NOMBRECTA = "CUENTA CORRIENTE AL PERSONAL"; 
                                    DEBE = Convert.ToInt32(r1[44]);
                                    HABER = 0;
                                    RUT = "66.666.666-6";
                                    DOCUMENTO = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    limpiarV();
                                }
                                else
                                    {
                                        th = 6;
                                        dFecha_pro = r1["FECHA_PRO"].ToString().Substring(0, 10);
                                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                                        CODCUENTA = "1-1-041-004";
                                        NOMBRECTA = "CUENTA CORRIENTE AL PERSONAL";
                                        DEBE = Convert.ToInt32(r1[44]) * -1;
                                        HABER = 0;
                                        RUT = "66.666.666-6";
                                        DOCUMENTO = "";
                                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                        TIPO = "";

                                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                        lista.Add(c);
                                        limpiarV();
                                    }
                            }
                        }
                    }
                    //..............................Fin de CUENTA CORRIENTE AL PERSONAL................................................................
                    //...........................Inicio de CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS ................................................
                    if (meddica)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-014";  
                            NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS"; 
                            DEBE = Convert.ToInt32(r[13]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-014";  
                                NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";  
                                DEBE = 0;
                                HABER = (Convert.ToInt32(r[13])) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }

                        //...........................Fin de CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS .....................................................
                        //...........................Inicio de CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS ...................................................
                        if (Convert.ToInt32(r[12]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-013";  
                            NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            DEBE = Convert.ToInt32(r[12]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-013";  
                                NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS"; 
                                DEBE = 0;
                                HABER = (Convert.ToInt32(r[12])) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);//,th2);
                                lista.Add(c);
                                limpiarV();
                            }
                    }
                    //...........................Fin de CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS ...................................................
                    //..............................Inicio de CLIENTES TARJ. CREDITO FCV..............................................................
                    if (meddica == false)
                    {
                        //gesC = 0;
                        //int DEBEGesC;
                        //if (Convert.ToInt32(r[28]) > 0)       //modificacion 11.06.2018
                        //{
                        //    gesC = Convert.ToInt32(r[28]);
                        //}
                        //else
                        //    {
                        //        gesC = 0;
                        //    }

                        if (Convert.ToInt32(r[13]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-014"; //cuentasCOD(18); //
                            NOMBRECTA = "CLIENTES TARJ. CREDITO FCV"; //cuentasNOM(18); //
                                                                      //if (gesC > 0)
                                                                      //{
                                                                      //        DEBE = Convert.ToInt32(r[13]);// - gesC;
                                                                      //    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO + GES";
                                                                      //}
                                                                      //else
                                                                      //    {
                                                                      //DEBE = Convert.ToInt32(r[13]);
                                                                      //GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO";
                                                                      //    }
                            DEBE = Convert.ToInt32(r[13]);
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO";
                            HABER = 0;
                            RUT = "";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);//,th2);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-014"; 
                                NOMBRECTA = "CLIENTES TARJ. CREDITO FCV"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[13]) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " CREDITO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                        //..............................Fin de CLIENTES TARJ. CREDITO FCV......................................................................
                        //..............................Inicio de CLIENTES TARJ. DEBITO FCV....................................................................

                        if (Convert.ToInt32(r[12]) >= 0)
                        {
                            //gesD = 0;
                            //int DEBEGesD;
                            //if (Convert.ToInt32(r[29]) > 0)                           ////modificacion 11.06.2018
                            //{
                            //    gesD = Convert.ToInt32(r[29]);
                            //}
                            //else
                            //    {
                            //        gesD = 0;
                            //    }

                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-013"; //cuentasCOD(17); //
                            NOMBRECTA = "CLIENTES TARJ. DEBITO FCV"; //cuentasNOM(17); //
                                                                     //if (gesD > 0)
                                                                     //{
                                                                     //        DEBE = Convert.ToInt32(r[12]); //- gesD;
                                                                     //    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO + GES";
                                                                     //}
                                                                     //else
                                                                     //    {
                                                                     //        DEBE = Convert.ToInt32(r[12]);
                                                                     //        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO";
                                                                     //    }
                            DEBE = Convert.ToInt32(r[12]);
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO";
                            HABER = 0;
                            RUT = "";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-013"; //cuentasCOD(17); //
                                NOMBRECTA = "CLIENTES TARJ. DEBITO FCV"; //cuentasNOM(17); //
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[12]) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DEBITO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }

                    }
                    //..............................Fin de CLIENTES TARJ. DEBITO FCV......................................................................
                    //..............................inicio de CLIENTES CON TARJ. CRUZ VERDE...............................................................

                    if (!meddica)
                    {
                        if (Convert.ToInt32(r[16]) >= 0)
                        {
                            int tcv; //debeTCV;
                            if (Convert.ToInt32(r[31]) > 0)
                            {
                                tcv = Convert.ToInt32(r[31]);
                            }
                            else
                                { tcv = 0;}

                            th = 6;
                            //th2 = 0;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-001"; //cuentasCOD(9); //
                            NOMBRECTA = "CLIENTES CON TARJ. CRUZ VERDE"; //cuentasNOM(9); //
                            if (tcv > 0)
                            {
                                DEBE = Convert.ToInt32(r[16]) + tcv;
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " + GES";
                            }
                            else
                                {
                                    DEBE = Convert.ToInt32(r[16]);
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                }
                            HABER = 0;
                            RUT = "";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-001"; //cuentasCOD(9); //
                                NOMBRECTA = "CLIENTES CON TARJ. CRUZ VERDE"; //cuentasNOM(9); //
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[16]) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                    }
                    else
                        {
                            if (Convert.ToInt32(r[31]) >= 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-041-005";
                                NOMBRECTA = "CLIENTES FARMAZON";
                                DEBE = Convert.ToInt32(r[31]);
                                HABER = 0;
                                RUT = "";
                                DOCUMENTO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                            }
                            else
                                {
                                    th = 6;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "1-1-041-005";
                                    NOMBRECTA = "CLIENTES FARMAZON";
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r[31]) * -1;
                                    RUT = "";
                                    DOCUMENTO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                }
                        }
                    //..............................Fin de CLIENTES CON TARJ. CRUZ VERDE/FARMAZON..................................................................
                    //..............................inicio de CLIENTES CON CONVENIOS CREDITO..............................................................
                    if (Convert.ToInt32(r[17]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-015"; 
                        NOMBRECTA = "CLIENTES CON CONVENIOS CREDITO"; 
                        if (Convert.ToInt32(r[17]) + Convert.ToInt32(r[18]) > 0)
                        {
                            DEBE = Convert.ToInt32(r[17]) + Convert.ToInt32(r[18]);
                            HABER = 0;
                        }
                        else
                            {
                                DEBE = 0;
                                HABER = (Convert.ToInt32(r[17]) + Convert.ToInt32(r[18])) * -1;
                            }
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-015"; 
                            NOMBRECTA = "CLIENTES CON CONVENIOS CREDITO"; 
                            DEBE = 0;
                            HABER = (Convert.ToInt32(r[17]) + Convert.ToInt32(r[18])) * -1;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //..............................Fin de CLIENTES CON CONVENIOS CREDITO................................................................
                    //..............................Inicio de DICC CID CIE VOUCHER-NOMINAS...............................................................

                    if (Convert.ToInt32(r[19]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-016";
                        NOMBRECTA = "DICC CID CIE VOUCHER-NOMINAS"; //cuentasNOM(50); //
                        DEBE = Convert.ToInt32(r[19]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; ;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-016"; 
                            NOMBRECTA = "DICC CID CIE VOUCHER-NOMINAS"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[19]) * -1;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //..............................Fin de DICC CID CIE VOUCHER-NOMINAS...............................................................
                    //..............................Fin de VENTAS CLIENTES CONSALUD...............................................................
                    if (abcdin == false)
                    {
                        if (Convert.ToInt32(r[26]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-017"; //cuentasCOD(51); //
                            NOMBRECTA = "VENTAS CLIENTES CONSALUD"; //cuentasNOM(id); //
                            DEBE = Convert.ToInt32(r[26]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-042-017";
                                NOMBRECTA = "VENTAS CLIENTES CONSALUD"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[26]) * -1;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                    }
                    //..............................Fin de VENTAS CLIENTES CONSALUD...............................................................
                    //...........................Inicio de Paris/Cencosud ........................................................................
                    if (Convert.ToInt32(r[21]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-007"; 
                        NOMBRECTA = "CLIENTE CON TARJETA PARIS"; 
                        DEBE = Convert.ToInt32(r[21]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-007"; 
                            NOMBRECTA = "CLIENTE CON TARJETA PARIS";
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[21]) * -1;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }

                    if (Convert.ToInt32(r[22]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-007"; 
                        NOMBRECTA = "CLIENTE CON TARJETA PARIS"; 
                        DEBE = Convert.ToInt32(r[22]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " (CENCOSUD)";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-007"; 
                            NOMBRECTA = "CLIENTE CON TARJETA PARIS"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[22]) * -1;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " (CENCOSUD)";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //............................Fin de Paris Cencosud ...............................................................................
                    //............................Inicio de CMR ...............................................................................
                    if (Convert.ToInt32(r[25]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-002"; 
                        NOMBRECTA = "CLIENTES TARJ. FALABELLA"; 
                        DEBE = Convert.ToInt32(r[25]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-002"; 
                            NOMBRECTA = "CLIENTES TARJ. FALABELLA"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[25]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................Fin de CMR............................................................................................
                    //...........................Inicio de DIN/ABC............................................................................................
                    if (abcdin)
                    {
                        if (Convert.ToInt32(r[26]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-011"; 
                            NOMBRECTA = "CLIENTES TARJ. DIN/ABC"; 
                            DEBE = Convert.ToInt32(r[26]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc; 
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                if (Convert.ToInt32(r[26]) < 0)
                                {
                                    th = 6;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "1-1-042-011"; 
                                    NOMBRECTA = "CLIENTES TARJ. DIN/ABC"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r[26]) * -1;
                                    RUT = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    limpiarV();
                                }
                            }
                    }
                    //...........................fin de DIN/ABC...........................................................................................
                    //...........................Inicio de Avance efectivo.............................................................................
                    if (Convert.ToInt32(r[33]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-062-009"; 
                        NOMBRECTA = "AVANCES EN EFECTIVO TARJETAS"; 
                        DEBE = Convert.ToInt32(r[33]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-062-009"; 
                            NOMBRECTA = "AVANCES EN EFECTIVO TARJETAS"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[33]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................Fin de Avance efectivo................................................................................
                    //...........................Inicio de TARJETA RIPLEY..............................................................................

                    if (Convert.ToInt32(r[24]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-010"; 
                        NOMBRECTA = "TARJETA RIPLEY"; 
                        DEBE = Convert.ToInt32(r[24]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-010"; 
                            NOMBRECTA = "TARJETA RIPLEY"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[24]) *-1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................Fin de TARJETA RIPLEY.................................................................................
                    //...........................Inicio de TARJETA CYD.................................................................................

                    if (Convert.ToInt32(r[35]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-042-005"; 
                        NOMBRECTA = "CLIENTE TARJETA CYD (SUR)"; 
                        DEBE = Convert.ToInt32(r[35]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-042-005"; 
                            NOMBRECTA = "CLIENTE TARJETA CYD (SUR)"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[35]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................Fin de TARJETA CYD....................................................................................
                    //...........................inicio de ACREEDORES CRUZ VERDE....................................................................................
                    if (Convert.ToInt32(r[36]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "2-1-091-002"; 
                        NOMBRECTA = "ACREEDORES CRUZ VERDE"; 
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[36]);
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "2-1-091-002"; 
                            NOMBRECTA = "ACREEDORES CRUZ VERDE"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[36]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................fin de ACREEDORES CRUZ VERDE....................................................................................
                    //...........................inicio de ACREEDORES TARJETA A. PARIS....................................................................................
                    if (Convert.ToInt32(r[39]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "2-1-091-001"; 
                        NOMBRECTA = "ACREEDORES TARJETA A. PARIS"; 
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[39]); ;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "2-1-091-001"; 
                            NOMBRECTA = "ACREEDORES TARJETA A. PARIS"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[39]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................fin de ACREEDORES TARJETA A. PARIS....................................................................................
                    //...........................inicio de ACREEDORES HOGAR DE CRISTO...................................................................................

                    if (Convert.ToInt32(r[38]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "2-1-092-002"; 
                        NOMBRECTA = "ACREEDORES HOGAR DE CRISTO";
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[38]); ;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "2-1-092-002"; 
                            NOMBRECTA = "ACREEDORES HOGAR DE CRISTO"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[38]) * -1; 
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................fin de ACREEDORES HOGAR DE CRISTO...................................................................................
                    //...........................inicio de ACREEDOR SOLVENTA...................................................................................

                    if (Convert.ToInt32(r[37]) >= 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "2-1-092-003"; 
                        NOMBRECTA = "ACREEDOR SOLVENTA";
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[37]);
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "2-1-092-003"; 
                            NOMBRECTA = "ACREEDOR SOLVENTA"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[37]) * -1;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    //...........................fin de ACREEDOR SOLVENTA...................................................................................
                    //...........................inicio de RECAUDACION BANMEDICA...................................................................................

                    if (r[27].ToString() == "" || Convert.ToInt32(r[27]) == 0)
                    {
                        if (Convert.ToInt32(r[41]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "2-1-092-010"; 
                            NOMBRECTA = "RECAUDACION BANMEDICA"; 
                            DEBE = 0;
                            HABER = Convert.ToInt32(r[41]);
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " EFECTIVO";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                        else
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "2-1-092-010"; 
                                NOMBRECTA = "RECAUDACION BANMEDICA";
                                DEBE = Convert.ToInt32(r[41]) * -1;         
                                HABER = 0;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " EFECTIVO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                    }
                    else
                        {
                            if (Convert.ToInt32(r[27]) >= 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "2-1-092-010"; 
                                NOMBRECTA = "RECAUDACION BANMEDICA"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[27]); //int HABER = Convert.ToInt32(r[41]) + eGES;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " EFECTIVO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                            else
                                {
                                    th = 6;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "2-1-092-010"; 
                                    NOMBRECTA = "RECAUDACION BANMEDICA"; 
                                    DEBE = Convert.ToInt32(r[27]) * -1; //int HABER = Convert.ToInt32(r[41]) + eGES;      modificado 15.01.2018 dnavarrete
                                    HABER = 0;
                                    RUT = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " EFECTIVO";
                                    DOCUMENTO = "";
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    limpiarV();
                                }
                        }
                    //...........................fin de RECAUDACION BANMEDICA...................................................................................
                    //...........................Inicio de DEPOSITOS RECETARIO MAGISTRAL.........................................................

                    if (Convert.ToInt32(r[32]) > 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-012-054"; 
                        NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL"; 
                        DEBE = Convert.ToInt32(r[32]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " ABONO PREVTA. REC. MAG.";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            if (Convert.ToInt32(r[32]) < 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-012-054"; 
                                NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[32]) * -1;    //modificado 15.01.2018 dnavarrete
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " ABONO PREVTA. REC. MAG.";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                        }
                    //----------------------------------------------------------------------------------------------------------------------------------------------------

                    if (Convert.ToInt32(r[40]) > 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-012-054"; 
                        NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL"; 
                        DEBE = Convert.ToInt32(r[40]);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " REC. MAG. EFECTIVO";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            if (Convert.ToInt32(r[40]) < 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-012-054"; 
                                NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[40]) * -1;    //modificado 15.01.2018 dnavarrete
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " REC. MAG. EFECTIVO";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);//,th2);
                                lista.Add(c);
                                limpiarV();
                            }
                        }
                    //..............................Fin de DEPOSITOS RECETARIO MAGISTRAL (PREV)...........................................................
                    //..............................inicio de CLIENTES POR COBRAR.........................................................................

                    if (Convert.ToInt32(r[18]) < 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-043-003"; 
                        NOMBRECTA = "CLIENTES POR COBRAR"; 
                        DEBE = Convert.ToInt32(r[18]) * -1;
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DESCTO. BONIFICACION";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                        {
                            if (Convert.ToInt32(r[18]) >= 0)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-043-003"; 
                                NOMBRECTA = "CLIENTES POR COBRAR"; 
                                DEBE = 0;
                                HABER = Convert.ToInt32(r[18]);
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DESCTO. BONIFICACION";
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                        }

                    if (Convert.ToInt32(r[18]) < 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0,10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-043-002"; 
                        NOMBRECTA = "CLIENTES CON GUIAS CRUZ VERDE"; 
                        DEBE = 0;
                        HABER = Convert.ToInt32(r[18]) * -1;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DESCTO. BONIFICACION";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                    }
                    else
                    {
                        if (Convert.ToInt32(r[18]) >= 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-043-002"; 
                            NOMBRECTA = "CLIENTES CON GUIAS CRUZ VERDE"; 
                            DEBE = Convert.ToInt32(r[18]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " DESCTO. BONIFICACION";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                        }
                    }
                    //..............................Fin de CLIENTES POR COBRAR............................................................................
                    //..............................Fin de Clientes con boleta - rut - doc................................................................               
                    //...........................Inicio Foreach GF .......................................................................................
                    double V_bmx = 0;
                    int V_credit = 0;

                        foreach (DataRow r4 in t4.Rows)
                        {
                        //.......................................................
                            th = 0;
                            dFecha_pro = String.Empty;
                            nuLocal = 0;
                            CODCUENTA = String.Empty;
                            NOMBRECTA = String.Empty;
                            DEBE = 0;
                            HABER = 0;
                            RUT = String.Empty;
                            TIPO = String.Empty;
                            DOCUMENTO = String.Empty;
                        //......................................................
                            if (r4[5].ToString() == "G")
                            {
                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 2;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-043-002"; 
                                    NOMBRECTA = "CLIENTES CON GUIAS CRUZ VERDE"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r4[4]);
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "GU";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }
                            }

                            if (r4[5].ToString() == "E")
                            {
                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-001"; 
                                    NOMBRECTA = "CLIENTES CON FACTURA"; 
                                    DEBE = Convert.ToInt32(r4[4]);
                                    HABER = 0;
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "FD";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 3;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "6-1-010-003"; 
                                    NOMBRECTA = "VENTAS CON FACTURA LOCALES"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(Convert.ToInt32(r4[4]) / 1.19);
                                    RUT = r4["RUT"].ToString(); 
                                    TIPO = "";
                                    DOCUMENTO = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "2-1-300-001"; 
                                    NOMBRECTA = "IVA DEBITO FISCAL"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r4[4]) - Convert.ToInt32(Convert.ToInt32(r4[4]) / 1.19);
                                    RUT = "";
                                    TIPO = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-001"; 
                                    NOMBRECTA = "CLIENTES CON FACTURA"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r4[4]);
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "FD";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }
                            }

                            if (r4[5].ToString() == "C")
                            {
                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-002"; 
                                    NOMBRECTA = "CLIENTES CON BOLETAS";
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r4[4]);
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "NA";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 4;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "6-1-010-001";        //modificada 09.05.2017
                                    NOMBRECTA = "VENTAS CON BOLETA"; 
                                    DEBE = Convert.ToInt32(Convert.ToInt32(r4[4]) / 1.19);
                                    HABER = 0;
                                    RUT = "";
                                    ncRUT = r4["RUT"].ToString();
                                    TIPO = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = ""; 
                                    ncDocumento = r4[3].ToString();

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "2-1-300-001"; 
                                    NOMBRECTA = "IVA DEBITO FISCAL"; 
                                    DEBE = Convert.ToInt32(r4[4]) - Convert.ToInt32(Convert.ToInt32(r4[4]) / 1.19);
                                    HABER = 0;
                                    RUT = "";
                                    TIPO = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 5;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-001"; 
                                    NOMBRECTA = "CLIENTES CON BOLETAS";
                                    DEBE = Convert.ToInt32(r4[4]);
                                    HABER = 0;
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "NA";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + HABER;
                                    limpiarV();
                                }
                            }

                            if (r4[5].ToString() == "M")
                            {
                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 1;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-002"; 
                                    NOMBRECTA = "CLIENTES CON BOLETAS"; 
                                    DEBE = Convert.ToInt32(r4[4]);
                                    HABER = 0;
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "BM";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_bmx = V_bmx + DEBE;
                                    limpiarV();
                                }
                            }

                            if (V_bmx > 0)
                            {
                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 2;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "6-1-010-001"; 
                                    NOMBRECTA = "VENTAS CON BOLETAS";  
                                    DEBE = 0;
                                    V_bmx = Math.Round(V_bmx - Math.Round(V_bmx * 0.19));
                                    HABER = Convert.ToInt32(V_bmx);
                                    RUT = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "2-1-300-001"; 
                                    NOMBRECTA = "IVA DEBITO FISCAL"; 
                                    V_bmx = Math.Round(V_bmx * 0.19);
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(V_bmx);
                                    RUT = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + DEBE;
                                    limpiarV();
                                }

                                if (Convert.ToInt32(r4[4]) > 0)
                                {
                                    th = 0;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                    nuLocal = Convert.ToInt32(r4["LOCAL"]);
                                    CODCUENTA = "1-1-041-002"; 
                                    NOMBRECTA = "CLIENTES CON BOLETAS"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(V_bmx);
                                    RUT = r4["RUT"].ToString();
                                    TIPO = "BM";
                                    DOCUMENTO = r4[3].ToString();
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " RUT " + RUT + " Doc. " + DOCUMENTO;

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    V_credit = V_credit + HABER;
                                    limpiarV();
                                }
                            }
                        }
                    //...........................Fin Foreach GF ..............................................................................................
                    //...........................Inicio DIFERENCIA POR RECAUDACION/DEPOSITO PENDIENTE.......................................................................................
                foreach (DataRow r1 in t1.Rows)
                {
                    //...........................................validar deposito anticipado/diferencia de recaudacion 10.01.2018 dnavarrete.................................................
                    String pers = r1[46].ToString();
                    bool cjch = false;
                    bool crp = false;                    

                    if (Local.ToString() == "16")
                    { l16 = true;}

                    int dfl16 = Convert.ToInt32(r[44]);

                    if (dfl16 == 0)
                    { dl16 = false; }
                        else { dl16 = true; }

                    if (Convert.ToInt32(r[44]) > 0)
                    {                           
                        //...............................................caja chica ....................................................
                        String cajaChica = r[46].ToString();
                        if (cajaChica.Contains("CAJA CHICA") || cajaChica.Contains("caja chica") || cajaChica.Contains("Caja Chica") || cajaChica.Contains("Caja chica"))
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-012-055"; 
                            NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                            DEBE = Convert.ToInt32(r[44]);
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " REPOSICION CAJA CHICA";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            cjch = true;
                            limpiarV();
                        }
                        //.........................................................................................................................
                        //............................................credito personal......................................................
                        pers = r[46].ToString();
                        if (pers.Contains(cred) || pers.Contains(cred1) || pers.Contains(cred2) || pers.Contains(cred3))
                        { /*comprueba credito personal */
                            crp = true;
                        }
                        //..................................................................................................................
                        //........................................analizar valor diferencia..................................................
                        if (cjch == true && crp == false || cjch == false && crp == true)
                        {/*si encuentra caja chica o credito personal*/}
                        else
                            {
                                if (Convert.ToInt32(r[44]) <= 500)
                                {
                                    th = 6;
                                    dFecha_pro = r["FECHA_PRO"].ToString().Substring(0,10);
                                    nuLocal = Convert.ToInt32(r["LOCAL"]);
                                    CODCUENTA = "1-1-012-055"; 
                                    NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                                    DEBE = 0;
                                    HABER = Convert.ToInt32(r[44]);
                                    RUT = "";
                                    GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                    DOCUMENTO = "";
                                    TIPO = "";

                                    Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                    lista.Add(c);
                                    limpiarV();
                                }
                                else
                                    {
                                        th = 6;
                                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0,10);
                                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                                        CODCUENTA = "1-1-012-053"; //cuentasCOD(3);              //				//"2-1-500-002";
                                        NOMBRECTA = "DEPOSITO PENDIENTE"; //cuentasNOM(3);              //		//"DEPOSITO ANTICIPADO";
                                        DEBE = Convert.ToInt32(r[44]);              //0;
                                        HABER = 0;                                  //Convert.ToInt32(r[44]);	
                                        RUT = "";
                                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                        DOCUMENTO = "";
                                        TIPO = "";

                                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                        lista.Add(c);
                                        limpiarV();
                                        //...................................................................................................................
                                    }
                            }
                    }

                    if (Convert.ToInt32(r[44]) < 0)
                    {
                        //.....................................................caja chica.......................................................................
                        String cajaChica = r[46].ToString();
                        if (cajaChica.Contains("CAJA CHICA") || cajaChica.Contains("caja chica"))
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0,10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-012-055"; 
                            NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                            DEBE = Convert.ToInt32(r[44]) * -1;
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " REPOSICION CAJA CHICA";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            cjch = true;
                            limpiarV();
                            //..................................................................................................................................
                        }
                        //................................................credito personal ................................................................
                        pers = r[46].ToString();
                        if (pers.Contains(cred) || pers.Contains(cred1) || pers.Contains(cred2) || pers.Contains(cred3))
                        { /*comprueba credito personal*/ crp = true; }
                        //..................................................................................................................................                            

                        if (cjch == true && crp == false || cjch == false && crp == true) //&& l16 == false)
                        {/*si encuentra caja chica o credito personal*/}
                        else
                        {
                            if (Convert.ToInt32(r[44]) >= -500)
                            {
                                th = 6;
                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                CODCUENTA = "1-1-012-055"; 
                                NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                                DEBE = Convert.ToInt32(r[44]) * -1;
                                HABER = 0;
                                RUT = "";
                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                DOCUMENTO = "";
                                TIPO = "";

                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                lista.Add(c);
                                limpiarV();
                            }
                            else
                            {
                                if (l16)
                                {
                                    String ef1 = "E";
                                    String ch1 = "C";

                                    foreach (DataRow r3 in t3.Rows)
                                    {
                                        th = 0;
                                        dFecha_pro = String.Empty;
                                        nuLocal = 0;
                                        CODCUENTA = String.Empty;
                                        NOMBRECTA = String.Empty;
                                        DEBE = 0;
                                        HABER = 0;
                                        RUT = String.Empty;
                                        TIPO = String.Empty;
                                        DOCUMENTO = String.Empty;

                                        if (r3[4].Equals(ef1))
                                        {
                                            th = 6;
                                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                                            CODCUENTA = "1-1-012-053"; 
                                            NOMBRECTA = "DEPOSITO PENDIENTE"; 
                                            DEBE = Convert.ToInt32(r3[3]);                          //(Convert.ToInt32(r[44]) * -1) + Convert.ToInt32(r3[3]);
                                            HABER = 0;
                                            RUT = "";
                                            TIPO = "";                                              //"EF"; modificado 11.04.2018 dnavarrete
                                            DOCUMENTO = "";                                         //r3[2].ToString(); modificado 11.04.2018 dnavarrete
                                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;       // + " Dep. " + DOCUMENTO + " Ef."; modificado 11.04.2018 dnavarrete

                                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                            lista.Add(c);
                                            limpiarV();
                                        }
                                        else
                                        {
                                            if (r3[4].Equals(ch1))
                                            {
                                                th = 6;
                                                dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                                nuLocal = Convert.ToInt32(r["LOCAL"]);
                                                CODCUENTA = "1-1-012-053"; 
                                                NOMBRECTA = "DEPOSITO PENDIENTE"; 
                                                DEBE = Convert.ToInt32(r3[3]);  //(Convert.ToInt32(r[44]) * -1) + Convert.ToInt32(r3[3]);
                                                HABER = 0;
                                                TIPO = "CH";
                                                DOCUMENTO = "";         //r3[2].ToString(); modificado 11.04.2018
                                                GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;       // + " Dep. " + DOCUMENTO + " Ch.";
                                                RUT = "";

                                                Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                                lista.Add(c);
                                                limpiarV();
                                            }
                                        }
                                    }//fin foreach r3
                                }
                                else
                                    {
                                        th = 6;
                                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                                        CODCUENTA = "2-1-500-002"; 
                                        NOMBRECTA = "DEPOSITO ANTICIPADO"; 
                                        DEBE = Convert.ToInt32(r[44]) * -1; 
                                        HABER = 0;                              //Convert.ToInt32(r[44]);
                                        RUT = "";
                                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                                        DOCUMENTO = "";
                                        TIPO = "";

                                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                                        lista.Add(c);
                                        limpiarV();
                                    }
                            }
                        }
                        //}
                        //}
                    }
                    //.........................................................................................................................................................................

                    String redondeo = r[30].ToString();                            //variable ley de redondeo
                    if (Convert.ToInt32(redondeo) > 0)
                    {
                        th = 6;
                        dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                        nuLocal = Convert.ToInt32(r["LOCAL"]);
                        CODCUENTA = "1-1-012-055";
                        NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                        DEBE = Convert.ToInt32(redondeo);
                        HABER = 0;
                        RUT = "";
                        GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " LEY DE REDONDEO";
                        DOCUMENTO = "";
                        TIPO = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                        lista.Add(c);
                        limpiarV();
                        drd = true;
                    }
                    else
                    {
                        if (Convert.ToInt32(redondeo) < 0)
                        {
                            th = 6;
                            dFecha_pro = r["FECHA_PRO"].ToString().Substring(0, 10);
                            nuLocal = Convert.ToInt32(r["LOCAL"]);
                            CODCUENTA = "1-1-012-055"; 
                            NOMBRECTA = "DIFERENCIA POR RECAUDACION"; 
                            DEBE = Convert.ToInt32(redondeo) * -1;
                            HABER = 0;
                            RUT = "";
                            GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc + " LEY DE REDONDEO";
                            DOCUMENTO = "";
                            TIPO = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA, th);
                            lista.Add(c);
                            limpiarV();
                            drd = true;
                        }
                    }
                    } //.....................fin foreach.........................................................................................................
                      //...........................ininio llenado datagridview y textbox...............................................................................
                      //...........................inicio try-catch dgv-voucher........................................................................................
                    try
                    {
                        dgvDatosVaucher.DataSource = lista;
                        dgvDatosVaucher.Columns[11].Visible = false;     //oculta columna con tipo de documento                        

                        double sumaDebe = 0;
                        double sumaHaber = 0;
                        double debeDif = 0;
                        double haberDif = 0;
                        double diferenciaDif = 0;

                        foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                        {
                            sumaDebe += Convert.ToInt32(row.Cells[5].Value);       
                            sumaHaber += Convert.ToInt32(row.Cells[6].Value);       
                        }

                        if (okL)
                        {

                            debeDif = sumaDebe;
                            txtTotalDebe.Text = debeDif.ToString("C0");

                            haberDif = sumaHaber;
                            txtTotalHaber.Text = haberDif.ToString("C0");

                            diferenciaDif = sumaDebe - sumaHaber;
                        }
                        else
                            {
                                debeDif = sumaDebe;
                                txtTotalDebe.Text = debeDif.ToString("C0");

                                haberDif = sumaHaber;
                                txtTotalHaber.Text = haberDif.ToString("C0");

                                diferenciaDif = sumaDebe - sumaHaber;
                                txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                            }

                        if (diferenciaDif == 0)
                        { txtDiferenciaDH.Text = "Comprobante OK";}
                        else
                            {
                                sumaDebe = 0;
                                sumaHaber = 0;

                                if (diferenciaDif != 0 && diferenciaDif <= 10 && diferenciaDif >= -10)
                                {
                                    if (MessageBox.Show("¿Desea Agregar Diferencia por Recaudación?", "Direfencia Recaudación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                                    {
                                        String fecha, cc, rut, doc, tipo, glosa;
                                        int local, debeN, haberN;

                                        if (diferenciaDif > 0 && diferenciaDif <= 10)
                                        {
                                            th = 6;
                                            fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                            local = Convert.ToInt32(txtLocal.Text);
                                            CODCUENTA = "1-1-012-055"; 
                                            NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                       
                                            debeN = 0;
                                            haberN = Convert.ToInt32(diferenciaDif);
                                            cc = CC;
                                            rut = "";
                                            doc = "";
                                            tipo = "";
                                            glosa = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                            Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                            lista.Add(list);
                                            limpiarV();
                                            
                                            dgvDatosVaucher.DataSource = null;
                                            dgvDatosVaucher.DataSource = lista;
                                            dgvDatosVaucher.Columns[11].Visible = false;

                                            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                            {
                                                sumaDebe += Convert.ToInt32(row.Cells[5].Value);        
                                                sumaHaber += Convert.ToInt32(row.Cells[6].Value);      
                                            }

                                            double debe1 = sumaDebe;
                                            txtTotalDebe.Text = debe1.ToString("C0");

                                            double haber1 = sumaHaber;
                                            txtTotalHaber.Text = haber1.ToString("C0");

                                            double di = sumaDebe - sumaHaber;

                                            if (di == 0)
                                            {
                                                txtDiferenciaDH.Text = "Comprobante OK";
                                            }

                                        }
                                        else
                                            {
                                                if (diferenciaDif < 0 && diferenciaDif >= -10)
                                                {
                                                    th = 6;
                                                    fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                    local = Convert.ToInt32(txtLocal.Text);
                                                    CODCUENTA = "1-1-012-055"; 
                                                    NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                             
                                                    debeN = Convert.ToInt32(diferenciaDif) * -1;
                                                    haberN = 0;
                                                    cc = CC;
                                                    rut = "";
                                                    doc = "";
                                                    tipo = "";
                                                    glosa = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                                    Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                    lista.Add(list);
                                                    limpiarV();

                                                    dgvDatosVaucher.DataSource = null;
                                                    dgvDatosVaucher.DataSource = lista;
                                                    dgvDatosVaucher.Columns[11].Visible = false;

                                                    foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                    {
                                                        sumaDebe += Convert.ToInt32(row.Cells[5].Value);        
                                                        sumaHaber += Convert.ToInt32(row.Cells[6].Value);       
                                                    }

                                                    double debe1 = sumaDebe;
                                                    txtTotalDebe.Text = debe1.ToString("C0");

                                                    double haber1 = sumaHaber;
                                                    txtTotalHaber.Text = haber1.ToString("C0");

                                                    double di = sumaDebe - sumaHaber;

                                                    if (di == 0)
                                                    {
                                                        txtDiferenciaDH.Text = "Comprobante OK";
                                                    }
                                                }
                                          }
                                    }
                                }
                                else
                                    {
                                        if (txtLocal.Text == "16")
                                        {
                                            if (MessageBox.Show("¿Desea Agregar Deposito Pendiente?", "Deposito Pendiente", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                                            {
                                                String fecha, cc, rut, doc, tipo, glosa;
                                                int local, debeN, haberN;

                                                if (diferenciaDif < 0)
                                                {
                                                    th = 6;
                                                    fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                    local = Convert.ToInt32(txtLocal.Text);
                                                    CODCUENTA = "1-1-012-055";
                                                    NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                      
                                                    debeN = Convert.ToInt32(diferenciaDif) * -1;
                                                    haberN = 0;
                                                    cc = CC;
                                                    rut = "";
                                                    doc = "";
                                                    tipo = "";
                                                    glosa = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                                    Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                    lista.Add(list);
                                                    limpiarV();

                                                    dgvDatosVaucher.DataSource = null;
                                                    dgvDatosVaucher.DataSource = lista;
                                                    dgvDatosVaucher.Columns[11].Visible = false;        //columna th                                             

                                                    foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                    {
                                                        sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                        sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                    }

                                                    double debe1 = sumaDebe;
                                                    txtTotalDebe.Text = debe1.ToString("C0");

                                                    double haber1 = sumaHaber;
                                                    txtTotalHaber.Text = haber1.ToString("C0");

                                                    double di = sumaDebe - sumaHaber;

                                                    if (di == 0)
                                                    {
                                                        txtDiferenciaDH.Text = "Comprobante OK";
                                                    }
                                                }
                                                else
                                                    {
                                                        if (diferenciaDif > 0)
                                                        {
                                                            th = 6;
                                                            fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                            local = Convert.ToInt32(txtLocal.Text);
                                                            CODCUENTA = "1-1-012-055";
                                                            NOMBRECTA = "DIFERENCIA POR RECAUDACION"; //cuentasNOM(3); //                                           
                                                            debeN = 0;
                                                            haberN = Convert.ToInt32(diferenciaDif);
                                                            cc = CC;
                                                            rut = "";
                                                            doc = "";
                                                            tipo = "";
                                                            glosa = GLOSA;

                                                            Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                            lista.Add(list);
                                                            limpiarV();

                                                            dgvDatosVaucher.DataSource = null;
                                                            dgvDatosVaucher.DataSource = lista;
                                                            dgvDatosVaucher.Columns[11].Visible = false;

                                                            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                            {
                                                                sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                                sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                            }

                                                            double debe1 = sumaDebe;
                                                            txtTotalDebe.Text = debe1.ToString("C0");

                                                            double haber1 = sumaHaber;
                                                            txtTotalHaber.Text = haber1.ToString("C0");

                                                            double di = sumaDebe - sumaHaber;

                                                            if (di == 0)
                                                            {
                                                                txtDiferenciaDH.Text = "Comprobante OK";
                                                            }
                                                        }
                                                    }
                                            }
                                        }
                                        foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                        {
                                            sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                            sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                        }

                                        double df = sumaDebe - sumaHaber;
                                        
                                        //................................ redondeo .......................................................................................................
                                        
                                        if (df != 0 && df > 10 || df < -10)
                                        {
                                            if (!drd)
                                            {
                                                if (MessageBox.Show("¿Se ha encontrado una diferencia de " + df + " Pesos?, dicha diferencia es por ¿Ley de Redondeo?", "Direfencia Recaudación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                                                {
                                                    valRedondeo f = new valRedondeo();
                                                    if (f.ShowDialog() == DialogResult.OK)
                                                    {
                                                        vaRED = f.vRed;
                                                    }

                                                    String fecha, cc, rut, doc, tipo, glosa;
                                                    int local, debeN, haberN, redvnp;

                                                    redvnp = Convert.ToInt32(vaRED);

                                                    if (redvnp < 0)
                                                    {
                                                        th = 6;
                                                        fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                        local = Convert.ToInt32(txtLocal.Text);
                                                        CODCUENTA = "1-1-012-055"; 
                                                        NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                  
                                                        debeN = Convert.ToInt32(vaRED) * -1;
                                                        haberN = 0;
                                                        cc = CC;
                                                        rut = "";
                                                        doc = "";
                                                        tipo = "";
                                                        glosa = "ARQUEO LOCAL " + Local + " " + fechaArc + " LEY DE REDONDEO";

                                                        Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                        lista.Add(list);
                                                        limpiarV();

                                                        dgvDatosVaucher.DataSource = null;
                                                        dgvDatosVaucher.DataSource = lista;
                                                        dgvDatosVaucher.Columns[11].Visible = false;

                                                        foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                        {
                                                            sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                            sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                        }

                                                        if (okL)
                                                        {
                                                            debeDif = sumaDebe;
                                                            txtTotalDebe.Text = debeDif.ToString("C0");

                                                            haberDif = sumaHaber;
                                                            txtTotalHaber.Text = haberDif.ToString("C0");

                                                            if (difRed == 0)
                                                            {
                                                                diferenciaDif = debeDif - haberDif;
                                                            }
                                                            else
                                                                {
                                                                    diferenciaDif = debeDif - haberDif - difRed;
                                                                }
                                                        }
                                                        else
                                                        {
                                                            debeDif = sumaDebe;
                                                            txtTotalDebe.Text = debeDif.ToString("C0");

                                                            haberDif = sumaHaber;
                                                            txtTotalHaber.Text = haberDif.ToString("C0");

                                                            diferenciaDif = debeDif - haberDif;
                                                        }

                                                        difRed = diferenciaDif;

                                                        if (diferenciaDif == 0)
                                                        {
                                                            txtDiferenciaDH.Text = "Comprobante OK";
                                                            cOK = true;
                                                        }
                                                        else
                                                        {
                                                            txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (redvnp > 0)
                                                        {
                                                            th = 6;
                                                            fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                            local = Convert.ToInt32(txtLocal.Text);
                                                            CODCUENTA = "1-1-012-055"; 
                                                            NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                   
                                                            debeN = 0;
                                                            haberN = Convert.ToInt32(vaRED);
                                                            cc = CC;
                                                            rut = "";
                                                            doc = "";
                                                            tipo = "";
                                                            glosa = "ARQUEO LOCAL " + Local + " " + fechaArc + " LEY DE REDONDEO";

                                                            Consultor list2 = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                            lista.Add(list2);
                                                            limpiarV();

                                                            dgvDatosVaucher.DataSource = null;
                                                            dgvDatosVaucher.DataSource = lista;
                                                            dgvDatosVaucher.Columns[11].Visible = false;

                                                            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                            {
                                                                sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                                sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                            }

                                                            if (okL)
                                                            {
                                                                debeDif = sumaDebe;
                                                                txtTotalDebe.Text = debeDif.ToString("C0");

                                                                haberDif = sumaHaber;
                                                                txtTotalHaber.Text = haberDif.ToString("C0");

                                                                diferenciaDif = debeDif - haberDif;
                                                            }
                                                            else
                                                                {
                                                                    debeDif = sumaDebe;
                                                                    txtTotalDebe.Text = debeDif.ToString("C0");

                                                                    haberDif = sumaHaber;
                                                                    txtTotalHaber.Text = haberDif.ToString("C0");

                                                                    diferenciaDif = debeDif - haberDif;
                                                                }

                                                            double di = sumaDebe - sumaHaber;
                                                            difRed = di;

                                                            if (diferenciaDif == 0)
                                                            {
                                                                txtDiferenciaDH.Text = "Comprobante OK";
                                                                cOK = true;
                                                            }
                                                            else
                                                                { txtDiferenciaDH.Text = diferenciaDif.ToString("C0");}
                                                        }
                                                    }//diferencia
                                                    if (cOK != true)
                                                    {
                                                        if (difRed > 0 && difRed < 10)
                                                        {
                                                            sumaDebe = 0;
                                                            sumaHaber = 0;

                                                            if (difRed == 0)
                                                            {
                                                                txtDiferenciaDH.Text = "Comprobante OK";
                                                            }
                                                            else
                                                            {
                                                                if (difRed < 0)
                                                                {
                                                                    txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                    ok(difRed);
                                                                }
                                                                else
                                                                {
                                                                    if (difRed > 0)
                                                                    {
                                                                        txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                        ok(difRed);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (difRed < 0 && difRed > -10)
                                                            {
                                                                sumaDebe = 0;
                                                                sumaHaber = 0;

                                                                if (difRed == 0)
                                                                {
                                                                    txtDiferenciaDH.Text = "Comprobante OK";
                                                                }
                                                                else
                                                                {
                                                                    if (difRed < 0)
                                                                    {
                                                                        txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                        ok(difRed);
                                                                    }
                                                                    else
                                                                        {
                                                                            if (difRed > 0)
                                                                            {
                                                                                txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                                ok(difRed);
                                                                            }
                                                                        }
                                                                }
                                                            }
                                                            else
                                                                {
                                                                    if (difRed < 0 && difRed < -10 || difRed > 0 && difRed > 10)
                                                                    {
                                                                        sumaDebe = 0;
                                                                        sumaHaber = 0;

                                                                        if (difRed == 0)
                                                                        {
                                                                            txtDiferenciaDH.Text = "Comprobante OK";
                                                                        }
                                                                        else
                                                                            {
                                                                                if (difRed < 0)
                                                                                {
                                                                                    txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                                    ok(difRed);
                                                                                }
                                                                                else
                                                                                    {
                                                                                        if (difRed > 0)
                                                                                        {
                                                                                            txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                                            ok(difRed);
                                                                                        }
                                                                                    }
                                                                            }
                                                                    }
                                                                }
                                                        }
                                                    }

                                                } // fin cuadro pregunta diferencia redondeo
                                            }
                                            else
                                                {
                                                    if (difRed < 0 && difRed > -10 || difRed > 0 && difRed > 10)
                                                    {
                                                        sumaDebe = 0;
                                                        sumaHaber = 0;

                                                        if (difRed == 0)
                                                        {
                                                            txtDiferenciaDH.Text = "Comprobante OK";
                                                        }
                                                        else
                                                            {
                                                                if (difRed < 0)
                                                                {
                                                                    txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                    ok(difRed);
                                                                }
                                                                else
                                                                    {
                                                                        if (difRed > 0)
                                                                        {
                                                                            txtDiferenciaDH.Text = difRed.ToString("C0");
                                                                            ok(difRed);
                                                                        }
                                                                    }
                                                            }
                                                    }
                                                    else
                                                        {
                                                            if (diferenciaDif < -10)
                                                            {
                                                                th = 6;
                                                                String fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                                int local = Convert.ToInt32(txtLocal.Text);
                                                                CODCUENTA = "1-1-012-055"; 
                                                                NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                            
                                                                int debeN = Convert.ToInt32(diferenciaDif) * -1;
                                                                int haberN = 0;
                                                                String cc = CC;
                                                                String rut = "";
                                                                String doc = "";
                                                                String tipo = "";
                                                                String glosa = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                                                Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                                                lista.Add(list);
                                                                limpiarV();

                                                                dgvDatosVaucher.DataSource = null;
                                                                dgvDatosVaucher.DataSource = lista;
                                                                dgvDatosVaucher.Columns[11].Visible = false;

                                                                sumaDebe = 0;
                                                                sumaHaber = 0;

                                                                foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                                {
                                                                    sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                                    sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                                }

                                                                double debe1 = sumaDebe;
                                                                txtTotalDebe.Text = debe1.ToString("C0");

                                                                double haber1 = sumaHaber;
                                                                txtTotalHaber.Text = haber1.ToString("C0");

                                                                double di = sumaDebe - sumaHaber;

                                                                if (di == 0)
                                                                {
                                                                    txtDiferenciaDH.Text = "Comprobante OK";
                                                                }
                                                            }
                                                            else
                                                                {
                                                                    th = 6;
                                                                    String fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                                                    int local = Convert.ToInt32(txtLocal.Text);
                                                                    CODCUENTA = "1-1-012-055"; 
                                                                    NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                      
                                                                    int debeN = 0;
                                                                    int haberN = Convert.ToInt32(diferenciaDif);
                                                                    String cc = CC;
                                                                    String rut = "";
                                                                    String doc = "";
                                                                    String tipo = "";
                                                                    String glosa = "ARQUEO LOCAL " + Local + " " + fechaArc;

                                                                    Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, CC, debeN, haberN, rut, tipo, doc, glosa, th);
                                                                    lista.Add(list);
                                                                    limpiarV();

                                                                    dgvDatosVaucher.DataSource = null;
                                                                    dgvDatosVaucher.DataSource = lista;
                                                                    dgvDatosVaucher.Columns[11].Visible = false;

                                                                    sumaDebe = 0;
                                                                    sumaHaber = 0;

                                                                    foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                                                                    {
                                                                        sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                                                        sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                                                                    }

                                                                    double debe1 = sumaDebe;
                                                                    txtTotalDebe.Text = debe1.ToString("C0");

                                                                    double haber1 = sumaHaber;
                                                                    txtTotalHaber.Text = haber1.ToString("C0");

                                                                    double di = sumaDebe - sumaHaber;

                                                                    if (di == 0)
                                                                    {
                                                                        txtDiferenciaDH.Text = "Comprobante OK";
                                                                    }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                    }
                    catch (OdbcException oex)
                        {
                            MessageBox.Show("Error al cargar datos en grila, " +oex.ToString());
                        }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar llenado datagridview, " + ex.ToString());
                    }
                    //............... fin try-catch dgv-voucher..............................................................................................................
                    //............... fin llenado datagridview y textbox.....................................................................................................
                }
            }
            catch (OdbcException oex)
                {
                    MessageBox.Show("Error ODBC AQ-CP-EC-GF: " + oex.ToString());
                }
            catch (Exception ex)
            {
                MessageBox.Show("Error Final grilla: " + ex.ToString());
            }
            //............... fin try-catch datos AQ-CP-EC-GF...........................................................................................................
        }

        public void crearT(String dia, String mes, String año, String local)            //Metodo crear archivo XX 
        {
            String fechaARC, XX;
            int Local;

            fechaARC = dia + "/" + mes + "/" + año;

            Local = Convert.ToInt32(local);

            XX = "XX" + año.Remove(0, 2) + mes + dia;
            String ruta = "D:\\Cierres\\" + año + mes + dia + "\\" + local + "\\" + XX + "." + local;
            //valor de una celda a[columna,fila].Values.ToString()
            DataGridView a = dgvDatosVaucher;

            //esribir archivo txt
            FileStream stream = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);

            bool meddica = false;
            String vacio = " ";


            if (Local.Equals(2) || Local.Equals(16) || Local.Equals(201) || Local.Equals(202) || Local.Equals(203) || Local.Equals(204) || Local.Equals(205) || Local.Equals(206))
            { meddica = true;}
            else
                { meddica = false;}

            DataTable dtX = new DataTable();
            if (okL)        //limpia dataTable dtX
            {
                dtX.Clear();
            }

            dtX.Columns.Add("FECHAPRO");
            dtX.Columns.Add("LOCAL");
            dtX.Columns.Add("CODCUENTA");
            dtX.Columns.Add("NOMBRECTA");
            dtX.Columns.Add("CC");
            dtX.Columns.Add("DEBE");
            dtX.Columns.Add("HABER");
            dtX.Columns.Add("RUT");
            dtX.Columns.Add("TIPOD");
            dtX.Columns.Add("DOCUMENTO");
            dtX.Columns.Add("GLOSA");
            dtX.Columns.Add("th");

            foreach (DataGridViewRow rowGrid in a.Rows)
            {
                DataRow row = dtX.NewRow();
                row["FECHAPRO"] = Convert.ToString(rowGrid.Cells[0].Value);
                row["LOCAL"] = Convert.ToString(rowGrid.Cells[1].Value);
                row["CODCUENTA"] = Convert.ToString(rowGrid.Cells[2].Value);
                row["NOMBRECTA"] = Convert.ToString(rowGrid.Cells[3].Value);
                row["CC"] = Convert.ToString(rowGrid.Cells[4].Value);
                row["DEBE"] = Convert.ToString(rowGrid.Cells[5].Value);
                row["HABER"] = Convert.ToString(rowGrid.Cells[6].Value);
                row["RUT"] = Convert.ToString(rowGrid.Cells[7].Value);
                row["TIPOD"] = Convert.ToString(rowGrid.Cells[8].Value);
                row["DOCUMENTO"] = Convert.ToString(rowGrid.Cells[9].Value);
                row["GLOSA"] = Convert.ToString(rowGrid.Cells[10].Value);
                row["th"] = Convert.ToString(rowGrid.Cells[11].Value);

                dtX.Rows.Add(row);
            }
            int i = 0;
            foreach (DataRow row in dtX.Rows)
            {
                if (Convert.ToInt32(row[5]) > 0 || Convert.ToInt32(row[6]) > 0) //(Convert.ToInt32(row[5]) > 0 || Convert.ToInt32(row[6]) > 0)
                {
                    //if (meddica == true)
                    //{

                    //meddica = true;

                    String linea;
                    String tsap = Convert.ToString(row[11]);        //tipo asiento
                    String tipo1 = Convert.ToString(row[8]);
                    String debe = Convert.ToString(row[5]);
                    String haber = Convert.ToString(row[6]);
                    String glosa = Convert.ToString(row[10]);
                    String rut = Convert.ToString(row[7]);
                    //String rut1 = "\"" + vacio + "\"";    modificado 10.01.2018 dnavarrete

                    if (String.IsNullOrEmpty(rut))
                    { rut = ""; }                                  //"\"" + vacio + "\""; modificado 10.01.2018 dnavarrete                    
                    else
                        { rut = row[7].ToString();}                //Convert.ToString(row[7]); modificado 10.01.2018 dnavarrete                        

                    if (String.IsNullOrEmpty(tipo1))
                    { tipo1 = ""; }                                //"\"" + vacio + "\""; modificado 10.01.2018 dnavarrete                    
                    else
                        { tipo1 = Convert.ToString(row[8]);}                        

                    String cc = row[4].ToString();                 //"\"" + Convert.ToString(row[4]) + "\"";   modificado 10.01.2018 dnavarrete
                    String docum = row[9].ToString();

                    if (String.IsNullOrEmpty(docum))
                    {
                        // docum = "\"" + glosa + "\"";
                        docum = Convert.ToString(0);
                    }
                    else
                        { docum = row[9].ToString();}              //"\"" + Convert.ToString(row[9]) + "\""; modificado 10.01.2018 dnavarrete
                        

                    //..........................................................manejo de fechas y meses............................................................................................................
                    String vMes = mes;
                    String fpp = dia + mes + año;

                    if (Convert.ToInt32(dia) == 28 || Convert.ToInt32(dia) == 29 && Convert.ToInt32(mes) == 02)     //mes febrero normal o bisiesto
                    {
                        fpp = 01 + 03 + año;
                    }
                    else
                    {
                        if (Convert.ToInt32(dia) == 30)
                        {
                            if (Convert.ToInt32(mes) == 04 || Convert.ToInt32(mes) == 06 || Convert.ToInt32(mes) == 09 || Convert.ToInt32(mes) == 11)
                            {
                                fpp = 01 + 0 + Convert.ToString(Convert.ToInt32(mes) + 1) + año;
                            }
                        }
                        else
                            {
                                if (Convert.ToInt32(dia) == 31)
                                {
                                    if (Convert.ToInt32(mes) == 01 || Convert.ToInt32(mes) == 03 || Convert.ToInt32(mes) == 05 || Convert.ToInt32(mes) == 07 || Convert.ToInt32(mes) == 08 || Convert.ToInt32(mes) == 10 || Convert.ToInt32(mes) == 12)
                                    {
                                        if (Convert.ToInt32(mes) == 12)
                                        {
                                            fpp = 01 + 01 + año + 1;
                                        }
                                        else
                                            {
                                                fpp = 01 + 0 + Convert.ToString(Convert.ToInt32(mes) + 1) + año;
                                            }
                                    }
                                }
                                else
                                    {
                                        if (Convert.ToInt32(dia) == 10 || Convert.ToInt32(dia) == 20 || Convert.ToInt32(dia) == 30)
                                        {
                                            fpp = Convert.ToString(Convert.ToInt32(dia) + 1) + mes + año;
                                        }
                                        else
                                            {
                                                int fpp1 = Convert.ToInt32(dia) + 1;

                                                if (fpp1 == 10 || fpp1 == 20 || fpp1 == 30)
                                                {
                                                    fpp = Convert.ToString(fpp1) + mes + año; ;
                                                }
                                                else
                                                    {
                                                        if (fpp1 == 1 || fpp1 == 2 || fpp1 == 3 || fpp1 == 4 || fpp1 == 5 || fpp1 == 6 || fpp1 == 7 || fpp1 == 8 || fpp1 == 9)
                                                        {
                                                            fpp = 0 + Convert.ToString(Convert.ToInt32(dia) + 1) + mes + año;
                                                        }
                                                        else
                                                            {
                                                                fpp = Convert.ToString(Convert.ToInt32(dia) + 1) + mes + año;
                                                            }
                                                    }

                                            }
                                    }
                            }
                    }
                    //fechas para caso tipo...........................................................................................................................................................   

                    String tipoArq, tipoArqSap,cuentaCod, vDebe, vHaer, cGlosa, cCC, col17, col18, cRut,cRut2, documento, col20, col21, col22, col23, col24, col25, col27, col28, col29, col36, debeAnt, CreditAnt;

                    tipoArq = tsap;
                    cuentaCod = row[2].ToString();          //"\"" + Convert.ToString(row[2]) + "\"";   modificado 0.01.2018 dnavarrete
                    vDebe = debe;
                    vHaer = haber;
                    cGlosa = glosa;                         //"\"" + glosa + "\"";  modificado 0.01.2018 dnavarrete
                    cCC = cc;
                    documento = String.Empty;
                    col17 = String.Empty;
                    col18 = String.Empty;
                    cRut = String.Empty;
                    cRut2 = String.Empty;
                    col20 = String.Empty;
                    col21 = String.Empty;
                    col22 = String.Empty;
                    col23 = String.Empty;
                    col24 = String.Empty;
                    col25 = String.Empty;
                    col27 = String.Empty;
                    col28 = String.Empty;
                    col29 = String.Empty;
                    col36 = String.Empty;
                    debeAnt = String.Empty;
                    CreditAnt = String.Empty;

                    if (tipoArq == "4")
                    { cRut = ncRUT; }
                    else
                        { cRut = rut;}                        
                    
                    documento = ncDocumento; 

                    String cuenta = cuentaCod;                 //Convert.ToString(row[2]); modificado 0.01.2018 dnavarrete

                    //switch cuentas ...................................................................................................  
                    //Caso 1............................................................................................................
                    switch (cuenta)
                    {
                        case "1-1-041-001":
                            if (Convert.ToInt32(haber) > 0)              //debe...................................................
                            {
                                cCC = cc;

                                if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                {
                                    rut = rut.Replace(".", "");
                                    //rut = rut.Substring(0, rut.Length - 1);
                                    cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete

                                }
                                else
                                    {
                                        rut = rut.Replace(".", "");
                                        if (rut.StartsWith("0"))
                                        {
                                            rut.Substring(1);
                                            //rut = rut.Substring(1, rut.Length - 2);
                                            cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                        }
                                    }

                                if (tipo1 != "FD")
                                {
                                    col20 = "\"" + "NA" + "\"";
                                    col21 = docum;
                                }
                                else
                                    {
                                        col20 = "\"" + "DP" + "\"";
                                        col21 = folio;
                                    }
                                
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";

                                if (tipo1 != "FD")
                                {
                                    col24 = "\"" + "NA" + "\"";
                                }
                                else
                                    {
                                        col24 = "\"" + "FD" + "\"";
                                    }

                                col25 = docum;  // "\"" + docum + "\"";                                    
                            }
                            else
                                {     // haber .................................................................
                                    if (Convert.ToInt32(debe) > 0)
                                    {
                                        cCC = cc;

                                        if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                        {
                                            rut = rut.Replace(".", "");
                                            //rut = rut.Substring(0, rut.Length - 1);
                                            cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete

                                        }
                                        else
                                            {
                                                rut = rut.Replace(".", "");
                                                if (rut.StartsWith("0"))
                                                {
                                                    rut.Substring(1);
                                                    //rut = rut.Substring(0, rut.Length - 1);
                                                    cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                                }
                                            }

                                        col20 = "\"" + "FD" + "\"";
                                        col21 = docum;
                                        col22 = "\"" + fechaARC + "\"";
                                        col23 = "\"" + fechaARC + "\"";
                                        col24 = "\"" + "FD" + "\"";
                                        col25 = docum;
                                    }
                                }
                            break;
                        //Caso 2...........................................................................
                        case "1-1-041-002":
                            if (Convert.ToInt32(debe) > 0)                      //debe..........................................
                            {
                                cCC = cc;

                                if (tipo1 == "NA")
                                {
                                    if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                    {
                                        rut = rut.Replace(".", "");
                                        //rut = rut.Substring(0, rut.Length - 1);
                                        cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                        cRut2 = cRut;
                                    }
                                    else
                                        {
                                            rut = rut.Replace(".", "");
                                            if (rut.StartsWith("0"))
                                            {
                                                rut.Substring(1);
                                                //rut = rut.Substring(1, rut.Length - 2);
                                                cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                                cRut2 = cRut;
                                            }
                                        }
                                }
                                else
                                    {
                                        if (meddica || tipo1 == "BM")
                                        {
                                            cRut = "99999999-6";      //"\"" + "99999999" + "\""; modificado 10.01.2018 dnavarrete
                                            cRut2 = cRut;
                                        }
                                        else
                                            {
                                                cRut = "66666666-6";      // "\"" + "66666666" + "\""; modificado 10.01.2018 dnavarrete
                                                cRut2 = cRut;
                                            }
                                    }

                                if (tipo1 != "NA")
                                {
                                    if (meddica)
                                    {
                                        col20 = "\"" + "BM" + "\"";
                                    }
                                    else
                                        {
                                            col20 = "BN";//"\"" + tipo1 + "\"";
                                        }
                                }
                                else
                                    {
                                        col20 = "\"" + "BN" + "\"";
                                    }
                                                     
                                col21 = folio + Local;
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";

                                if (tipo1 != "NA")
                                {
                                    if (meddica)
                                    {
                                        col24 = "\"" + "BM" + "\"";
                                    }
                                    else
                                        {
                                            col24 = "\"" + tipo1 + "\"";
                                        }
                                }
                                else
                                    {
                                        col24 = "\"" + tipo1 + "\"";
                                    }

                                if (tipo1 != "NA")
                                {
                                    col25 = folio + Local;
                                }
                                else
                                    {
                                        col25 = docum;
                                    }
                            }
                            else
                                {
                                    //haber.............................................
                                    if (Convert.ToInt32(haber) > 0)
                                    {
                                        cCC = cc;

                                        if (tipo1 == "NA")
                                        {
                                            if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                            {
                                                //rut = rut.Replace("-", "");
                                                rut = rut.Replace(".", "");
                                                //rut = rut.Substring(0, rut.Length - 1);
                                                cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                                cRut2 = cRut;
                                            }
                                            else
                                                {
                                                    rut = rut.Replace(".", "");

                                                    if (rut.StartsWith("0"))
                                                    {
                                                        rut.Substring(1);
                                                        //rut = rut.Substring(1, rut.Length - 2);
                                                        cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                                        cRut2 = cRut;
                                                    }
                                                }
                                        }
                                        else
                                            {
                                                if (meddica || tipo1 == "BM")
                                                {
                                                    cRut = "99999999-6";      //"\"" + "99999999" + "\""; modificado 10.01.2018 dnavarrete
                                                    cRut2 = cRut;
                                                }
                                                else
                                                    {
                                                        cRut = "66666666-6";      // "\"" + "66666666" + "\""; modificado 10.01.2018 dnavarrete
                                                        cRut2 = cRut;
                                                    }
                                            }

                                        if (tipo1 != "NA")
                                        {
                                            col20 = "BN";//"\"" + "DP" + "\"";
                                        }
                                        else
                                            {
                                                col20 = "\"" + "NA" + "\"";
                                            }

                                        if (tipo1 != "NA")
                                        {
                                            col21 = folio + Local;
                                        }
                                        else
                                            {
                                                col21 = docum;
                                            }

                                        col22 = "\"" + fechaARC + "\"";
                                        col23 = "\"" + fechaARC + "\"";

                                        if (tipo1 != "NA")
                                        {
                                            if (meddica || tipo1 == "BM")
                                            {
                                                col24 = "\"" + "BM" + "\"";
                                            }
                                            else
                                                {
                                                    col24 = "\"" + "BN" + "\"";
                                                }
                                        }
                                        else
                                            {
                                                col24 = "\"" + "NA" + "\"";
                                            }

                                        if (tipo1 != "NA")
                                        {
                                            col25 = folio + Local;
                                        }
                                        else
                                            {
                                                col25 = docum;
                                            }
                                    }
                                }
                            break;
                        //Caso 3...........................................................................
                        case "1-1-041-003":
                            if (Convert.ToInt32(debe) > 0)                      //debe.............................
                            {
                                cCC = cc;

                                if (meddica)
                                {
                                    cRut = "99999999-6";      //"\"" + "99999999" + "\""; modificado 10.01.2018 dnavarrete
                                }
                                else
                                {
                                    cRut = "66666666-6";      // "\"" + "66666666" + "\""; modificado 10.01.2018 dnavarrete
                                }

                                col20 = "\"" + "BO" + "\"";
                                col21 = folio + Local;
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";
                                col24 = "\"" + "BO" + "\"";
                                col25 = folio + Local;
                            }
                            else
                            {
                                if (Convert.ToInt32(haber) > 0)
                                {
                                    cCC = cc;

                                    if (meddica)
                                    {
                                        cRut = "99999999-6";      //"\"" + "99999999" + "\""; modificado 10.01.2018 dnavarrete
                                    }
                                    else
                                        {
                                            cRut = "66666666-6";      // "\"" + "66666666" + "\""; modificado 10.01.2018 dnavarrete
                                        }

                                    col20 = "\"" + "DP" + "\"";
                                    col21 = folio + Local;
                                    col22 = "\"" + fechaARC + "\"";
                                    col23 = "\"" + fechaARC + "\"";
                                    col24 = "\"" + "BO" + "\"";
                                    col25 = folio + Local;
                                }
                            }
                            break;
                        //Caso 4...........................................................................
                        case "1-1-041-004":
                            String d = txtMes.Text;
                            if (d.StartsWith("0"))
                            {
                                d = d.Substring(1);
                            }

                            if (Convert.ToInt32(debe) > 0)
                            {
                                if (txtLocal.Text == "661")
                                {
                                    cCC = cc;                                             //"\"" + vacio + "\"";
                                    col17 = "\"" + vacio + "\"";
                                }

                                if (rut == "\" \"")
                                {
                                    rut = "6.666.666-6";

                                    if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                    {
                                        rut = rut.Replace(".", "");
                                        //rut = rut.Substring(0, rut.Length - 1);
                                        cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                    }
                                    else
                                    {
                                        rut = rut.Replace(".", "");
                                        if (rut.StartsWith("0"))
                                        {
                                            rut.Substring(1);
                                            //rut = rut.Substring(1, rut.Length - 2);
                                            cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(rut.Substring(0, 1)) != 0)
                                    {
                                        rut = rut.Replace(".", "");
                                        //rut = rut.Substring(0, rut.Length - 1);
                                        cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                    }
                                    else
                                    {
                                        rut = rut.Replace(".", "");
                                        if (rut.StartsWith("0"))
                                        {
                                            rut.Substring(1);
                                            //rut = rut.Substring(1, rut.Length - 2);
                                            cRut = rut;                                 //"\"" + rut + "\""; modificado 10.01.2018 dnavarrete
                                        }
                                    }
                                }
                                col20 = "\"" + "CP" + "\"";

                                if (Local.ToString() == "661")
                                {
                                    col21 = d + txtAño.Text;                      //numero documento
                                }
                                else
                                    {
                                        col21 = "999";                      //numero documento
                                    }

                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";
                                col24 = "\"" + "CP" + "\"";

                                if (Local.ToString() == "661")
                                {
                                    col25 = d + txtAño.Text;                      //numero documento
                                }
                                else
                                {
                                    col25 = "999";                      //numero documento
                                }
                                //switch tipo.........
                            }
                            break;


                        default:
                            if (cuenta != "2-1-300-001")
                            {
                                cCC = cc;
                            }
                            break;
                    }

                    String tipos = Convert.ToString(row[8]);
                    switch (tipos)
                    {
                        case "EF":
                            if (Convert.ToInt32(debe) > 0)
                            {
                                col17 = "\"" + "DP" + "\"";
                                col18 = fpp;
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";
                            }
                            break;

                        case "CH":
                            if (Convert.ToInt32(debe) > 0)
                            {
                                col17 = "\"" + "DP" + "\"";
                                col18 = fpp;                            //fecha deposito
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";
                            }
                            break;

                        case "PI":
                            if (Convert.ToInt32(debe) > 0)
                            {
                                cCC = "\"" + vacio + "\"";
                                col17 = "\"" + "DP" + "\"";
                                col18 = fpp;
                                col22 = "\"" + fechaARC + "\"";
                                col23 = "\"" + fechaARC + "\"";
                            }
                            break;
                    }

                    //fin switch cuentas
                    //.......................................................................
                    //inicio detalle libro ...................................................................

                    switch (cuenta)
                    {
                        case "1-1-041-001":
                            if (tipo1 == "NA")
                            {
                                double colu27 = Convert.ToInt32(haber) - Math.Round((Convert.ToInt32(haber) * 0.19));
                                col27 = Convert.ToString(colu27);
                                double colu29 = Math.Round(Convert.ToInt32(haber) * 0.19);
                                col29 = Convert.ToString(colu29);
                                col36 = haber;
                            }
                            else
                            {
                                double colu27 = Convert.ToInt32(debe) - Math.Round((Convert.ToInt32(Convert.ToInt32(debe) / 1.19)) * 0.19);
                                col27 = Convert.ToString(colu27);
                                double colu29 = Math.Round(Convert.ToInt32(debe) - (Convert.ToInt32(debe) / 1.19));
                                col29 = Convert.ToString(colu29);
                                col36 = debe;
                            }
                            break;

                        case "1-1-041-002":
                            if (Convert.ToInt32(debe) > 0)
                            {
                                if (tipo1 != "NA")
                                {
                                    double colu27 = Math.Round(Convert.ToInt32(debe) / 1.19);
                                    col27 = Convert.ToString(colu27);
                                    double colu29 = Convert.ToInt32(debe) - Math.Round((Convert.ToInt32(debe) / 1.19));
                                    col29 = Convert.ToString(colu29);
                                    col36 = debe;
                                }
                                else
                                    {
                                        col27 = "0";
                                        col29 = "0";
                                        col36 = "0";
                                    }
                            }
                            if (Convert.ToInt32(haber) > 0)
                            {
                                if (tipo1 == "NA")
                                {
                                    double colu27 = Math.Round(Convert.ToInt32(haber) / 1.19);
                                    col27 = Convert.ToString(colu27);
                                    double colu29 = Convert.ToInt32(haber) - Math.Round((Convert.ToInt32(haber) / 1.19));
                                    col29 = Convert.ToString(colu29);
                                    col36 = haber;
                                }
                                else
                                    {
                                        col27 = "0";
                                        col29 = "0";
                                        col36 = "0";
                                    }
                            }
                            break;

                        case "1-1-041-003":
                            if (Convert.ToInt32(debe) > 0)
                            {
                                col28 = debe;
                                col36 = debe;
                            }
                            else
                                {
                                    col28 = "0";
                                    col36 = "0";
                                }
                            break;

                        default:
                            col28 = "0";
                            col36 = "0";
                            break;
                    }
                    if (tipoArq == "4")
                    {
                        cRut2 = dgvDatosVaucher.Rows[(i - 1)].Cells[7].Value.ToString().Replace(".","");
                        debeAnt = dgvDatosVaucher.Rows[(i - 1)].Cells[5].Value.ToString().Replace(".", "");
                        CreditAnt = dgvDatosVaucher.Rows[(i - 1)].Cells[6].Value.ToString().Replace(".", "");
                    }

                    linea = tipoArq + "," + cuentaCod + "," + vDebe + "," + vHaer + "," + cGlosa + "," + cCC + "," + col17 + "," + col18 + "," + cRut + "," + documento + "," + col20 + ",," + col21 + "," + col22 + ","+ 
                            col23 + "," + col24 + "," + col25 + "," + col27 + "," + col28 + "," + col29 + "," + col36 + "," + cRut2 + "," + debeAnt + "," + CreditAnt ;

                    sw.WriteLine(linea);    //crea y escribe archivo XX y salta una linea                                         
                }
                i++;
            }
            sw.Close(); //cierra archivo XX
            stream.Close();

        }
        
        public class Consultor                                                          //Clase estructura datos DGV 
        {
            public Consultor(String FECHA_PRO, int LOCAL, String CODCUENTA, String NOMBRECTA, String CC, int DEBE, int HABER, String RUT, String TIPOD, String DOCUMENTO, String GLOSA, byte tH)//, byte tH2)
            {
                this.FECHA_PRO = FECHA_PRO;
                this.LOCAL = LOCAL;
                this.CODCUENTA = CODCUENTA;
                this.NOMBRECTA = NOMBRECTA;
                this.CC = CC;
                this.DEBE = DEBE;
                this.HABER = HABER;
                this.RUT = RUT;
                this.TIPOD = TIPOD;
                this.DOCUMENTO = DOCUMENTO;
                this.GLOSA = GLOSA;
                this.tH = tH;
                //this.tH2 = tH2;
            }

            public String FECHA_PRO { get; set; }
            public int LOCAL { get; set; }
            public String CODCUENTA { get; set; }
            public String NOMBRECTA { get; set; }
            public String CC { get; set; }
            public int DEBE { get; set; }
            public int HABER { get; set; }
            public String RUT { get; set; }
            public String TIPOD { get; set; }
            public String DOCUMENTO { get; set; }
            public String GLOSA { get; set; }
            public byte tH { get; set; }
            //public byte tH2 { get; set; }

        }

        private void btnBuscar_Click(object sender, EventArgs e)                        //boton buscar 
        {
            String dia, mes, año, fechaProc1, fechaProc2, fechaAr, fNom;

            //obtiene los valores dia - mes - año
            dia = txtDia.Text;
            mes = txtMes.Text;
            año = txtAño.Text;

            if (dia == "")
            {
                MessageBox.Show("Favor ingresar día");
                txtDia.Focus();
            }
            else
                {
                    if (mes == "")
                    {
                        MessageBox.Show("Favor ingresar mes");
                        txtMes.Focus();
                    }
                    else
                        {
                            if (año == "")
                            {
                                MessageBox.Show("Favor ingresar año");
                                txtAño.Focus();
                            }
                            else
                                {
                                    if (txtLocal.Text == "")
                                    {
                                        MessageBox.Show("Favor ingresar numero de Local");
                                        txtLocal.Focus();
                                    }
                                    else
                                        {
                                            //formato fecha.................................................
                                            fechaProc1 = año + mes + dia;                                               //yyyyMMdd
                                            fechaProc2 = dia + "-" + mes + "-" + año;                                   //dd-MM-yyyy
                                            fechaAr = dia + "/" + mes + "/" + año;                                      //dd/MM/yyyy
                                            fNom = año.Remove(0, 2) + mes + dia;                                         //yyMMdd                                         

                                            int nLocal1 = Convert.ToInt32(txtLocal.Text);                               //obtiene numero de local                                            
                                                                                                                        //envia datos a metodo datosLocales
                                            datosLocales(fechaProc1, nLocal1, fNom);
                                        }
                                }
                        }
                }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)                       //limpiar DataGridView's y textbox's 
        {
            dgvDatosVaucher.DataSource = null;
            txtDiferenciaDH.Text = null;
            txtLocal.Text = null;
            txtTotalDebe.Text = null;
            txtTotalHaber.Text = null;
            txtDia.Text = null;
            txtMes.Text = null;
            txtAño.Text = null;
            txtDia.Focus();
            okL = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)                         //boton salir 
        {
            if (MessageBox.Show("¿Desea cerrar el programa?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
            { this.Close();}
        }

        private void btnGuardar_Click(object sender, EventArgs e)                       //boton guardar/crear archivo XX 
        {
            String dia, mes, año, fechaAR, fNom, localT;
            //int Local;
            //double dif = 0;
            List<Consultor> lista = new List<Consultor>();
            String gls = "Arqueo Local " + txtLocal.Text + " " + txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;

            if (dgvDatosVaucher.DataSource != null)
            {
                //Local = Convert.ToInt32(txtLocal.Text);
                localT = txtLocal.Text;
                dia = txtDia.Text;
                mes = txtMes.Text;
                año = txtAño.Text;

                fechaAR = dia + "/" + mes + "/" + año;
                fNom = año.Remove(0, 2) + mes + dia;

                String ruta = "D:\\Cierres\\" + año + mes + dia + "\\" + localT + "\\" + "XX" + fNom + "." + localT;        //crea ruta donde se guardara archivo XX                

                if (txtDiferenciaDH.Text == "Comprobante OK")                                                               //coprueba que comprobante/arqueo este ok (diferencia debe - haber = 0)
                {
                    crearT(dia, mes, año, localT);                                                                          //llama metodo crearT para crear archivo XX
                    MessageBox.Show("Archivo XX" + fNom + "." + localT + " ha sido creado exitosamente");
                    //VO();

                    if (MessageBox.Show("¿Desea abrir el archivo?", "Abrir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", ruta);
                        Process p = Process.Start(psi);
                    }
                } else
                    { MessageBox.Show("No se pudo crear Archivo XX" + fNom + "." + localT + ", por diferencia entre Debe y Haber, verifique datos");}
            }
            else
                { MessageBox.Show("No se puede crear archivo XX sin antes realizar revisión de archivos de arqueo");}
        }

        private void txt1_TextChanged(object sender, EventArgs e)                       //focus de textbox dia a textbox mes 
        {
            String a1 = txtDia.Text;
            if (a1.Length == 2)
            {
                txtMes.Focus();
            }
        }

        private void txt2_TextChanged(object sender, EventArgs e)                       //focus de textbox mes a textbox año 
        {
            String a2 = txtMes.Text;
            if (a2.Length == 2)
            {
                txtAño.Focus();
            }
        }

        private void txt3_TextChanged(object sender, EventArgs e)                       //focus de textbox año a textbox numero de local 
        {
            String a3 = txtAño.Text;
            if (a3.Length == 4)
            {
                txtLocal.Focus();
            }
        }

        private void txtLocal_TextChanged(object sender, EventArgs e)                   //maximo de 3 caracteres en textbox numero de local 
        {
            txtLocal.MaxLength = 3;
        }

        private void txtLocal_KeyPress(object sender, KeyPressEventArgs e)              //valida solo numeros y enter en textbox numero local 
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                btnBuscar.PerformClick();
            }
            else
            {
                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
                {
                    MessageBox.Show("Solo puede ingresar numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Handled = true;
                    return;
                }
            }
        }

        private void txtDia_KeyPress(object sender, KeyPressEventArgs e)                //Solo numeros en textbox dia 
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo puede ingresar numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtMes_KeyPress(object sender, KeyPressEventArgs e)                //Solo numeros en textbox mes 
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo puede ingresar numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtAño_KeyPress(object sender, KeyPressEventArgs e)                //Solo numeros en textbox año 
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo puede ingresar numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void sALIRToolStripMenuItem_Click(object sender, EventArgs e)                    
        {
            if (MessageBox.Show("¿Desea cerrar el programa?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
            {
                this.Close();
            }
        }

        private void dgvDatosVaucher_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double sumaDebe = 0;
            double sumaHaber = 0;

            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
            {
                sumaDebe += Convert.ToInt32(row.Cells[5].Value);        
                sumaHaber += Convert.ToInt32(row.Cells[6].Value);       
            }
            
            dgvDatosVaucher.Columns[11].Visible = false;

            double debe = sumaDebe;
            txtTotalDebe.Text = debe.ToString("C0");

            double haber = sumaHaber;
            txtTotalHaber.Text = haber.ToString("C0");

            double diferencia = sumaDebe - sumaHaber;

            if (diferencia == 0)
            {
                txtDiferenciaDH.Text = "Comprobante OK";
            }
            else
                {
                    if (diferencia < 0)
                    {
                        txtDiferenciaDH.Text = diferencia.ToString("C0");
                        ok(diferencia);
                    }
                    else
                        {
                            if (diferencia > 0)
                            {
                                txtDiferenciaDH.Text = diferencia.ToString("C0");
                                ok(diferencia);
                            }
                        }
                }
        }

        public void ok(double dif)                                                               
        {
            String gls = "ARQUEO LOCAL " + txtLocal.Text + " " + txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String CC1 = txtLocal.Text;

            if (CC1.Length == 3)
            {
                CC1 = "01-0" + txtLocal.Text;
            }
            else if (CC1.Length == 2)
                {
                    CC1 = "01-00" + txtLocal.Text;
                }
                else if (CC1.Length == 1)
                    {
                        CC1 = "01-000" + txtLocal.Text;
                    }

            DataTable dt = new DataTable();
            dt.Columns.Add("FECHAPRO");
            dt.Columns.Add("LOCAL");
            dt.Columns.Add("CODCUENTA");
            dt.Columns.Add("NOMBRECTA");
            dt.Columns.Add("CC");
            dt.Columns.Add("DEBE");
            dt.Columns.Add("HABER");
            dt.Columns.Add("RUT");
            dt.Columns.Add("TIPOD");
            dt.Columns.Add("DOCUMENTO");
            dt.Columns.Add("GLOSA");
            dt.Columns.Add("tH");

            DataGridView a = dgvDatosVaucher;

            foreach (DataGridViewRow rowGrid in a.Rows)
            {
                DataRow row = dt.NewRow();
                row["FECHAPRO"] = Convert.ToString(rowGrid.Cells[0].Value);
                row["LOCAL"] = Convert.ToString(rowGrid.Cells[1].Value);
                row["CODCUENTA"] = Convert.ToString(rowGrid.Cells[2].Value);
                row["NOMBRECTA"] = Convert.ToString(rowGrid.Cells[3].Value);
                row["CC"] = Convert.ToString(rowGrid.Cells[4].Value);
                row["DEBE"] = Convert.ToString(rowGrid.Cells[5].Value);
                row["HABER"] = Convert.ToString(rowGrid.Cells[6].Value);
                row["RUT"] = Convert.ToString(rowGrid.Cells[7].Value);
                row["TIPOD"] = Convert.ToString(rowGrid.Cells[8].Value);
                row["DOCUMENTO"] = Convert.ToString(rowGrid.Cells[9].Value);
                row["GLOSA"] = Convert.ToString(rowGrid.Cells[10].Value);
                row["tH"] = Convert.ToString(rowGrid.Cells[11].Value);

                dt.Rows.Add(row);
            }

            double sumaDebe = 0;
            double sumaHaber = 0;
            double debeDif = 0;
            double haberDif = 0;
            double diferenciaDif = 0;

            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
            {
                sumaDebe += Convert.ToInt32(row.Cells[5].Value);        
                sumaHaber += Convert.ToInt32(row.Cells[6].Value);      
            }

            dgvDatosVaucher.Columns[11].Visible = false;

            if (okL)
            {
                debeDif = sumaDebe;
                txtTotalDebe.Text = debeDif.ToString("C0");

                haberDif = sumaHaber;
                txtTotalHaber.Text = haberDif.ToString("C0");

                diferenciaDif = debeDif - haberDif;
            }
            else
                {
                    debeDif = sumaDebe;
                    txtTotalDebe.Text = debeDif.ToString("C0");

                    haberDif = sumaHaber;
                    txtTotalHaber.Text = haberDif.ToString("C0");

                    diferenciaDif = sumaDebe - sumaHaber;
                }

            if (diferenciaDif == 0)
            {
                txtDiferenciaDH.Text = "Comprobante OK";
            }
            else
                {
                    double sumaDebeAg = 0;
                    double sumaHaberAg = 0;

                    if (dif != 0 && dif < 10 && dif > -10)
                    {
                        if (MessageBox.Show("¿Desea Agregar Diferencia por Recaudación?", "Direfencia Recaudación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                        {
                            String fecha, cc, rut, doc, tipo, glosa;
                            int local, debeN, haberN;

                            if (dif > 0 && dif < 10)
                            {
                                fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                local = Convert.ToInt32(txtLocal.Text);
                                String CODCUENTA = "1-1-012-055"; 
                                String NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                           
                                debeN = 0;
                                haberN = Convert.ToInt32(dif);
                                cc = CC1;
                                rut = "";
                                doc = "";
                                tipo = "";
                                glosa = gls;
                                byte th = 6;

                                DataRow row = dt.NewRow();
                                row["FECHAPRO"] = fecha;
                                row["LOCAL"] = local;
                                row["CODCUENTA"] = CODCUENTA;
                                row["NOMBRECTA"] = NOMBRECTA;
                                row["CC"] = cc;
                                row["DEBE"] = debeN;
                                row["HABER"] = haberN;
                                row["RUT"] = rut;
                                row["TIPOD"] = tipo;
                                row["DOCUMENTO"] = doc;
                                row["GLOSA"] = glosa;
                                row["tH"] = th;

                                dt.Rows.Add(row);

                                Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                dgvDatosVaucher.DataSource = dt;

                                lista.Add(list);
                                dgvDatosVaucher.DataSource = null;
                                dgvDatosVaucher.DataSource = lista;
                                dgvDatosVaucher.Columns[11].Visible = false;

                                foreach (DataGridViewRow row1 in dgvDatosVaucher.Rows)
                                {
                                    sumaDebe += Convert.ToInt32(row1.Cells[5].Value);        
                                    sumaHaber += Convert.ToInt32(row1.Cells[6].Value);      
                                }

                                if (okL)
                                {
                                    debeDif = sumaDebeAg;
                                    txtTotalDebe.Text = debeDif.ToString("C0");

                                    haberDif = sumaHaberAg;
                                    txtTotalHaber.Text = haberDif.ToString("C0");

                                    diferenciaDif = debeDif - haberDif;
                                }
                                else
                                    {
                                        debeDif = sumaDebeAg;
                                        txtTotalDebe.Text = debeDif.ToString("C0");

                                        haberDif = sumaHaberAg;
                                        txtTotalHaber.Text = haberDif.ToString("C0");

                                        diferenciaDif = debeDif - haberDif;
                                    }
                           
                                if (diferenciaDif == 0)
                                {
                                    txtDiferenciaDH.Text = "Comprobante OK";
                                }
                                else
                                    {
                                        if (diferenciaDif < 0)
                                        {
                                            txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                        }
                                        else
                                            {
                                                if (diferenciaDif > 0)
                                                {
                                                    txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                }
                                            }
                                    }
                                sumaDebeAg = 0;
                                sumaHaberAg = 0;
                            }
                            else
                                {
                                    if (dif < 0 && dif > -10)
                                    {
                                        fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                        local = Convert.ToInt32(txtLocal.Text);
                                        String CODCUENTA = "1-1-012-055"; 
                                        String NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                       
                                        debeN = Convert.ToInt32(dif) * -1;
                                        haberN = 0;
                                        cc = CC1;
                                        rut = "";
                                        doc = "";
                                        tipo = "";
                                        glosa = gls;
                                        byte th = 6;

                                        DataRow row = dt.NewRow();

                                        row["FECHAPRO"] = fecha;
                                        row["LOCAL"] = local;
                                        row["CODCUENTA"] = CODCUENTA;
                                        row["NOMBRECTA"] = NOMBRECTA;
                                        row["CC"] = cc;
                                        row["DEBE"] = debeN;
                                        row["HABER"] = haberN;
                                        row["RUT"] = rut;
                                        row["TIPOD"] = tipo;
                                        row["DOCUMENTO"] = doc;
                                        row["GLOSA"] = glosa;
                                        row["tH"] = th;

                                        dt.Rows.Add(row);

                                        Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                        dgvDatosVaucher.DataSource = dt;

                                        lista.Add(list);
                                        dgvDatosVaucher.DataSource = null;
                                        dgvDatosVaucher.DataSource = lista;
                                        dgvDatosVaucher.Columns[11].Visible = false;

                                        foreach (DataGridViewRow row1 in dgvDatosVaucher.Rows)
                                        {
                                            sumaDebe += Convert.ToInt32(row1.Cells[5].Value);        
                                            sumaHaber += Convert.ToInt32(row1.Cells[6].Value);      
                                        }

                                        if (okL)
                                        {
                                            debeDif = sumaDebeAg;
                                            txtTotalDebe.Text = debeDif.ToString("C0");

                                            haberDif = sumaHaberAg;
                                            txtTotalHaber.Text = haberDif.ToString("C0");

                                            diferenciaDif = debeDif - haberDif;
                                        }
                                        else
                                            {
                                                debeDif = sumaDebeAg;
                                                txtTotalDebe.Text = debeDif.ToString("C0");

                                                haberDif = sumaHaberAg;
                                                txtTotalHaber.Text = haberDif.ToString("C0");

                                                diferenciaDif = debeDif - haberDif;
                                            }
                                        if (diferenciaDif == 0)
                                        {
                                            txtDiferenciaDH.Text = "Comprobante OK";
                                        }
                                        else
                                            {
                                                if (diferenciaDif < 0)
                                                {
                                                    txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                }
                                                else
                                                    {
                                                        if (diferenciaDif > 0)
                                                        {
                                                            txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                        }
                                                    }
                                            }
                                        sumaDebeAg = 0;
                                        sumaHaberAg = 0;
                                    }
                                }
                        }
                    }
                    else
                        {
                            if (difRed < 0 && difRed < -10 || difRed > 0 && difRed > 10)
                            {
                                if (MessageBox.Show("¿Desea Agregar Diferencia por Recaudación?", "Direfencia Recaudación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                                {
                                    if (difRed < 0)
                                    {
                                        String fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                        int local = Convert.ToInt32(txtLocal.Text);
                                        String CODCUENTA = "1-1-012-055";
                                        String NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                       
                                        int debeN = Convert.ToInt32(dif) * -1;
                                        int haberN = 0;
                                        String cc = CC1;
                                        String rut = "";
                                        String doc = "";
                                        String tipo = "";
                                        String glosa = gls;
                                        byte th = 6;

                                        DataRow row = dt.NewRow();

                                        row["FECHAPRO"] = fecha;
                                        row["LOCAL"] = local;
                                        row["CODCUENTA"] = CODCUENTA;
                                        row["NOMBRECTA"] = NOMBRECTA;
                                        row["CC"] = cc;
                                        row["DEBE"] = debeN;
                                        row["HABER"] = haberN;
                                        row["RUT"] = rut;
                                        row["TIPOD"] = tipo;
                                        row["DOCUMENTO"] = doc;
                                        row["GLOSA"] = glosa;
                                        row["tH"] = th;

                                        dt.Rows.Add(row);

                                        Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                        dgvDatosVaucher.DataSource = dt;

                                        lista.Add(list);
                                        dgvDatosVaucher.DataSource = null;
                                        dgvDatosVaucher.DataSource = lista;
                                        dgvDatosVaucher.Columns[11].Visible = false;

                                        foreach (DataGridViewRow row1 in dgvDatosVaucher.Rows)
                                        {
                                            sumaDebe += Convert.ToInt32(row1.Cells[5].Value);       
                                            sumaHaber += Convert.ToInt32(row1.Cells[6].Value);      
                                        }

                                        if (okL)
                                        {
                                            debeDif = sumaDebeAg;
                                            txtTotalDebe.Text = debeDif.ToString("C0");

                                            haberDif = sumaHaberAg;
                                            txtTotalHaber.Text = haberDif.ToString("C0");

                                            diferenciaDif = debeDif - haberDif;
                                        }
                                        else
                                            {
                                                debeDif = sumaDebeAg;
                                                txtTotalDebe.Text = debeDif.ToString("C0");

                                                haberDif = sumaHaberAg;
                                                txtTotalHaber.Text = haberDif.ToString("C0");

                                                diferenciaDif = debeDif - haberDif;
                                            }
                                        if (diferenciaDif == 0)
                                        {
                                            txtDiferenciaDH.Text = "Comprobante OK";
                                        }
                                        else
                                            {
                                                if (diferenciaDif < 0)
                                                {
                                                    txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                }
                                                else
                                                    {
                                                        if (diferenciaDif > 0)
                                                        {
                                                            txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                        }
                                                    }
                                            }
                                        sumaDebeAg = 0;
                                        sumaHaberAg = 0;
                                    }
                                    else
                                        {
                                            String fecha = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                            int local = Convert.ToInt32(txtLocal.Text);
                                            String CODCUENTA = "1-1-012-055"; 
                                            String NOMBRECTA = "DIFERENCIA POR RECAUDACION";                                       
                                            int debeN = 0;
                                            int haberN = Convert.ToInt32(dif);
                                            String cc = CC1;
                                            String rut = "";
                                            String doc = "";
                                            String tipo = "";
                                            String glosa = gls;
                                            byte th = 6;

                                            DataRow row = dt.NewRow();

                                            row["FECHAPRO"] = fecha;
                                            row["LOCAL"] = local;
                                            row["CODCUENTA"] = CODCUENTA;
                                            row["NOMBRECTA"] = NOMBRECTA;
                                            row["CC"] = cc;
                                            row["DEBE"] = debeN;
                                            row["HABER"] = haberN;
                                            row["RUT"] = rut;
                                            row["TIPOD"] = tipo;
                                            row["DOCUMENTO"] = doc;
                                            row["GLOSA"] = glosa;
                                            row["tH"] = th;

                                            dt.Rows.Add(row);

                                            Consultor list = new Consultor(fecha, local, CODCUENTA, NOMBRECTA, cc, debeN, haberN, rut, tipo, doc, glosa, th);
                                            dgvDatosVaucher.DataSource = dt;

                                            lista.Add(list);
                                            dgvDatosVaucher.DataSource = null;
                                            dgvDatosVaucher.DataSource = lista;
                                            dgvDatosVaucher.Columns[11].Visible = false;

                                            foreach (DataGridViewRow row1 in dgvDatosVaucher.Rows)
                                            {
                                                sumaDebe += Convert.ToInt32(row1.Cells[5].Value);        
                                                sumaHaber += Convert.ToInt32(row1.Cells[6].Value);       
                                            }

                                            if (okL)
                                            {
                                                debeDif = sumaDebeAg;
                                                txtTotalDebe.Text = debeDif.ToString("C0");

                                                haberDif = sumaHaberAg;
                                                txtTotalHaber.Text = haberDif.ToString("C0");

                                                diferenciaDif = debeDif - haberDif;
                                            }
                                            else
                                                {
                                                    debeDif = sumaDebeAg;
                                                    txtTotalDebe.Text = debeDif.ToString("C0");

                                                    haberDif = sumaHaberAg;
                                                    txtTotalHaber.Text = haberDif.ToString("C0");

                                                    diferenciaDif = debeDif - haberDif;
                                                }
                                            if (diferenciaDif == 0)
                                            {
                                                txtDiferenciaDH.Text = "Comprobante OK";
                                            }
                                            else
                                                {
                                                    if (diferenciaDif < 0)
                                                    {
                                                        txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                    }
                                                    else
                                                        {
                                                            if (diferenciaDif > 0)
                                                            {
                                                                txtDiferenciaDH.Text = diferenciaDif.ToString("C0");
                                                            }
                                                        }
                                                }
                                            sumaDebeAg = 0;
                                            sumaHaberAg = 0;
                                        }
                                }
                            }
                        }
                }
        }

        private void acercaDeVoucherToolStripMenuItem_Click(object sender, EventArgs e)          
        {
            Acerca a = new Acerca();
            a.ShowDialog();
        }
        
        //private void btnImprimir_Click(object sender, EventArgs e)                               
        //{
            //Report r = new Report();
            //r.ShowDialog();
        //}

        //............Codigo SAP-Hana..........................................................................................................................

        public bool boleta(DataTable bL)                                                         
        { //borrador boleta 
            bool ok = true;
            int registros = 0;
            registros = bL.Rows.Count;                 //numero de registro (filas) en archivo txt
            int conteoB = 0;                           //contador de ciclos            

            String dia = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String referencia2 = "L" + txtLocal.Text + " - " + txtDia.Text + txtMes.Text + txtAño.Text;
            String empres = empresa(Convert.ToInt32(txtLocal.Text));
            
            //escribir archivo texto
            String ruta = "Cuentas Faltantes Local "+ txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";

            StreamWriter swr; //= new StreamWriter(streamAr);

            if (File.Exists(rutaF))
            {
                File.Delete(rutaF); //File.AppendText(rutaF);//swr = new StreamWriter(streamAr.ToString(), append: true);
                swr = File.CreateText(rutaF);
            }
            else
                { swr = File.CreateText(rutaF); }//new StreamWriter(streamAr);
                
            
            foreach (DataRow cuentaB in bL.Rows)
            {
                String cuentaPro = cuentaB["cuenta"].ToString().Replace("-", "");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"Segment_0\", T0.\"AcctName\" FROM " + empres + ".\"OACT\" T0 WHERE T0.\"Segment_0\" = '" + cuentaPro + "'";
                    HanaCommand hcmd = new HanaCommand(cmd, con);
                    HanaDataReader hdr = hcmd.ExecuteReader();

                    hdr.Read();

                    a1 = hdr.GetValue(0).ToString();
                    a2 = hdr.GetValue(1).ToString();

                    con.Close();

                    if (a1 != "" || a1 != " " || a1 != null)
                    {
                        hdr = null;    //dt.Clear();
                        a1 = String.Empty;
                        a2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Numero cuenta: " + cuentaPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar cuenta: " + cuentaPro);
                    MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            swr.Close();

            FileInfo fi = new FileInfo(rutaF);
            StreamReader sr = fi.OpenText();

            string s = "";
            while ((s = sr.ReadLine()) != null)
            { ok = false;}

            sr.Close();

            if (ok)
            {
                if (conExxis())
                {
                    //---------------------variables borrador boleta---------------------------------------------------------------
                    Documents boleta = SBO.GetBusinessObject(BoObjectTypes.oDrafts);      //borrador
                    boleta.DocObjectCode = BoObjectTypes.oInvoices;                       //borrador tipo documento factura
                    boleta.DocumentSubType = BoDocumentSubType.bod_Bill;                  //boleta            
                    boleta.DocType = BoDocumentTypes.dDocument_Service;                   //factura tipo servicio
                    boleta.Lines.TaxCode = "IVA";                                         //Si la boleta es con IVA

                    boleta.CardCode = "99999999C";                                        //codigo cliente con boleta
                    //---fechas------------------------------------------
                    boleta.DocDate = Convert.ToDateTime(dia);                             //fecha de contabilizacion
                    boleta.DocDueDate = Convert.ToDateTime(dia);                          //fecha de vencimiento
                    boleta.TaxDate = Convert.ToDateTime(dia);                             //fecha de documento
                    //---fechas------------------------------------------
                    boleta.Comments = "ARQUEO LOCAL " + txtLocal.Text + " " + dia;        //comentario/glosa del arqueo
                    boleta.NumAtCard = referencia2;
                    //---------------------variables borrador boleta---------------------------------------------------------------

                    foreach (DataRow linea in bL.Rows)
                    {
                        conteoB++;

                        ChartOfAccounts cha;                                       //variable usada para plan de cuenta
                        String res, codigoH = String.Empty;

                        String cuentaPro = linea["cuenta"].ToString().Replace("\"", "").Replace("-", "");
                        int debePro = Convert.ToInt32(linea["debe"]);
                        int haberPro = Convert.ToInt32(linea["haber"]);
                        String glosaPro = Convert.ToString(linea["glosa"]);

                        try
                        {
                            //---------------se obtiene cuenta formato _SYS0000-------------
                            Recordset cuentaH;
                            res = String.Empty;
                            SBObob c;
                            cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                            c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                            cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                            cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);     //codigo cuenta
                            //----------------------------------------------------------------
                            res = cuentaH.Fields.Item(0).Value;
                            boleta.Lines.ItemDescription = glosaPro;                              //descripcion del servicio

                            if (registros == conteoB)
                            {
                                if (cha.GetByKey(res))
                                {
                                    boleta.Lines.AccountCode = res;
                                    cuentaH = null;
                                }
                                else
                                    {
                                        MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                        break;
                                    }

                                if (debePro == 0 && haberPro > 0)
                                { boleta.Lines.LineTotal = haberPro; }           //precio/total boleta haber                                
                                else if (debePro > 0 && haberPro == 0)
                                        { boleta.Lines.LineTotal = debePro; }    //precio/total boleta debe   

                                errSAPNum = boleta.Add();

                                if (!errSAPNum.Equals(0))
                                {
                                    SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                    MessageBox.Show("Error Codigo: " + errSAPNum + ", " + errSAPDesc);
                                }
                                else
                                    { ok = true;}
                            }
                            else
                                {
                                    boleta.Lines.ItemDescription = glosaPro;                            //descripcion del servicio

                                    if (cha.GetByKey(res))
                                    {
                                        boleta.Lines.AccountCode = res;                                 //codigo cuenta
                                        cuentaH = null;
                                    }
                                    else
                                        {
                                            MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                            break;
                                        }

                                    if (debePro == 0 && haberPro > 0)
                                    { boleta.Lines.LineTotal = haberPro; }                //precio/total boleta haber   
                                    else if (debePro > 0 && haberPro == 0)
                                            { boleta.Lines.LineTotal = debePro; }         //precio/total boleta debe       

                                    boleta.Lines.Add();
                                }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error al ingresar boleta, Error: " + e);
                            descExxis();
                        }
                    }
                    descExxis();
                }else
                    { ok = false;}
            }
            return ok;
        }

        public bool boletaEX(DataTable bLX)                                                      
        { //borrador boleta Exenta
            bool ok = true;
            int registros = 0;
            registros = bLX.Rows.Count;                 //numero de registro (filas) en archivo txt
            int conteoBX = 0;                           //contador de ciclos
            //bool sinCuenta = true;

            String dia = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String referencia2 = "L" + txtLocal.Text + " - " + txtDia.Text + txtMes.Text + txtAño.Text;
            String empres = empresa(Convert.ToInt32(txtLocal.Text));
            
            //escribir archivo texto
            String ruta = "Cuentas Faltantes Local " + txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";
            StreamWriter swr;

            if (File.Exists(rutaF))
            {
                swr = File.AppendText(rutaF);//swr = new StreamWriter(streamAr.ToString(), append: true);
            }
            else
                { swr = File.CreateText(rutaF); }//new StreamWriter(streamAr);                

            foreach (DataRow cuentaBX in bLX.Rows)
            {
                String cuentaPro = cuentaBX["cuenta"].ToString().Replace("-", "");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"Segment_0\", T0.\"AcctName\" FROM " + empres + ".\"OACT\" T0 WHERE T0.\"Segment_0\" = '" + cuentaPro + "'";
                    HanaCommand hcmd = new HanaCommand(cmd, con);
                    HanaDataReader hdr = hcmd.ExecuteReader();
                    hdr.Read();
                    a1 = hdr.GetValue(0).ToString();
                    a2 = hdr.GetValue(1).ToString();
                    con.Close();

                    if (a1 != "" || a1 != " " || a1 != null)
                    {
                        hdr = null;    //dt.Clear();
                        a1 = String.Empty;
                        a2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Numero cuenta: " + cuentaPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar cuenta: " + cuentaPro);
                    MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            swr.Close();

            FileInfo fi = new FileInfo(rutaF);
            StreamReader sr = fi.OpenText();

            string s = "";
            while ((s = sr.ReadLine()) != null)
            { ok = false;}

            sr.Close();
            
            if (ok)
            {
                if (conExxis())
                {
                    //---------------------variables borrador boleta exenta---------------------------------------------------------------
                    Documents boleta = SBO.GetBusinessObject(BoObjectTypes.oDrafts);      //borrador
                    boleta.DocObjectCode = BoObjectTypes.oInvoices;                       //borrador tipo documento factura
                    boleta.DocumentSubType = BoDocumentSubType.bod_ExemptBill;            //boleta exenta           
                    boleta.DocType = BoDocumentTypes.dDocument_Service;                   //factura tipo servicio
                    boleta.Lines.TaxCode = "IVA_Exe";                                     //Si la boleta es exenta de iva

                    boleta.CardCode = "99999998C";                                        //codigo cliente
                    //---fechas------------------------------------------
                    boleta.DocDate = Convert.ToDateTime(dia);                             //fecha de contabilizacion
                    boleta.DocDueDate = Convert.ToDateTime(dia);                          //fecha de vencimiento
                    boleta.TaxDate = Convert.ToDateTime(dia);                             //fecha de documento
                    //---fechas------------------------------------------
                    boleta.Comments = "ARQUEO LOCAL " + txtLocal.Text + " " + dia;              //comentario/glosa del arqueo
                    boleta.NumAtCard = referencia2;
                    //---------------------variables borrador boleta exenta---------------------------------------------------------------

                    foreach (DataRow linea in bLX.Rows)
                    {
                        conteoBX++;

                        ChartOfAccounts cha;                                                                //variable usada para plan de cuenta
                        String res, codigoH = String.Empty;

                        String cuentaPro = linea["cuenta"].ToString().Replace("\"", "").Replace("-", "");   //codigo de cuenta
                        int debePro = Convert.ToInt32(linea["debe"]);                                       //valor debe
                        int haberPro = Convert.ToInt32(linea["haber"]);                                     //valor haber
                        String glosaPro = Convert.ToString(linea["glosa"]);                                 //glosa del arqueo (cuenta)

                        try
                        {
                            //---------------se obtiene cuenta formato _SYS0000-------------
                            Recordset cuentaH;
                            res = String.Empty;
                            SBObob c;
                            cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                            c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                            cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                            cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);     //codigo cuenta                                                                                                                                                             
                            //----------------------------------------------------------------
                            res = cuentaH.Fields.Item(0).Value;

                            boleta.Lines.ItemDescription = glosaPro;                              //descripcion del servicio

                            if (registros == conteoBX)
                            {
                                if (cha.GetByKey(res))
                                {
                                    boleta.Lines.AccountCode = res;
                                    cuentaH = null;
                                }
                                else
                                    {
                                        MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                        break;
                                    }

                                if (debePro == 0 && haberPro > 0)
                                { boleta.Lines.LineTotal = haberPro; }        //precio/total boleta haber
                                else if (debePro > 0 && haberPro == 0)
                                        { boleta.Lines.LineTotal = debePro; } //precio/total boleta debe                                    

                                errSAPNum = boleta.Add();

                                if (!errSAPNum.Equals(0))
                                {
                                    SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                    MessageBox.Show("Error, " + errSAPDesc);
                                }
                                else
                                    { ok = true;}
                            }
                            else
                                {
                                    boleta.Lines.ItemDescription = glosaPro;                            //descripcion del servicio

                                    if (cha.GetByKey(res))
                                    {
                                        boleta.Lines.AccountCode = res;                                 //codigo cuenta
                                        cuentaH = null;
                                    }
                                    else
                                        {
                                            MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                            break;
                                        }

                                    if (debePro == 0 && haberPro > 0)
                                    { boleta.Lines.LineTotal = haberPro; }       //precio/total boleta haber                                    
                                    else if (debePro > 0 && haberPro == 0)
                                            { boleta.Lines.LineTotal = debePro; }       //precio/total boleta debe                                        

                                    boleta.Lines.Add();
                                }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error al ingresar boleta, Error: " + e);
                            descExxis();
                        }
                    }
                    descExxis();
                }else
                    { ok = false;}
            }
            return ok;     
        }

        public bool factura(DataTable fT)                                                        
        { //borrador factura  
            bool ok = true;
            int registros = 0;
            registros = fT.Rows.Count;                                                          //numero de registro (filas) en archivo txt
            int conteoF = 0;                                                                    //contador de ciclos           

            String dia = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String res = String.Empty;                                                      //gustada valor cuenta SYS
            String referencia2 = "L" + txtLocal.Text + " - " + txtDia.Text + txtMes.Text + txtAño.Text;
            String empres = empresa(Convert.ToInt32(txtLocal.Text));

            //escribir archivo texto
            String ruta = "Cuentas Faltantes Local " + txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";
            StreamWriter swr;

            if (File.Exists(rutaF))
            {
                swr = File.AppendText(rutaF);//swr = new StreamWriter(streamAr.ToString(), append: true);
            }
            else
                { swr = File.CreateText(rutaF); }//new StreamWriter(streamAr);                

            foreach (DataRow cuentaF in fT.Rows)
            {
                String cuentaPro = cuentaF["cuenta"].ToString().Replace("\"", "").Replace("-", "");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"Segment_0\", T0.\"AcctName\" FROM " + empres + ".\"OACT\" T0 WHERE T0.\"Segment_0\" = '" + cuentaPro + "'";
                    HanaCommand hcmd1 = new HanaCommand(cmd, con);
                    HanaDataReader hdr1 = hcmd1.ExecuteReader();
                    hdr1.Read();
                    a1 = hdr1.GetValue(0).ToString();
                    a2 = hdr1.GetValue(1).ToString();
                    con.Close();

                    if (a1 != "" || a1 != " " || a1 != null)
                    {
                        hdr1 = null;    //dt.Clear();
                        a1 = String.Empty;
                        a2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Numero cuenta: " + cuentaPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar cuenta: " + cuentaPro);
                    MessageBox.Show(""+e.ToString()+"", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    con.Close();
                }

            }
                
            foreach (DataRow rutF in fT.Rows)
            {
                String rutPro = rutF["rut"].ToString().Replace(".","");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"CardCode\", T0.\"LicTradNum\" FROM " + empres + ".\"OCRD\" T0 WHERE T0.\"LicTradNum\" = '"+ rutPro + "'";
                    HanaCommand hcmd2 = new HanaCommand(cmd, con);
                    HanaDataReader hdr2 = hcmd2.ExecuteReader();
                    hdr2.Read();
                    b1 = hdr2.GetValue(0).ToString();
                    b2 = hdr2.GetValue(1).ToString();

                    con.Close();

                    if (b1 != "" || b1 != " " || b1 != null)
                    {
                        hdr2 = null;    //dt.Clear();
                        b2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Rut: " + rutPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar rut: " + rutPro);
                    MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }

            swr.Close();

            FileInfo fi = new FileInfo(rutaF);
            StreamReader sr = fi.OpenText();

            string s = "";
            while ((s = sr.ReadLine()) != null)
            { ok = false;}
            sr.Close();
            
            if (ok)
            {
                if (conExxis())
                {
                    //---------------------variables borrador factura---------------------------------------------------------------
                    Documents facturaX = SBO.GetBusinessObject(BoObjectTypes.oDrafts);      //borrador
                    facturaX.DocObjectCode = BoObjectTypes.oInvoices;                       //borrador tipo documento factura            
                    facturaX.DocType = BoDocumentTypes.dDocument_Service;                   //factura tipo servicio

                    facturaX.CardCode = b1;                                                 //codigo cliente                                                                                        
                    //---fechas------------------------------------------
                    facturaX.DocDate = Convert.ToDateTime(dia);                             //fecha de contabilizacion
                    facturaX.DocDueDate = Convert.ToDateTime(dia);                          //fecha de vencimiento
                    facturaX.TaxDate = Convert.ToDateTime(dia);                             //fecha de documento
                    //---fechas------------------------------------------
                    facturaX.Comments = "ARQUEO LOCAL " + txtLocal.Text + " " + dia;              //comentario/glosa del arqueo
                    facturaX.NumAtCard = referencia2;                                       //referenia
                    //--------------------------------------------------------------------------------------------------------------

                    try
                    {
                        foreach (DataRow linea in fT.Rows)
                        {
                            conteoF++;

                            String cuentaPro = linea["cuenta"].ToString().Replace("-", "");
                            int debePro = Convert.ToInt32(linea["debe"]);
                            int haberPro = Convert.ToInt32(linea["haber"]);
                            String glosaPro = Convert.ToString(linea["glosa"]);

                            //---------------se obtiene cuenta formato _SYS0000-------------
                            Recordset cuentaH;
                            SBObob c;
                            ChartOfAccounts cha;
                            cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                            c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                            cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                            cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);           //codigo cuenta
                            //----------------------------------------------------------------
                            res = cuentaH.Fields.Item(0).Value;                                     //guarda valor cuenta SYS 
                            if (registros == conteoF)                                               //valida ultimo registro
                            {
                                if (cha.GetByKey(res))
                                {
                                    facturaX.Lines.AccountCode = res;
                                }
                                else
                                    {
                                        MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                        break;
                                    }

                                facturaX.Lines.ItemDescription = glosaPro;                              //descripcion del servicio

                                if (debePro == 0 && haberPro > 0)
                                { facturaX.Lines.LineTotal = haberPro; }            //precio/total boleta haber                                
                                else if (debePro > 0 && haberPro == 0)
                                        { facturaX.Lines.LineTotal = debePro; }     //precio/total boleta debe                                    

                                errSAPNum = facturaX.Add();

                                if (!errSAPNum.Equals(0))
                                {
                                    SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                    MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                    ok = false;
                                }
                                else
                                    { ok = true;}
                                cuentaH = null;
                            }
                            else
                                {
                                    if (cha.GetByKey(res))
                                    {
                                        facturaX.Lines.AccountCode = res;
                                    }
                                    else
                                        {
                                            MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                            break;
                                        }

                                    facturaX.Lines.ItemDescription = glosaPro;          //descripcion del servicio

                                    if (debePro == 0 && haberPro > 0)
                                    { facturaX.Lines.LineTotal = haberPro; }            //precio/total boleta haber                                    
                                    else if (debePro > 0 && haberPro == 0)
                                            { facturaX.Lines.LineTotal = debePro; }       //precio/total boleta debe                                        

                                    errSAPNum = facturaX.Add();

                                    if (!errSAPNum.Equals(0))
                                    {
                                        SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                        MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                    }
                                    cuentaH = null;
                                }
                        }
                        descExxis();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error al buscar ingresar factura, Error: " + e);
                        descExxis();
                    }
                }
                else
                    { ok = false;}
            }
            return ok;
        }

        public bool notaCredito(DataTable nT)                                                    
        { //borrador notaCredito 
            bool ok = true;
            bool bncf = true;
            int registros = 0;
            registros = nT.Rows.Count;                                                          //numero de registro (filas) en archivo txt
            int conteoN = 0;                                                                    //contador de ciclos           

            String dia = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String res = String.Empty;                                                      //gustada valor cuenta SYS
            String referencia2 = "L" + txtLocal.Text + " - " + txtDia.Text + txtMes.Text + txtAño.Text;
            String empres = empresa(Convert.ToInt32(txtLocal.Text));

            //escribir archivo texto
            String ruta = "Cuentas Faltantes Local " + txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";
            StreamWriter swr;

            if (File.Exists(rutaF))
            {
                swr = File.AppendText(rutaF);//swr = new StreamWriter(streamAr.ToString(), append: true);
            }
            else
                { swr = File.CreateText(rutaF); }//new StreamWriter(streamAr);
                

            foreach (DataRow cuentaN in nT.Rows)
            {
                String cuentaPro = cuentaN["cuenta"].ToString().Replace("\"", "").Replace("-", "");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"Segment_0\", T0.\"AcctName\" FROM " + empres + ".\"OACT\" T0 WHERE T0.\"Segment_0\" = '" + cuentaPro + "'";
                    HanaCommand hcmd1 = new HanaCommand(cmd, con);
                    HanaDataReader hdr1 = hcmd1.ExecuteReader();
                    hdr1.Read();
                    a1 = hdr1.GetValue(0).ToString();
                    a2 = hdr1.GetValue(1).ToString();
                    con.Close();

                    if (a1 != "" || a1 != " " || a1 != null)
                    {
                        hdr1 = null;    //dt.Clear();
                        a1 = String.Empty;
                        a2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Numero cuenta: " + cuentaPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar cuenta: " + cuentaPro);
                    MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }

            }

            foreach (DataRow rutN in nT.Rows)
            {
                String rutPro = rutN["rut"].ToString().Replace(".", "");

                try
                {
                    con.Open();
                    String cmd = "SELECT T0.\"CardCode\", T0.\"LicTradNum\" FROM " + empres + ".\"OCRD\" T0 WHERE T0.\"LicTradNum\" = '" + rutPro + "'";
                    //String cmd = "SELECT T0.\"CardCode\", T1.\"LicTradNum\" FROM " + empres + ".\"ORIN\" T0  INNER JOIN " + empres + ".\"OCRD\" T1 ON T0.\"CardCode\" = T1.\"CardCode\" WHERE T1.\"LicTradNum\" = '" + rutPro + "'";
                    HanaCommand hcmd2 = new HanaCommand(cmd, con);
                    HanaDataReader hdr2 = hcmd2.ExecuteReader();
                    hdr2.Read();
                    b1 = hdr2.GetValue(0).ToString();
                    b2 = hdr2.GetValue(1).ToString();

                    con.Close();

                    if (b1 != "" || b1 != " " || b1 != null)
                    {
                        hdr2 = null;    //dt.Clear();
                        b2 = String.Empty;
                    }
                    else
                        { swr.WriteLine("Rut: " + rutPro);}
                }
                catch (Exception e)
                {
                    swr.WriteLine("Error al encontrar rut: " + rutPro);
                    MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }

            swr.Close();

            FileInfo fi = new FileInfo(rutaF);
            StreamReader sr = fi.OpenText();

            string s = "";
            while ((s = sr.ReadLine()) != null)
            { ok = false;}

            sr.Close();

            if (ok)
            {
                if (conExxis())
                {
                    //---------------------variables borrador factura---------------------------------------------------------------
                    Documents notaCredito = SBO.GetBusinessObject(BoObjectTypes.oDrafts);      //borrador
                    notaCredito.DocObjectCode = BoObjectTypes.oCreditNotes;                    //borrador tipo documento factura            
                    notaCredito.DocType = BoDocumentTypes.dDocument_Service;                   //factura tipo servicio

                    notaCredito.CardCode = b1;                                                 //codigo cliente                                                                                        
                    //---fechas------------------------------------------
                    notaCredito.DocDate = Convert.ToDateTime(dia);                             //fecha de contabilizacion
                    notaCredito.DocDueDate = Convert.ToDateTime(dia);                          //fecha de vencimiento
                    notaCredito.TaxDate = Convert.ToDateTime(dia);                             //fecha de documento
                    //---fechas------------------------------------------
                    notaCredito.Comments = "ARQUEO LOCAL " + txtLocal.Text + " " + dia;        //comentario/glosa del arqueo
                    notaCredito.NumAtCard = referencia2;                                       //referenia                                       
                    //--------------------------------------------------------------------------------------------------------------
                    try
                    {
                        foreach (DataRow linea in nT.Rows)
                        {
                            conteoN++;

                            String cuentaPro = linea["cuenta"].ToString().Replace("-", "");
                            int debePro = Convert.ToInt32(linea["debe"]);
                            int haberPro = Convert.ToInt32(linea["haber"]);
                            String glosaPro = Convert.ToString(linea["glosa"]);
                            int docNC = Convert.ToInt32(linea["documento"]);
                                                       
                            notaCredito.UserFields.Fields.Item("U_GLO_NUM_NC").Value = docNC;       //campo de usuario, guarda numero nota de credito
                                                        
                            //---------------se obtiene cuenta formato _SYS0000-------------
                            Recordset cuentaH;
                            SBObob c;
                            ChartOfAccounts cha;
                            cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                            c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                            cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                            cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);           //codigo cuenta
                            //----------------------------------------------------------------
                            res = cuentaH.Fields.Item(0).Value;                                     //guarda valor cuenta SYS 
                            if (registros == conteoN)                                               //valida ultimo registro
                            {
                                if (cha.GetByKey(res))
                                {
                                    notaCredito.Lines.AccountCode = res;
                                }
                                else
                                    {
                                        MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                        break;
                                    }

                                notaCredito.Lines.ItemDescription = glosaPro;                              //descripcion del servicio

                                if (debePro == 0 && haberPro > 0)
                                {notaCredito.Lines.LineTotal = haberPro;}               //precio/total boleta haber                                
                                else if (debePro > 0 && haberPro == 0)
                                        { notaCredito.Lines.LineTotal = debePro; }      //precio/total boleta debe
                                        

                                errSAPNum = notaCredito.Add();

                                if (!errSAPNum.Equals(0))
                                {
                                    SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                    MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                    ok = false;
                                }
                                else
                                    { ok = true;}

                                cuentaH = null;
                            }
                            else
                                {
                                    if (cha.GetByKey(res))
                                    {
                                        notaCredito.Lines.AccountCode = res;
                                    }
                                    else
                                        {
                                            MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                            break;
                                        }

                                    notaCredito.Lines.ItemDescription = glosaPro;          //descripcion del servicio

                                    if (debePro == 0 && haberPro > 0)
                                    { notaCredito.Lines.LineTotal = haberPro; }            //precio/total boleta haber                                    
                                    else if (debePro > 0 && haberPro == 0)
                                            { notaCredito.Lines.LineTotal = debePro; }     //precio/total boleta debe                                        

                                    errSAPNum = notaCredito.Add();

                                    if (!errSAPNum.Equals(0))
                                    {
                                        SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                        MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                    }
                                    cuentaH = null;
                                }
                        }
                        descExxis();
                    }
                    catch (Exception e)
                        {
                            MessageBox.Show("Error al buscar ingresar factura, Error: " + e);
                            descExxis();
                        }
                }
                else
                    { ok = false;}
            }
            return ok;
        }

        public bool asiento(DataTable tbAS)                                                      
        { //codigo para insertar asiento en un borrador
            bool ok = true;
            int registros = 0;
            registros = tbAS.Rows.Count;
            //numero de registro (filas) en archivo txt
            int conteoA = 0;                                                                      //contador de ciclos
            //int a;
            String nCuentaMala = String.Empty;
            String dia = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
            String comentario = "ARQUEO LOCAL " + txtLocal.Text + " " + dia;
            String referencia2 = "L" + txtLocal.Text + " - " + txtDia.Text + txtMes.Text + txtAño.Text;

            //escribir archivo texto
            String ruta = "Cuentas Faltantes Local " + txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";
            String empres = empresa(Convert.ToInt32(txtLocal.Text));

            StreamWriter swr;

            if (File.Exists(rutaF))
            { swr = File.AppendText(rutaF); }//swr = new StreamWriter(streamAr.ToString(), append: true);            
            else
                { swr = File.CreateText(rutaF); }//new StreamWriter(streamAr);
                

            foreach (DataRow cuentaA in tbAS.Rows)
            {
                String tip = cuentaA["tipo"].ToString();        //asiento

                if (tip == "4" || tip == "5" || tip == "6")
                {
                    String cuentaPro = cuentaA["cuenta"].ToString().Replace("-", "");

                    try
                    {
                        con.Open();
                        String cmd = "SELECT T0.\"Segment_0\", T0.\"AcctName\" FROM " + empres + ".\"OACT\" T0 WHERE T0.\"Segment_0\" = '" + cuentaPro + "'";
                        HanaCommand hcmd = new HanaCommand(cmd, con);
                        HanaDataReader hdr = hcmd.ExecuteReader();
                        hdr.Read();
                        a1 = hdr.GetValue(0).ToString();
                        a2 = hdr.GetValue(1).ToString();

                        con.Close();

                        if (a1 != "" || a1 != " " || a1 != null)
                        {
                            hdr = null;    //dt.Clear();
                            a1 = String.Empty;
                            a2 = String.Empty;
                        }
                        else
                            { swr.WriteLine("Numero cuenta: " + cuentaPro);}
                    }
                    catch (Exception e)
                    {
                        swr.WriteLine("Error al encontrar cuenta: " + cuentaPro);
                        MessageBox.Show("" + e.ToString() + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                    }
                }
            }
            swr.Close();

            FileInfo fi = new FileInfo(rutaF);
            StreamReader sr = fi.OpenText();

            string s = "";
            while ((s = sr.ReadLine()) != null)
            { ok = false;}
            sr.Close();

            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            if (ok)
            {
                if (conExxis())                                                                           //conexion hana para objetos
                {
                    JournalVouchers asiento = SBO.GetBusinessObject(BoObjectTypes.oJournalVouchers);      //debe estar dentro del periodo contable del sistema SB1
                                                                                                          //Documents asiento = SBO.GetBusinessObject(BoObjectTypes.oDrafts); //Documents asiento = SBO.GetBusinessObject(BoObjectTypes.oJournalEntries);
                                                                                                          //asiento.DocObjectCode = BoObjectTypes.oJournalEntries;
                    //---fechas------------------------------------------
                    asiento.JournalEntries.DueDate = DateTime.Parse(dia);                                 //fecha de vencimiento
                    asiento.JournalEntries.TaxDate = DateTime.Parse(dia);                                 //fecha de documento
                    asiento.JournalEntries.ReferenceDate = DateTime.Parse(dia);                           //fecha de contabilizacion
                    //---------------------------------------------------
                    asiento.JournalEntries.Memo = comentario;                                             //comentario asiento arqueo

                    foreach (DataRow linea in tbAS.Rows)
                    {
                        conteoA++;

                        String tipoEx = linea["tipo"].ToString();
                        String cuentaPro = linea["cuenta"].ToString().Replace("\"", "").Replace("-", "");
                        int debito = Convert.ToInt32(linea["debe"]);
                        int credito = Convert.ToInt32(linea["haber"]);
                        String glosaPro = Convert.ToString(linea["glosa"]);

                        if (registros == conteoA)
                        {
                            if (tipoEx == "6")
                            {
                                try
                                {
                                    ChartOfAccounts cha;                                                              //variable usada para plan de cuenta

                                    String res, codigoH = String.Empty;

                                    //---------------se obtiene cuenta formato _SYS0000-------------
                                    Recordset cuentaH;
                                    res = String.Empty;
                                    SBObob c;
                                    cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                                    c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                                    cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                                    cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);     //codigo cuenta                                                                                                                
                                    //---------------------------------------------------------------

                                    res = cuentaH.Fields.Item(0).Value;                                     //guarda valor cuenta SYS
                                    
                                    if (cha.GetByKey(res))
                                    {
                                        asiento.JournalEntries.Lines.AccountCode = res;
                                        cuentaH = null;
                                    }
                                    else
                                        {
                                            MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                            break;
                                        }

                                    asiento.JournalEntries.Lines.Credit = credito;                      //credito
                                    asiento.JournalEntries.Lines.Debit = debito;                        //debito

                                    errSAPNum = asiento.Add();

                                    if (!errSAPNum.Equals(0))
                                    {
                                        SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                        MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                    }                                    
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Error al crear asiento, Error: " + e);
                                    break;
                                }
                            } 
                            else if(tipoEx == "5")
                                    {
                                        try
                                        {
                                            String rutPro = Convert.ToString(linea["rut"]);
                                            String tipoC = Convert.ToString(linea["tipoC"].ToString().Replace("\"", "").Replace("-", ""));
                                            String tipoF = Convert.ToString(linea["tipoF"].ToString().Replace("\"", "").Replace("-", ""));

                                            if (tipoC == "BN")
                                            {//boleta
                                                cuentaPro = "99999999C";

                                                asiento.JournalEntries.Lines.ShortName = cuentaPro;

                                                asiento.JournalEntries.Reference2 = referencia2;
                                                asiento.JournalEntries.Lines.Credit = credito;                     
                                                asiento.JournalEntries.Lines.Debit = debito;                                                
                                        
                                                errSAPNum = asiento.Add();

                                                if (!errSAPNum.Equals(0))
                                                {
                                                    SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                                    MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                                }
                                            }                                        
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show("Error al crear asiento, Error: " + e);
                                            break;
                                        }                                
                                    }                                 
                                    else if (tipoEx == "4")
                                            {//nota de credito
                                                try
                                                {
                                                    String rutPro = Convert.ToString(linea["rut"]);
                                                    String tipoC = Convert.ToString(linea["tipoC"].ToString().Replace("\"", "").Replace("-", ""));
                                                    Double debitoAnt = Convert.ToDouble(linea["debeAnt"].ToString());
                                                    Double creditoAnt = Convert.ToDouble(linea["haberAnt"].ToString());

                                                    try
                                                    {
                                                        con.Open();
                                                        String cmd = "SELECT T0.\"CardCode\", T0.\"LicTradNum\" FROM " + empres + ".\"OCRD\" T0 WHERE T0.\"LicTradNum\" = '" + rutPro + "'";
                                                        HanaCommand hcmd2 = new HanaCommand(cmd, con);
                                                        HanaDataReader hdr2 = hcmd2.ExecuteReader();
                                                        hdr2.Read();
                                                        b1 = hdr2.GetValue(0).ToString();
                                                        b2 = hdr2.GetValue(1).ToString();

                                                        con.Close();

                                                        if (b1 != "" || b1 != " " || b1 != null)
                                                        {
                                                            hdr2 = null;    //dt.Clear();
                                                            b2 = String.Empty;
                                                        }
                                                    }
                                                    catch (Exception e)
                                                        { con.Close(); }

                                                    asiento.JournalEntries.Lines.ShortName = b1;

                                                    asiento.JournalEntries.Reference2 = referencia2;
                                                    asiento.JournalEntries.Lines.Credit = debitoAnt;
                                                    asiento.JournalEntries.Lines.Debit = creditoAnt;

                                                    errSAPNum = asiento.Add();

                                                    if (!errSAPNum.Equals(0))
                                                    {
                                                        SBO.GetLastError(out errSAPNum, out errSAPDesc);
                                                        MessageBox.Show("Error: " + errSAPDesc + " codigo: " + errSAPNum);
                                                    }
                                                }
                                                catch (Exception e)
                                                    {
                                                        MessageBox.Show("Error al crear asiento, Error: " + e);
                                                        break;
                                                    }
                                        }
                        }
                        else    //resgistro != conteoA
                            {
                                if (tipoEx == "6")
                                {
                                    try
                                    {
                                        ChartOfAccounts cha;                                                              //variable usada para plan de cuenta

                                        String res, codigoH = String.Empty;

                                        //---------------se obtiene cuenta formato _SYS0000-------------
                                        Recordset cuentaH;
                                        res = String.Empty;
                                        SBObob c;
                                        cha = SBO.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                                        c = SBO.GetBusinessObject(BoObjectTypes.BoBridge);
                                        cuentaH = SBO.GetBusinessObject(BoObjectTypes.BoRecordset);

                                        cuentaH = c.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", cuentaPro, BoQueryConditions.bqc_Equal);     //codigo cuenta                                                                                                                
                                        //---------------------------------------------------------------

                                        res = cuentaH.Fields.Item(0).Value;

                                        if (cha.GetByKey(res))
                                        {
                                            asiento.JournalEntries.Lines.AccountCode = res;
                                            cuentaH = null;
                                        }
                                        else
                                            {
                                                MessageBox.Show("error al buscar cuenta " + cuentaPro);
                                                break;
                                            }

                                        asiento.JournalEntries.Lines.Credit = credito;
                                        asiento.JournalEntries.Lines.Debit = debito;
                                        asiento.JournalEntries.Lines.Add();
                                        cuentaH = null;
                                    }
                                    catch (Exception e)
                                        {
                                            MessageBox.Show("Error al crear asiento, Error: " + e);
                                            break;
                                        }
                                }
                                if (tipoEx == "5")      
                                {
                                    try
                                    {
                                        String rutPro = Convert.ToString(linea["rut"]);
                                        String tipoC = Convert.ToString(linea["tipoC"].ToString().Replace("\"", "").Replace("-", ""));

                                        if (tipoC == "BN")
                                        {//boleta
                                            cuentaPro = "99999999C";

                                            asiento.JournalEntries.Lines.ShortName = cuentaPro;

                                            asiento.JournalEntries.Reference2 = referencia2;
                                            asiento.JournalEntries.Lines.Credit = credito;
                                            asiento.JournalEntries.Lines.Debit = debito;

                                            asiento.JournalEntries.Lines.Add();
                                        }
                                    }
                                    catch (Exception e)
                                        {
                                            MessageBox.Show("Error al crear asiento, Error: " + e);
                                            break;
                                        }
                                }
                                if (tipoEx == "2")
                                {//boleta exenta
                                 //if (cuentaPro == "11041003")
                                 //{
                                 //cuentaPro = "61010002";
                                    String rutPro = "99999998-9";
                                    try
                                    {
                                        try
                                        {
                                            con.Open();
                                            String cmd = "SELECT T0.\"CardCode\", T0.\"LicTradNum\" FROM " + empres + ".\"OCRD\" T0 WHERE T0.\"LicTradNum\" = '" + rutPro + "'";
                                            HanaCommand hcmd2 = new HanaCommand(cmd, con);
                                            HanaDataReader hdr2 = hcmd2.ExecuteReader();
                                            hdr2.Read();
                                            b1 = hdr2.GetValue(0).ToString();
                                            b2 = hdr2.GetValue(1).ToString();

                                            con.Close();

                                            if (b1 != "" || b1 != " " || b1 != null)
                                            {
                                                hdr2 = null;    //dt.Clear();
                                                b2 = String.Empty;
                                            }
                                        }
                                        catch (Exception e)
                                            { con.Close(); }

                                        asiento.JournalEntries.Lines.ShortName = b1;

                                        asiento.JournalEntries.Reference2 = referencia2;
                                        asiento.JournalEntries.Lines.Credit = credito;
                                        asiento.JournalEntries.Lines.Debit = debito;

                                        asiento.JournalEntries.Lines.Add();
                                    }
                                    catch (Exception e)
                                        {
                                            MessageBox.Show("Error al crear asiento, Error: " + e);
                                            break;
                                        }
                                }
                                if (tipoEx == "4")
                                {//nota de credito

                                    try
                                    {
                                        String rutPro = Convert.ToString(linea["rut"]);
                                        String tipoC = Convert.ToString(linea["tipoC"].ToString().Replace("\"", "").Replace("-", ""));
                                        Double debitoAnt = Convert.ToDouble(linea["debeAnt"].ToString());
                                        Double creditoAnt = Convert.ToDouble(linea["haberAnt"].ToString());

                                        try
                                        {
                                            con.Open();
                                            String cmd = "SELECT T0.\"CardCode\", T0.\"LicTradNum\" FROM " + empres + ".\"OCRD\" T0 WHERE T0.\"LicTradNum\" = '" + rutPro + "'";
                                            HanaCommand hcmd2 = new HanaCommand(cmd, con);
                                            HanaDataReader hdr2 = hcmd2.ExecuteReader();
                                            hdr2.Read();
                                            b1 = hdr2.GetValue(0).ToString();
                                            b2 = hdr2.GetValue(1).ToString();

                                            con.Close();

                                            if (b1 != "" || b1 != " " || b1 != null)
                                            {
                                                hdr2 = null;    //dt.Clear();
                                                b2 = String.Empty;
                                            }
                                        }
                                        catch (Exception e)
                                        { con.Close(); }

                                        asiento.JournalEntries.Lines.ShortName = b1;

                                        asiento.JournalEntries.Reference2 = referencia2;
                                        asiento.JournalEntries.Lines.Credit = debitoAnt;
                                        asiento.JournalEntries.Lines.Debit = creditoAnt;

                                        asiento.JournalEntries.Lines.Add();
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("Error al crear asiento, Error: " + e);
                                        break;
                                    }
                                }
                            }
                    }
                    descExxis();
                }
                else
                    { ok = false;}
            }
            return ok;
        }

        public void cuentas()
        { // DataTables para tipos de cuentas

            DataTable aS = null;
            aS = new DataTable();
            aS.Columns.Add("tipo");
            aS.Columns.Add("cuenta");
            aS.Columns.Add("debe");
            aS.Columns.Add("haber");
            aS.Columns.Add("glosa");
            aS.Columns.Add("Fecha");
            aS.Columns.Add("rut");
            aS.Columns.Add("documento");
            aS.Columns.Add("tipoC");
            aS.Columns.Add("tipoF");
            aS.Columns.Add("debeAnt");
            aS.Columns.Add("haberAnt");

            DataTable ext = null;
            ext = new DataTable();
            ext.Columns.Add("tipo");
            ext.Columns.Add("cuenta");
            ext.Columns.Add("debe");
            ext.Columns.Add("haber");
            ext.Columns.Add("glosa");
            ext.Columns.Add("Fecha");
            ext.Columns.Add("rut");
            ext.Columns.Add("documento");
            ext.Columns.Add("tipoC");
            ext.Columns.Add("tipoF");


            DataTable bL = null;
            bL = new DataTable();
            bL.Columns.Add("tipo");
            bL.Columns.Add("cuenta");
            bL.Columns.Add("debe");
            bL.Columns.Add("haber");
            bL.Columns.Add("glosa");
            bL.Columns.Add("Fecha");

            DataTable bLX = null;
            bLX = new DataTable();
            bLX.Columns.Add("tipo");
            bLX.Columns.Add("cuenta");
            bLX.Columns.Add("debe");
            bLX.Columns.Add("haber");
            bLX.Columns.Add("glosa");
            bLX.Columns.Add("Fecha");
            bLX.Columns.Add("rut");

            DataTable fT = null;
            fT = new DataTable();
            fT.Columns.Add("tipo");
            fT.Columns.Add("cuenta");
            fT.Columns.Add("debe");
            fT.Columns.Add("haber");
            fT.Columns.Add("glosa");
            fT.Columns.Add("Fecha");
            fT.Columns.Add("rut");

            DataTable nT = null;
            nT = new DataTable();
            nT.Columns.Add("tipo");
            nT.Columns.Add("cuenta");
            nT.Columns.Add("debe");
            nT.Columns.Add("haber");
            nT.Columns.Add("glosa");
            nT.Columns.Add("Fecha");
            nT.Columns.Add("rut");
            nT.Columns.Add("documento");

            String fNom = txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaXX = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\XX" + fNom + "." + txtLocal.Text;

            if (File.Exists(rutaXX))
            { sapB1(bL,bLX,aS,fT,nT);}  //metodo que procesa cuentas
            else
                { MessageBox.Show("Favor procesar arqueo y crear archivo XX antes de presionar boton \"Ingresar a SAP B1\" ","Información",MessageBoxButtons.OK,MessageBoxIcon.Information);}
        }
        
        private void button1_Click_1(object sender, EventArgs e)                                 
        {
            if (txtDia.Text != "" && txtMes.Text != "" && txtAño.Text != "")
            { cuentas();}
            else
                { MessageBox.Show("Favor ingresar fecha de arqueo","Información",MessageBoxButtons.OK,MessageBoxIcon.Information);} 
        }        
        
        //.....................................................................................................................................................

        private void limpiarV()
        {
            int th = 0;
            String  dFecha_pro = "";
            int nuLocal = 0;
            String CODCUENTA = "";                                  
            String NOMBRECTA = "";                                  
            int DEBE =0;
            int HABER = 0;
            String RUT = "";
            String TIPO = "";
            String DOCUMENTO = "";
            String GLOSA = "";
            String fecha = "";
            int local = 0;                                             
            int debeN = 0;
            int haberN = 0;
            String cc = "";
            String rut = "";
            String doc = "";
            String tipo = "";
            String glosa = "";

        }

        private void Vaucher_Load(object sender, EventArgs e)
        {

        }

        public String empresa(int Local)      
        { //selecciona empresa (Base de datos) a insertar segun local
            String BaseDatosEmp = String.Empty;

            if (Local.Equals(2) || Local.Equals(16) || Local.Equals(201) || Local.Equals(202) || Local.Equals(203) || Local.Equals(204) || Local.Equals(205) || Local.Equals(206))
            { BaseDatosEmp = "\"SBOPRUEBASGLOBAL\"";}
            else if (Local.Equals(651) || Local.Equals(664) || Local.Equals(673) || Local.Equals(687) || Local.Equals(695))
                    { BaseDatosEmp = "\"SBOPRUEBASGLOBAL\"";}
                else if (Local.Equals(568) || Local.Equals(632) || Local.Equals(648) || Local.Equals(655) || Local.Equals(672) || Local.Equals(681) || Local.Equals(693) || Local.Equals(923))
                        { BaseDatosEmp = "\"SBOPRUEBASGLOBAL\"";}
                    else if (Local.Equals(661))
                            { BaseDatosEmp = "\"SBOTEST_ALICIA\"";}
                        else if (Local.Equals(658))
                                { BaseDatosEmp = "\"SBOPRUEBASGLOBAL\"";}

            return BaseDatosEmp;
        }   

        public String bancosL()
        { //nombre cuenta bancaria que corresponde a locales

            String bancos =String.Empty;
            int local = Convert.ToInt32(txtLocal.Text);
            
            if (local == 201 || local == 202 || local == 567)
            { bancos = "BANCO DE CREDITO E INVERSIONES";}            
            else if(local == 681)
                    { bancos = "BANCO DEL ESTADO";}
                    else if (local == 203 || local == 204 || local == 205 || local == 568 || local == 629 || local == 632 || local == 648 || local == 651 || local == 655 || local == 658 || local == 661 || local == 664 || local == 672 || local == 673 || local == 687 || local == 693 || local == 695 || local == 923 || local == 936 || local == 16)
                            { bancos = "BANCO DE CHILE";}
            return bancos;
        }
        
        public String cuentaBL()
        { //codigo cuenta bancaria que corresponde a locales

            String cuenta = String.Empty;
            int local = Convert.ToInt32(txtLocal.Text);

            if (local == 201 || local == 202 || local == 567)
            { cuenta = "1-1-012-003"; }                         //BCi
            else if (local == 681)
                    { cuenta = "1-1-012-005"; }                 //BANCO ESTADO
                    else if (local == 203 || local == 204 || local == 205 || local == 568 || local == 629 || local == 632 || local == 648 || local == 651 || local == 655 || local == 658 || local == 661 || local == 664 || local == 672 || local == 673 || local == 687 || local == 693 || local == 695 || local == 923 || local == 936 || local == 16)
                            {cuenta = "1-1-012-001"; }          //BANCO DE CHILE
            
            return cuenta;
        }

        public void sapB1(DataTable bL, DataTable bLX, DataTable aS, DataTable fT, DataTable nT)
        { //procesa cuentas
            String fNom = txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaXX = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\XX" + fNom + "." + txtLocal.Text;

            //busca archivo txt-----------------------------
            StreamReader XX = new StreamReader(rutaXX);
            StringBuilder cadena = new StringBuilder();
            //----------------------------------------------
            String lineas = XX.ReadLine();

            String[] lin = File.ReadAllLines(rutaXX);

            int nLin = lin.Count();
            int tipo;  //asiento

            foreach (String linea in lin)
            {
                char[] delimitador = { ',' };
                String[] palabras = linea.Split(delimitador);

                tipo = Convert.ToInt32(palabras[0]);

                if (tipo == 1)                                                                     //clientes con boletas y ventas con boletas
                {
                    DataRow rowB = bL.NewRow();
                    rowB["tipo"] = Convert.ToInt32(palabras[0]);
                    rowB["cuenta"] = palabras[1];
                    rowB["debe"] = Convert.ToInt32(palabras[2]);
                    rowB["haber"] = Convert.ToInt32(palabras[3]);
                    rowB["glosa"] = palabras[4];
                    rowB["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                    bL.Rows.Add(rowB);
                }

                if (tipo == 2)                                                                      //boleta exenta y venta con boleta exenta
                {
                    DataRow rowBX = bLX.NewRow();
                    rowBX["tipo"] = Convert.ToInt32(palabras[0]);
                    rowBX["cuenta"] = palabras[1];
                    rowBX["debe"] = Convert.ToInt32(palabras[2]);
                    rowBX["haber"] = Convert.ToInt32(palabras[3]);
                    rowBX["glosa"] = palabras[4];
                    rowBX["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                    bLX.Rows.Add(rowBX);
                }

                if (tipo == 3)                                                                      //cliente con factura
                {
                    DataRow rowF = fT.NewRow();
                    rowF["tipo"] = Convert.ToInt32(palabras[0]);
                    rowF["cuenta"] = palabras[1];
                    rowF["debe"] = Convert.ToInt32(palabras[2]);
                    rowF["haber"] = Convert.ToInt32(palabras[3]);
                    rowF["glosa"] = palabras[4];
                    rowF["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                    rowF["rut"] = palabras[8];
                    fT.Rows.Add(rowF);
                }

                if (tipo == 4)                                                                     //nota de credito clientes
                {
                    DataRow rowNT = nT.NewRow();
                    rowNT["tipo"] = Convert.ToInt32(palabras[0]);
                    rowNT["cuenta"] = palabras[1];
                    rowNT["debe"] = Convert.ToInt32(palabras[2]);
                    rowNT["haber"] = Convert.ToInt32(palabras[3]);
                    rowNT["glosa"] = palabras[4];
                    rowNT["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                    if (palabras[8] != "")
                    { rowNT["rut"] = palabras[8];}
                    else
                        { rowNT["rut"] = palabras[20];}
                    //rowNT["rut"] = palabras[9];
                    rowNT["documento"] = palabras[9];
                    nT.Rows.Add(rowNT);
                }

                if (tipo == 6)                                                                      //asiento
                {
                    DataRow rowA = aS.NewRow();
                    rowA["tipo"] = Convert.ToInt32(palabras[0]);
                    rowA["cuenta"] = palabras[1];
                    rowA["debe"] = Convert.ToInt32(palabras[2]);
                    rowA["haber"] = Convert.ToInt32(palabras[3]);
                    rowA["glosa"] = palabras[4];
                    rowA["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                    rowA["rut"] = "";
                    rowA["documento"] = "";
                    rowA["tipoC"] = "";
                    rowA["tipoF"] = "";
                    aS.Rows.Add(rowA);

                }                
                else if (tipo == 4)
                        {
                            DataRow rowA = aS.NewRow();
                            rowA["tipo"] = Convert.ToInt32(palabras[0]);
                            rowA["cuenta"] = palabras[1];
                            rowA["debe"] = Convert.ToInt32(palabras[2]);
                            rowA["haber"] = Convert.ToInt32(palabras[3]);
                            rowA["glosa"] = palabras[4];
                            rowA["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                            if (palabras[8] != "")
                            { rowA["rut"] = palabras[8].Replace(".",""); }
                            else
                            { rowA["rut"] = palabras[21]; }
                            rowA["debeAnt"] = palabras[22];
                            rowA["haberAnt"] = palabras[23];
                            rowA["documento"] = Convert.ToInt32(palabras[9]); 
                            aS.Rows.Add(rowA);
                        }
                        else if (tipo == 5)
                                {
                                    DataRow rowA = aS.NewRow();
                                    rowA["tipo"] = Convert.ToInt32(palabras[0]);
                                    rowA["cuenta"] = palabras[1];
                                    rowA["debe"] = Convert.ToInt32(palabras[2]);
                                    rowA["haber"] = Convert.ToInt32(palabras[3]);
                                    rowA["glosa"] = palabras[4];
                                    rowA["Fecha"] = txtDia.Text + "/" + txtMes.Text + "/" + txtAño.Text;
                                    rowA["rut"] = palabras[8]; 
                                    rowA["documento"] = palabras[9];
                                    rowA["tipoC"] = palabras[10];
                                    //rowA["tipof"] = palabras[9];

                                    aS.Rows.Add(rowA);
                                }
        }

            bool bolet = false;
            bool boletaEx = false;
            bool fact = false;
            bool notaCred = false;
            bool asi = false;

            if (bL != null)
            {
                bolet = boleta(bL);
                if (!bolet)
                {
                    MessageBox.Show("Error al ingresar Boletas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //MessageBox.Show("Boletas ingresadas exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (bLX != null)
            {
                boletaEx = boletaEX(bLX);
                if (!boletaEx)
                {
                    MessageBox.Show("Error al ingresar Boletas Exentas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  //MessageBox.Show("Boletas Exentas ingresadas exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (fT != null)
            {
                fact = factura(fT);
                if (!fact)
                {
                    MessageBox.Show("Error al ingresar Facturas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //MessageBox.Show("Facturas ingresadas exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nT != null)
            {
                notaCred = notaCredito(nT);
                if (!notaCred)
                {
                    MessageBox.Show("Error al ingresar Nota de Credito", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //MessageBox.Show("Facturas ingresadas exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (aS != null)
            {
                asi = asiento(aS);
                if (!asi)
                {
                    MessageBox.Show("Error al ingresar Asientos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  //MessageBox.Show("Asientos ingresados exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            String ruta = "Cuentas Faltantes Local " + txtLocal.Text;    //"XX" + txtAño.Text.Remove(0, 2) + txtMes.Text + txtDia.Text;
            String rutaF = "D:\\Cierres\\" + txtAño.Text + txtMes.Text + txtDia.Text + "\\" + txtLocal.Text + "\\" + ruta + ".txt";

            if (File.Exists(rutaF))
            {
                StreamReader sr = new StreamReader(rutaF);

                List<String> listaCuentas = new List<string>();
                String lCuenta = String.Empty;

                do
                {
                    lCuenta = sr.ReadLine();
                    if (lCuenta != null)
                    {
                        if (!listaCuentas.Contains(lCuenta))
                        {
                            listaCuentas.Add(lCuenta);
                        }
                    }
                }
                while (lCuenta != null);

                sr.Close();

                StreamWriter wr = new StreamWriter(rutaF);
                foreach (String lcta in listaCuentas)
                {
                    wr.WriteLine(lcta);
                }
                wr.Close();

                FileInfo fi = new FileInfo(rutaF);
                StreamReader sr2 = fi.OpenText();

                string s = "";
                bool csap = false;
                while ((s = sr2.ReadLine()) != null)
                { csap = true; }
                
                sr2.Close();

                if (csap)
                {
                    if (MessageBox.Show("Se encontraron cuentas no creadas en SAP, ¿Desea ver cuentas?", "Abrir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == (System.Windows.Forms.DialogResult.Yes))
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", rutaF);
                        Process p = Process.Start(psi);
                    }
                }
                else
                    { MessageBox.Show("Cuentas ingresadas exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                
            }
        }

    }
}
