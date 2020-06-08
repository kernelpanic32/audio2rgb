using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO.Ports;
using Un4seen.Bass;
using Un4seen.BassWasapi;
using System.ComponentModel;

namespace Audio_Spectrum
{

    public partial class Form1 : Form
    {
        private Analyzer analyzer;

        int r_prom = 0, g_prom= 0, b_prom = 0;

        public Form1()
        {
            InitializeComponent();

            //set default width (contracted)
            this.Width = 346;

            //serial com port list
            comports_box.Items.Clear();
            var ports = SerialPort.GetPortNames();
            comports_box.Text = ports[0];
            foreach (var port in ports) comports_box.Items.Add(port);           

            //audio bar graph init
            analyzer = new Analyzer(progressBar1, progressBar2, spectrum1, comboBox1, chart1);

        }

        private void comport_conn_Click(object sender, EventArgs e)
        {
            if(comport_conn.Text == "Connect")
            {
                //Serial port settings
                serialPort1.BaudRate = Convert.ToInt32(comport_baud.Text);
                if(comports_box.Text == "")
                {
                    serialPort1.PortName = (comports_box.Items[comports_box.SelectedIndex] as string);
                } else
                {
                    serialPort1.PortName = comports_box.Text;
                }
                
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.DtrEnable = true;
                serialPort1.Open();
                if (serialPort1.IsOpen)
                {
                    Btn_Enable.Enabled = true;
                }
                comport_conn.Text = "Disconnect";
            } else
            {
                Btn_Enable.Text = "Enable";
                Btn_Enable.Enabled = false;
                comport_conn.Text = "Connect";
                analyzer.Enable = false;
                analyzer.DisplayEnable = false;
                timer2.Enabled = false;
                serialPort1.Close();
            }
            
        }


        private void timer2_Tick(object sender, EventArgs e)
        {

           colorViewer.BackColor = System.Drawing.Color.FromArgb(r_prom, g_prom, b_prom);

            if (Btn_Enable.Text != "Enable")
            {            
                //creates an averge between selected bars
                r_prom = ((Convert.ToInt32(spectrum1.Bar01.Value) + Convert.ToInt32(spectrum1.Bar02.Value) + Convert.ToInt32(spectrum1.Bar03.Value)) / 3);
                g_prom = ((Convert.ToInt32(spectrum1.Bar08.Value) + Convert.ToInt32(spectrum1.Bar09.Value) + Convert.ToInt32(spectrum1.Bar10.Value)) / 3);
                b_prom = ((Convert.ToInt32(spectrum1.Bar20.Value) + Convert.ToInt32(spectrum1.Bar21.Value) + Convert.ToInt32(spectrum1.Bar22.Value)) / 3);

                //send serial data
                serialPort1.WriteLine(r_prom + "," + g_prom + "," + b_prom);

                //data visualization
                red_bar.Width = Convert.ToInt32(r_prom * 0.39);
                green_bar.Width = Convert.ToInt32(g_prom * 0.39);
                blue_bar.Width = Convert.ToInt32(b_prom * 0.39);

                red_label.Text = Convert.ToString(r_prom);
                green_label.Text = Convert.ToString(g_prom);
                blue_label.Text = Convert.ToString(b_prom);
            }

        }

        //enable/disable analyzer & rgb channels
        private void Btn_Enable_Click(object sender, EventArgs e)
        {        
            if (Btn_Enable.Text == "Enable")
            {
                analyzer.Enable = true;
                analyzer.DisplayEnable = true;
                Btn_Enable.Text = "Disable";
                timer2.Enabled = true;
            }
            else
            {
                analyzer.Enable = false;
                analyzer.DisplayEnable = false;
                Btn_Enable.Text = "Enable";

                timer2.Enabled = false;
            }
        }

        //hide to tray
        private void hide_btn_Click(object sender, EventArgs e)
        {    
            this.Hide();
            this.ShowInTaskbar = false;
        }

        //expand form to show sound graph
        private void showGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (showGraph.Checked)
            {
                //expanded
                this.Width = 875;
            } else
            {
                //contracted
                this.Width = 346;
            }
        }

        //places the form on top of everything
        private void pinTop_CheckedChanged(object sender, EventArgs e)
        {
            if (pinTop.Checked)
            {
                this.TopMost = true;
            } else
            {
                this.TopMost = false;
            }
                
        }

        //double click tray show window
        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {         
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {      
                //exit application when form is closed
                //Application.Exit();        
        }

    }
}