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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            backgroundWorker1.RunWorkerAsync();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new Font("Arial", 14, FontStyle.Regular);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Program.CloseConnection();
            Application.Exit();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Program.client != null)
            {
                try
                {
                    NetworkStream nwStream = Program.client.GetStream();
                    byte[] bytesToRead = new byte[Program.client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, Program.client.ReceiveBufferSize);
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    Console.WriteLine("Received : " + Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));

                    string message = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead);
                    string[] message2 = message.Split('|');
                    
                        
                        richTextBox1.Invoke(new Action(delegate()
                          {
                              richTextBox1.Text += message2[0] + " > ";

                              for (int i = 1; i < message2.Length; i++)
                              {
                                  richTextBox1.Text += message2[i];
                              }

                             // richTextBox1.Text += System.Environment.NewLine;
                           }));

                      
                    
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Program.SendData(textBox1.Text);
                textBox1.Text = "";
            }
        }

        


    }
}
