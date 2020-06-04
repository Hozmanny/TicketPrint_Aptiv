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
using System.Xml;
using System.Drawing.Printing;
using System.Diagnostics;

namespace ImpresionRecibosAptiv
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();

        }

        private void FormBase_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        string result, bbva;
        public void btnImprimir_Click(object sender, EventArgs e)
        {
            if (bbva == null)
            {
                MessageBox.Show("Busca primero tu numero de gafete", "Ingresar gafete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //System.Diagnostics.Process.Start(bbva);

                SendToPrinter(bbva);
                tbNombre.Clear();
                tbGafete.Clear();
                bbva = null;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tbNombre.Clear();
            tbGafete.Clear();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (tbGafete.Text == "")
            {
                MessageBox.Show("Escanea o ingresa tu numero de gafete","Falta de gafete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bbva = Buscar(tbGafete.Text);
                if (bbva == "No se encontro nada")
                {
                    MessageBox.Show("No se se encuentra recibo", "Busqueda sin exito", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbGafete.Clear();
                    tbNombre.Clear();
                }
            }
        }

        public string Buscar(string gafete)
        {
            string vScan = gafete;
            string[] dirs = Directory.GetFiles(@"C:\Users\MANNY\Documents\Aptiv_Kiosco\", "*.xml");
            string attrVal, nombre;
            for (int i = 0; i < dirs.Length; i++)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(dirs[i]);
                XmlNodeList elemList = doc.GetElementsByTagName("nomina12:Receptor");
                XmlNodeList elemList2 = doc.GetElementsByTagName("cfdi:Receptor");
                attrVal = elemList[0].Attributes["NumEmpleado"].Value;
                nombre = elemList2[0].Attributes["Nombre"].Value;
                if (vScan == attrVal)
                {
                    tbNombre.Text = nombre;

                    result = Path.ChangeExtension(dirs[i], ".pdf");
                    return result;
                    //System.Diagnostics.Process.Start(result);

                    //break;
                }
            }
            return "No se encontro nada";
        }

        private void SendToPrinter(string patho)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = patho;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }
    }
}
