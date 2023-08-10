using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics.Eventing.Reader;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        int saat = DateTime.Now.Hour;
        int dk = DateTime.Now.Minute;
        int sn = DateTime.Now.Second;
        string e = Environment.NewLine;
        bool port_durum = false;
        bool makine_kaydir = false;
        bool ozel_mesaj = true;

        public Form1()
        {
            InitializeComponent();
        }
        public void yaz(String v)
        {
            if (checkBox1.Checked)
            {
                saat = DateTime.Now.Hour;
                dk = DateTime.Now.Minute;
                sn = DateTime.Now.Second;
                textBox1.AppendText(e+saat+":"+dk+":"+sn+"  --> "+ v + e);
            }
            else
            {
                textBox1.AppendText(e + v + e);
            }
            
        }
        private void Alln(string vv)
        {
            if (checkBox1.Checked)
            {
                saat = DateTime.Now.Hour;
                dk = DateTime.Now.Minute;
                sn = DateTime.Now.Second;
                textBox1.AppendText(e +saat+":"+dk+":"+sn+"  | makine --> "+ vv + e);
            }
            else
            {
                textBox1.AppendText(e + " makine --> " + vv + e);
            } 
        }
        private void Al(String vv)
        {
                textBox1.AppendText(vv);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "bağlan";
            button1.BackColor = Color.White;
            textBox1.ReadOnly = true;
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Both;
            button1.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_baglan(object sender, EventArgs e)
        { 
            
            if(comboBox1.SelectedItem != null && !port1.IsOpen && !port_durum)
            {
                try
                {
                    port1.PortName = comboBox1.SelectedItem.ToString();
                    port1.Open();
                    if(port1.IsOpen)
                    {
                        button1.BackColor = Color.Green;
                        button1.Text = "kes";
                        port_durum = true;
                        yaz("bağlantı başlatıldı");
                        button1.CheckState = CheckState.Checked;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(port_durum == true)
            {
                try
                {
                    port1.Close();
                    if (!port1.IsOpen)
                    {
                        button1.CheckState = CheckState.Unchecked;
                        button1.BackColor = Color.White;
                        button1.Text = "bağlan";
                        port_durum = false;
                        yaz("bağlantı kesildi");
                        button1.CheckState = CheckState.Unchecked;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            } 
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            foreach (String port in ports)
            {
                comboBox1.Items.Add(port);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (port1.IsOpen)
                {
                    port1.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string girilen = textBox2.Text;
            string aranan = "";
            if(girilen.Length >= 1)
            {
                yaz(textBox2.Text);
                aranan = girilen.Substring(0,1);
            }
            if (aranan == "*")
            {
                ozel_mesaj = true;
                if (textBox2.Text == "*exit")
                {
                    Close();
                }
                if (textBox2.Text == "*clear")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                }
                if (textBox2.Text == "*timeon")
                {
                    checkBox1.Checked = true;
                }
                if (textBox2.Text == "*timeoff")
                {
                    checkBox1.Checked = false;
                }
                if (textBox2.Text == "*println")
                {
                    makine_kaydir = true;
                }
                if (textBox2.Text == "*print")
                {
                    makine_kaydir = false;
                } 
            }
            else
            {
                ozel_mesaj = false;
            }
            if (port1.IsOpen)
            {
                if (!ozel_mesaj)
                {
                    string veri = textBox2.Text.ToString();
                    port1.Write(veri);
                }
               
            }
            textBox2.Clear();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                button1.Enabled = true;
            }
        }

        private void port1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string veri = port1.ReadExisting();
            try
            {
                if (makine_kaydir)
                {
                    Alln(veri);
                }
                else
                {
                    Al(veri);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
