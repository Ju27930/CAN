using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CAN_client
{
    public static class Program
    {
        static Form1 affForm1;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             
            affForm1 = new Form1();
            affForm1.ShowDialog();
            Application.Run();
            
            
        }

        public static TcpClient client;

       
        public static void OpenConnection(string HOST, int PORT)
        {
            if (client != null)
            {
                return;
            }
            else
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(HOST, PORT);
                    SendData("LOGIN|" + Form1.login + "|" + Form1.passwd);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Une erreur est survenue Motha fucka..\n\n" + Ex, "Oups j'ai peut être coupé le serveur?? :X",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                    client = null;
                }
            }

        }

        static void CloseConnection()
        {
            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--Aucune connection en cours--");
                return;
            }

            try
            {
                client.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--Connection fermée--");
                client = null;
                // Renvoyer le FORM 1 StartNode();
            }
            catch (Exception ex)
            {
                Console.WriteLine("erreur.. " + ex);
                client = null;
            }

        }

        static void receiveData()
        {
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Received : " + Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));
            if (Encoding.UTF8.GetString(bytesToRead, 0, bytesRead) == "ERREURLOGIN")
            {
                MessageBox.Show("Le Login / Mot de passe est incorrecte", "Erreur d'identification",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                

                 Form2 affForm2 = new Form2();
                Program.affForm1.Hide();
                affForm2.ShowDialog();
                
            }
        }
        static void SendData(string data)
        {
            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("--Connection non etablie--");
                return;
            }

            byte[] bytesToSend = UTF8Encoding.UTF8.GetBytes(data);
            NetworkStream nwStream = client.GetStream();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("DATA : " + data);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            
            receiveData();



            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter data to send: ");
            var data2 = Console.ReadLine();
            SendData(data2);

        }



    


    }
}
