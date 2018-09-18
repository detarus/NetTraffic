using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTraffic
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            var Li=Application.OpenForms.OfType<Menu>().FirstOrDefault().A.QU("Select * FROM main_log");
            dataGridView1.RowCount = Li.Length;
            for (int i=0; i<Li.Length; i++)
            {
                dataGridView1[0, i].Value = Li[i].ItemArray[1];
                dataGridView1[1, i].Value = Li[i].ItemArray[2];
                dataGridView1[2, i].Value = Li[i].ItemArray[3];
            }
        }
    }
}
