using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CAN_client
{
    public partial class Form1 : Form
    {
        public static string serv, login, passwd;
        public static int port;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            tbServer.Text = "85.93.88.27";
            tbPort.Text = "9000";

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            serv = tbServer.Text;
            port = int.Parse(tbPort.Text);
            login = tbLogin.Text;
            passwd = tbPasswd.Text;

            Program.OpenConnection(serv, port);
            
        }
    }
}
