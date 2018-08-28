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


namespace Vaucher
{
    class archivoAQ
    {
        public class archivAQ 
        {

                        
            public class Consultor
            {
                public Consultor(String fecha, int local, String CODCUENTA, String NOMBRECTA, String CC, int DEBE, int HABER, String RUT, String TIPO, String DOCUMENTO, String GLOSA)
                {
                    this.FECHA_PRO = fecha;
                    this.LOCAL = local;
                    this.CODCUENTA = CODCUENTA;
                    this.NOMBRECTA = NOMBRECTA;
                    this.CC = CC;
                    this.DEBE = DEBE;
                    this.HABER = HABER;
                    this.RUT = RUT;
                    this.TIPO = TIPO;
                    this.DOCUMENTO = DOCUMENTO;
                    this.GLOSA = GLOSA;


                }

                public String FECHA_PRO { get; set; }
                public int LOCAL { get; set; }
                public String CODCUENTA { get; set; }
                public String NOMBRECTA { get; set; }
                public String CC { get; set; }
                public int DEBE { get; set; }
                public int HABER { get; set; }
                public String RUT { get; set; }
                public String TIPO { get; set; }
                public String DOCUMENTO { get; set; }
                public String GLOSA { get; set; }

            }

            public void datosAQ(DataTable t1, int Local, String fechaArc)
            {
                
                
                String GLOSA = "ARQUEO LOCAL " + Local + " " + fechaArc;
                String GLOSA2;
                String TIPO = "";
                String DOCUMENTO = "";
                String CC = Local.ToString();
                if (CC.Length == 3)
                {
                    CC = "01-0" + Local;
                }
                else if (CC.Length == 2)
                        {
                            CC = "01-00" + Local;
                        }
                        else if (CC.Length == 1)
                                {
                                    CC = "01-000" + Local;
                                }

                List<Consultor> lista = new List<Consultor>();

                foreach (DataRow r in t1.Rows)
                {
                    //...........................Inicio de CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS ................................................

                    if (Local == 2)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            //String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 16)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 201)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO"; ;

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 202)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 203)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 204)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 205)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 206)
                    {
                        if (Convert.ToInt32(r[13]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[13]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-014";
                            String NOMBRECTA = "CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[13]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " CREDITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //...........................Fin de CLIENTES TARJ. DE CREDITO A LOCALES PROPIOS .....................................................

                    //...........................Inicio de CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS ...................................................

                    if (Local == 2)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO"; ;

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 16)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 201)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 202)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 203)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 204)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 205)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }
                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Local == 206)
                    {
                        if (Convert.ToInt32(r[12]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = Convert.ToInt32(r[12]);
                            int HABER = 0;
                            String RUT = "";
                            String TIPO2 = TIPO;
                            GLOSA2 = GLOSA + " DEBITO";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-013";
                            String NOMBRECTA = "CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[12]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " DEBITO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //...........................Fin de CLIENTES TARJ. DE DEBITO A LOCALES PROPIOS ......................................................

                    //...........................Inicio de Paris Cencosud .............................................................................

                    if (Convert.ToInt32(r[21]) >= 0)
                    {
                        if (Convert.ToInt32(r[22]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-007";
                            String NOMBRECTA = "CLIENTE CON TARJETA PARIS";
                            int DEBE = Convert.ToInt32(r[22]);
                            int HABER = 0;
                            String RUT = "";
                            String GLOSA1 = GLOSA + "(CENCOSUD)";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA1);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-007";
                            String NOMBRECTA = "CLIENTE CON TARJETA PARIS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[22]); ;
                            String RUT = "";
                            GLOSA = GLOSA + "(CENCOSUD)";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                            lista.Add(c);
                        }
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-007";
                        String NOMBRECTA = "CLIENTE CON TARJETA PARIS";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[21]); ;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    //............................Fin de Paris Cencosud ...............................................................................

                    if (Convert.ToInt32(r[25]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-002";
                        String NOMBRECTA = "CLIENTES TARJ. FALABELLA";
                        int DEBE = Convert.ToInt32(r[25]);
                        int HABER = 0;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-002";
                        String NOMBRECTA = "CLIENTES TARJ. FALABELLA";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[25]); ;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    //...........................Fin de CMR............................................................................................

                    //...........................Inicio de ABCDIN......................................................................................

                    if (Local == 2)
                    {
                        if (Convert.ToInt32(r[25]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-011";
                            String NOMBRECTA = "CLIENTES TARJ. DIN/ABC";
                            int DEBE = Convert.ToInt32(r[25]);
                            int HABER = 0;
                            String RUT = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-011";
                            String NOMBRECTA = "CLIENTES TARJ. DIN/ABC";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[25]); ;
                            String RUT = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                            lista.Add(c);
                        }
                    }

                    if (Local == 16)
                    {
                        if (Convert.ToInt32(r[25]) >= 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-011";
                            String NOMBRECTA = "CLIENTES TARJ. DIN/ABC";
                            int DEBE = Convert.ToInt32(r[25]);
                            int HABER = 0;
                            String RUT = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                            lista.Add(c);
                        }
                        else
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-042-011";
                            String NOMBRECTA = "CLIENTES TARJ. DIN/ABC";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[25]); ;
                            String RUT = "";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                            lista.Add(c);
                        }
                    }


                    //...........................Fin de ABCDIN.........................................................................................

                    //...........................Inicio de Avance efectivo.............................................................................
                    if (Convert.ToInt32(r[33]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-062-009";
                        String NOMBRECTA = "AVANCES EN EFECTIVO TARJETAS";
                        int DEBE = Convert.ToInt32(r[33]);
                        int HABER = 0;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-062-009";
                        String NOMBRECTA = "AVANCES EN EFECTIVO TARJETAS";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[33]); ;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    //...........................Fin de Avance efectivo................................................................................

                    //...........................Inicio de TARJETA RIPLEY..............................................................................

                    if (Convert.ToInt32(r[24]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-010";
                        String NOMBRECTA = "TARJETA RIPLEY";
                        int DEBE = Convert.ToInt32(r[24]);
                        int HABER = 0;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-010";
                        String NOMBRECTA = "TARJETA RIPLEY";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[24]); ;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }


                    //...........................Fin de TARJETA RIPLEY.................................................................................

                    //...........................Inicio de TARJETA CYD.................................................................................

                    if (Convert.ToInt32(r[35]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-005";
                        String NOMBRECTA = "CLIENTE TARJETA CYD (SUR)";
                        int DEBE = Convert.ToInt32(r[35]);
                        int HABER = 0;
                        String RUT = ""; ;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-005";
                        String NOMBRECTA = "CLIENTE TARJETA CYD (SUR)";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[35]); ;
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }

                    //...........................Fin de TARJETA CYD....................................................................................

                    //...........................Inicio de DEPOSITOS RECETARIO MAGISTRAL (PREV).........................................................


                    if (Convert.ToInt32(r[32]) > 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-012-054";
                        String NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL";
                        int DEBE = Convert.ToInt32(r[32]);
                        int HABER = 0;
                        String RUT = "";
                        GLOSA2 = GLOSA + " LOCAL " + Local + " ABONO PREVTA. REC. MAG.";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA);
                        lista.Add(c);
                    }
                    else
                    {
                        if (Convert.ToInt32(r[32]) < 0)
                        {


                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-012-054";
                            String NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[32]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " LOCAL " + Local + " ABONO PREVTA. REC. MAG.";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //______________------------------------________________________----------------------_______________________________---------------__________

                    if (Convert.ToInt32(r[40]) > 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-012-054";
                        String NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL";
                        int DEBE = Convert.ToInt32(r[40]);
                        int HABER = 0;
                        String RUT = "";
                        GLOSA2 = GLOSA + " LOCAL " + Local + " REC. MAG. EFECTIVO";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {
                        if (Convert.ToInt32(r[40]) < 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "1-1-012-054";
                            String NOMBRECTA = "DEPOSITOS RECETARIO MAGISTRAL";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[40]); ;
                            String RUT = "";
                            GLOSA2 = GLOSA + " LOCAL " + Local + " REC. MAG. EFECTIVO";

                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                            lista.Add(c);
                        }
                    }

                    //..............................Fin de DEPOSITOS RECETARIO MAGISTRAL (PREV)...........................................................

                    //..............................inicio de CLIENTES POR COBRAR.........................................................................

                    if (Convert.ToInt32(r[18]) < 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-043-003";
                        String NOMBRECTA = "CLIENTES POR COBRAR";
                        int DEBE = Convert.ToInt32(r[18]) * -1;
                        int HABER = 0;
                        String RUT = "";
                        GLOSA2 = "LOCAL " + Local + " DESCTO. BONIFICACION";


                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-043-003";
                        String NOMBRECTA = "CLIENTES POR COBRAR'";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[18]); ;
                        String RUT = "";
                        GLOSA2 = "LOCAL " + Local + " DESCTO. BONIFICACION";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }

                    //..............................Fin de CLIENTES POR COBRAR............................................................................

                    //..............................inicio de CLIENTES CON GUIAS CRUZ VERDE...............................................................

                    if (Convert.ToInt32(r[18]) < 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-043-002";
                        String NOMBRECTA = "CLIENTES CON GUIAS CRUZ VERDE";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[18]) * -1;
                        String RUT = "";
                        GLOSA2 = "LOCAL " + Local + " DESCTO. BONIFICACION"; ;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-043-002";
                        String NOMBRECTA = "CLIENTES CON GUIAS CRUZ VERDE";
                        int DEBE = Convert.ToInt32(r[18]);
                        int HABER = 0;
                        String RUT = "";
                        GLOSA2 = "LOCAL " + Local + " DESCTO. BONIFICACION";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }

                    //..............................Fin de CLIENTES CON GUIAS CRUZ VERDE...................................................................

                    //..............................inicio de CLIENTES CON BOLETA..........................................................................

                    if (Convert.ToInt32(r[5]) > 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-041-002";
                        String NOMBRECTA = "CLIENTES CON BOLETAS";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[5]);
                        String RUT = "";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    if (Convert.ToInt32(r[5]) != 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-041-002";
                        String NOMBRECTA = "CLIENTES CON BOLETAS";
                        int DEBE = Convert.ToInt32(r[5]);
                        int HABER = 0;
                        String RUT = "";
                        TIPO = "BN";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }


                    //..............................Fin de CLIENTES CON BOLETA.............................................................................

                    //..............................Inicio de CLIENTES TARJ. CREDITO FCV...................................................................

                    if (Convert.ToInt32(r[13]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-014";
                        String NOMBRECTA = "CLIENTES TARJ. CREDITO FCV";
                        int DEBE = Convert.ToInt32(r[13]);
                        int HABER = 0;
                        String RUT = "";
                        String TIPO2 = TIPO;
                        GLOSA2 = GLOSA + " CREDITO";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-014";
                        String NOMBRECTA = "CLIENTES TARJ. CREDITO FCV";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[13]);
                        String RUT = "";
                        GLOSA2 = GLOSA + " CREDITO";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }

                    //..............................Fin de CLIENTES TARJ. CREDITO FCV......................................................................

                    //..............................Inicio de CLIENTES TARJ. DEBITO FCV....................................................................
                    if (Convert.ToInt32(r[12]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-013";
                        String NOMBRECTA = "CLIENTES TARJ. DEBITO FCV";
                        int DEBE = Convert.ToInt32(r[12]);
                        int HABER = 0;
                        String RUT = "";
                        String TIPO2 = TIPO;
                        GLOSA2 = GLOSA + " DEBITO";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-013";
                        String NOMBRECTA = "CLIENTES TARJ. DEBITO FCV";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[12]);
                        String RUT = "";
                        GLOSA2 = GLOSA + " DEBITO";

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }


                    //..............................Fin de CLIENTES TARJ. DEBITO FCV......................................................................

                    //..............................inicio de CLIENTES CON TARJ. CRUZ VERDE...............................................................

                    if (Convert.ToInt32(r[16]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-001";
                        String NOMBRECTA = "CLIENTES CON TARJ. CRUZ VERDE";
                        int DEBE = Convert.ToInt32(r[16]);
                        int HABER = 0;
                        String RUT = "";
                        String TIPO2 = TIPO;
                        GLOSA2 = GLOSA;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-001";
                        String NOMBRECTA = "CLIENTES CON TARJ. CRUZ VERDE";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[16]) * -1;
                        String RUT = "";
                        GLOSA2 = GLOSA;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }


                    //..............................Fin de CLIENTES CON TARJ. CRUZ VERDE..................................................................

                    //..............................inicio de CLIENTES CON CONVENIOS CREDITO..............................................................

                    if (Convert.ToInt32(r[17]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-015";
                        String NOMBRECTA = "CLIENTES CON CONVENIOS CREDITO";
                        int DEBE = Convert.ToInt32(r[17]) + Convert.ToInt32(r[18]);
                        int HABER = 0;
                        String RUT = "";
                        String TIPO2 = TIPO;
                        GLOSA2 = GLOSA;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-015";
                        String NOMBRECTA = "CLIENTES CON CONVENIOS CREDITO";
                        int DEBE = 0;
                        int HABER = (Convert.ToInt32(r[17]) + Convert.ToInt32(r[18])) * -1;
                        String RUT = "";
                        GLOSA2 = GLOSA; ;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }


                    //..............................Fin de CLIENTES CON CONVENIOS CREDITO................................................................

                    //..............................Inicio de DICC CID CIE VOUCHER-NOMINAS...............................................................


                    if (Convert.ToInt32(r[19]) >= 0)
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-016";
                        String NOMBRECTA = "DICC CID CIE VOUCHER-NOMINAS";
                        int DEBE = Convert.ToInt32(r[19]) + Convert.ToInt32(r[18]);
                        int HABER = 0;
                        String RUT = "";
                        String TIPO2 = TIPO; ;
                        GLOSA2 = GLOSA;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }
                    else
                    {

                        String dFecha_pro = r["FECHA_PRO"].ToString();
                        int nuLocal = Convert.ToInt32(r["LOCAL"]);
                        String CODCUENTA = "1-1-042-016";
                        String NOMBRECTA = "DICC CID CIE VOUCHER-NOMINAS";
                        int DEBE = 0;
                        int HABER = Convert.ToInt32(r[19]) * -1;
                        String RUT = "";
                        GLOSA2 = GLOSA;

                        Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, CC, DEBE, HABER, RUT, TIPO, DOCUMENTO, GLOSA2);
                        lista.Add(c);
                    }

                    //..............................Fin de DICC CID CIE VOUCHER-NOMINAS...............................................................

                    //..............................Inicio de VENTAS CLIENTES CONSALUD................................................................

                    /*  if (Convert.ToInt32(r[19]) >= 0)
                      {

                          String dFecha_pro = r["FECHA_PRO"].ToString();
                          int nuLocal = Convert.ToInt32(r["LOCAL"]);
                          String CODCUENTA = "1-1-042-017";
                          String NOMBRECTA = "VENTAS CLIENTES CONSALUD";
                          int DEBE = Convert.ToInt32(r[19]) + Convert.ToInt32(r[18]);
                          int HABER = 0;
                          String RUT = "";
                          String TIPO2 = TIPO;
                          GLOSA2 = GLOSA;

                          Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, DEBE, HABER, RUT, TIPO, GLOSA2);
                          lista.Add(c);
                      }
                      else
                      {

                          String dFecha_pro = r["FECHA_PRO"].ToString();
                          int nuLocal = Convert.ToInt32(r["LOCAL"]);
                          String CODCUENTA = "1-1-042-017";
                          String NOMBRECTA = "VENTAS CLIENTES CONSALUD";
                          int DEBE = 0;
                          int HABER = Convert.ToInt32(r[19]) * -1;
                          String RUT = "";
                          GLOSA2 = GLOSA;

                          Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, DEBE, HABER, RUT, TIPO, GLOSA2);
                          lista.Add(c);
                      }*/

                    //..............................Fin de VENTAS CLIENTES CONSALUD...................................................................

                    //..............................Inicio de VENTAS CON BOLETAS......................................................................

                    /*
                        if (Convert.ToInt32(r[7]) > 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "6-1-010-001";
                            String NOMBRECTA = "VENTAS CON BOLETAS";
                            int DEBE = 0;
                            int HABER = Convert.ToInt32(r[7]);
                            String RUT = "";
                            TIPO = "BN";
                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, DEBE, HABER, RUT, TIPO, GLOSA);
                            lista.Add(c);
                        }
                        if (Convert.ToInt32(r[5]) > 0)
                        {

                            String dFecha_pro = r["FECHA_PRO"].ToString();
                            int nuLocal = Convert.ToInt32(r["LOCAL"]);
                            String CODCUENTA = "6-1-010-001";
                            String NOMBRECTA = "VENTAS CON BOLETAS";
                            int DEBE = Convert.ToInt32(r[7]);
                            int HABER = 0;
                            String RUT = "";


                            Consultor c = new Consultor(dFecha_pro, nuLocal, CODCUENTA, NOMBRECTA, DEBE, HABER, RUT, TIPO, GLOSA);
                            lista.Add(c);
                        }*/

                    //..............................Fin de VENTAS CON BOLETAS.......................................................................



                    //........................................................................................................................................
                }
                //...........................Inicio Foreach AQ ................................................
                try
                        {
                            String rutaV = "C:\\Central 2017\\tablas 2014\\";
                            String db = "VOUCHER.dbf";

                            OdbcConnection con = new OdbcConnection();

                            con.ConnectionString = "Driver={Microsoft dBASE Driver (*.dbf)}; SourceType=DBF; DBQ=" + rutaV + ";DriverID=277;";

                            String query = "SELECT * FROM " + db + "";

                            con.Open();

                            OdbcDataAdapter a = new OdbcDataAdapter(query, con);
                            DataTable t = new DataTable();

                            a.Fill(t);
                            
                            
                            dgvDatosVaucher.DataSource = lista;

                            con.Close();

                            double sumaDebe = 0;
                            double sumaHaber = 0;

                            foreach (DataGridViewRow row in dgvDatosVaucher.Rows)
                            {
                                sumaDebe += Convert.ToInt32(row.Cells[5].Value);
                                sumaHaber += Convert.ToInt32(row.Cells[6].Value);
                            }
                    
                            txtTotalDebe.Text = Convert.ToString(sumaDebe);
                            txtTotalHaber.Text = Convert.ToString(sumaHaber);

                            txtDiferenciaDH.Text = Convert.ToString(Convert.ToInt32(txtTotalDebe.Text) - Convert.ToInt32(txtTotalHaber.Text));

                            if(Convert.ToInt32(txtDiferenciaDH.Text) == 0){

                                txtDiferenciaDH.Text = "Comprobante OK";
                            }

                        }
                        catch (OdbcException oex)
                        {
                            MessageBox.Show(oex.ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
           //............... fin llenado 2° datagridview ........................................................................................................................

            }
            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            }
        }
    }
}
