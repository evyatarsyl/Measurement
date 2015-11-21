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
using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        static int Measured = 0;
        const int interval = 1000;
//fffff
        static string path = @"C:\Users\Evyatar\Desktop\Measure.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Write(4);
                sw.Close();
            }
            StreamReader sr = new StreamReader(path);
            int border = Convert.ToInt32(sr.ReadLine());
            sr.Close();

            //------------------------------------------//

            while (true)
            {
                Measured += 1;
                if (Measured >= border)
                {
                    TcpClient client = new TcpClient("localhost", 2444);
                    try
                    {
                        Stream s = client.GetStream();
                        StreamReader NetSR = new StreamReader(s);
                        StreamWriter NetSW = new StreamWriter(s);
                        NetSW.AutoFlush = true;
                        NetSW.WriteLine("done");
                        string cmd = NetSR.ReadLine();
                        if (cmd == "off")
                        {
                            NetSR.Close();
                            NetSW.Close();
                            s.Close();
                            client.Close();
                            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            proc.StartInfo.FileName = @"C:\Users\Evyatar\Desktop\batchfile.bat";
                            proc.StartInfo.RedirectStandardError = false;
                            proc.StartInfo.RedirectStandardOutput = false;
                            proc.StartInfo.UseShellExecute = true;
                            proc.Start();
                            proc.WaitForExit();
                        }
                        else
                        {
                            MessageBox.Show("אתה יכול להיות עוד על המחשב");
                        }
                    }


                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: /n/n" + ex);
                        client.Close();
                    }


                }
                Thread.Sleep(interval);
            }
        }
    }
}
