using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using MySql.Data.MySqlClient;
namespace NetTraffic
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }
        public class SL
        {
            public string serverName;
            public string userName;
            public string dbName;
            public string port;
            public string password;
            public string connStr;
            public MySqlConnection connection;
            public SL()
            {
                serverName = "localhost";
                userName = "root";
                dbName = "traffic_log";
                port = "3306";
                password = "";
                connStr = "server=" + serverName +";user=" + userName +";database=" + dbName +";port=" + port + ";password=" + password + ";";
            }
           
            public DataRow[] QU(string sql)
            {
                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand sqlCom = new MySqlCommand(sql, connection);
                connection.Open();
                sqlCom.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                var myData = dt.Select();
                return myData;
            }
            public void NQ(string sql)
            {
                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand sqlCom = new MySqlCommand(sql, connection);
                connection.Open();
                sqlCom.ExecuteNonQuery();
                return;
            }
        }
        public PerformanceCounterCategory performanceCounterCategory;
        public string instance;
        public PerformanceCounter performanceCounterSent;
        public PerformanceCounter performanceCounterReceived;
        public double averageRec;
        public double maxRec;
        public double minRec;
        public double allRec=0;
        public SL A = new SL();
        public double averageSend;
        public double maxSend;
        public double minSend;
        public double allSend=0;
        public int numt=0;
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
        instance = performanceCounterCategory.GetInstanceNames()[2]; // 1st NIC !
        performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
        performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
        timer1.Enabled = true;
            button5.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = false;
            button1.Enabled = false;
            label1.Text = "Идёт анализ сетевого трафика...";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double rec = performanceCounterReceived.NextValue() / 1024;
            double send = performanceCounterSent.NextValue() / 1024;
            string date = DateTime.Now.ToString();
            if (numt == 0)
            {
                maxSend = send;
                minSend = send;
                allSend = send;
                maxRec = rec;
                minRec = rec;
                allRec = rec;
            }
            numt++;
            richTextBox1.Text = richTextBox1.Text + "Kb отправлено:" + send + "  Kb получено: " + rec+  "  Дата и время: "+ date + "\n";
            if (send > maxSend) maxSend = send;
            if (send < minSend) minSend = send;
            if (rec > maxRec) maxRec = rec;
            if (rec < minRec) minRec = rec;
            allRec += (rec);
            allSend +=( send);
            A.NQ("INSERT INTO main_log(Sent_kbps, Received_kpbs, Date)  VALUES ('" + (send) + "','" + (rec) + "','"+ date + "')");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numt = 0;

            richTextBox1.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result Res = new Result();
            if (allRec != 0)
            {
                Res.textBox1.Text = "OK";
                Res.groupBox2.Visible = true;
                Res.textBox10.Text = (allRec/numt).ToString();
                Res.textBox9.Text = (maxRec).ToString();
                Res.textBox8.Text = (minRec).ToString();
                Res.textBox7.Text = (allRec).ToString();
            }
            else Res.textBox1.Text = "Не было отслежено ни одного пакета.";
            if (allSend != 0)
            {
                Res.textBox2.Text = "OK";
                Res.groupBox3.Visible = true;
                Res.textBox3.Text = (allSend / numt).ToString();
                Res.textBox4.Text = (maxSend).ToString();
                Res.textBox5.Text = (minSend).ToString();
                Res.textBox6.Text = (allSend).ToString();
            }
            else Res.textBox2.Text = "Не было отслежено ни одного пакета.";
            Res.textBox11.Text = (numt * timer1.Interval / 1000).ToString() + " сек.";
            timer1.Enabled = false;
            numt = 0;
            richTextBox1.Text = "";
            button5.Enabled = false;
            button4.Enabled = true;
            button1.Enabled = true;
            label1.Text = "Анализ сетевого трафика закончен.";
            Res.ShowDialog();
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                button5.Text = "Пуск";
                label1.Text = "Анализ сетевого трафика приостановлен";
                button4.Enabled = true;
            }
            else{
                timer1.Enabled = true;
                button5.Text = "Пауза";
                button4.Enabled = false;
                label1.Text = "Идёт анализ сетевого трафика...";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Log Lo = new Log();
            Lo.ShowDialog();
        }
    }
}
