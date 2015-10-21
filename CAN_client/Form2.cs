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
        List<string> listConnecte = new List<string>();

        public Form2()
        {
            InitializeComponent();

            backgroundWorker1.RunWorkerAsync();
        }

        

        private void Form2_Load(object sender, EventArgs e)
        {

            
            richTextBox1.SelectionFont = new Font("Arial", 14, FontStyle.Regular);
            Program.SendData("WHO|");
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

                    if (message2[0] == "WHO")
                    {
                        listBox1.Invoke(new Action(delegate()
                            {
                                listBox1.Items.Clear();
                            }));
                        for (int i = 1; i < message2.Length-1; i++)
                              {
                                  listConnecte.Add(message2[i]);
                                  listBox1.Invoke(new Action(delegate()
                                  {
                                      listBox1.Items.Add(message2[i]);
                                  }));
                              }
                        

                    }
                    else
                    {
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                TopMost = true;
            else
                TopMost = false;
        }

        


    }
}
