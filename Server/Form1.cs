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
using System.Net.Sockets;
using System.Threading;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Thread listen = new Thread(new ThreadStart(Service));
        static TcpListener listener;
        static string cmd;

        private void Form1_Load(object sender, EventArgs e)
        {
            listen.Start();
        }
        
        static void Service()
        {
            listener = new TcpListener(2444);
            listener.Start();
            Socket soc = listener.AcceptSocket();
            NetworkStream NetStream = new NetworkStream(soc);
            StreamReader NetSR = new StreamReader(NetStream);
            StreamWriter NetSW = new StreamWriter(NetStream);
            NetSW.AutoFlush = true;
            string cmd = NetSR.ReadLine();
            Console.WriteLine(cmd);
            if (cmd == "done")
            {
                string caption = "Timeout";
                string message = "זמן המחשב שהקצבת לילדך תם. האם ברצונך לכבות את מחשבו?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    NetSW.WriteLine("off");
                    NetSW.Close();
                    NetSR.Close();
                }
                else
                {
                    NetSW.WriteLine("on");
                }
                
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            listen.Abort();
            listen.Join();
        }
    }
}
