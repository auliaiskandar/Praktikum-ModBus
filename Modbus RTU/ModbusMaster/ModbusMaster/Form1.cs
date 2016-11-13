using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Modbus.Device;


namespace ModbusMaster
{
    public partial class Form1 : Form
    {
        ModbusSerialMaster modbusMasterRTU; 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string item in SerialPort.GetPortNames())
            {
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
                comboBoxSerial.Items.Add(item);
                comboBoxSerial.SelectedIndex = 0;
            } 

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            serialPortModbus.PortName = comboBoxSerial.Text;
            if (!serialPortModbus.IsOpen)
            {
                serialPortModbus.Open();
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                try
                {
                    modbusMasterRTU = ModbusSerialMaster.CreateRtu(serialPortModbus);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
                timerGetData.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed To Open Port");
            } 

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            timerGetData.Enabled = false;
            if (serialPortModbus.IsOpen)
            {
                serialPortModbus.Close();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            } 

        }

        private void timerGetData_Tick(object sender, EventArgs e)
        {
            try
            {
                //ushort[] regMW=null;
                ushort[] regMW = modbusMasterRTU.ReadHoldingRegisters(1, 0, 8);
                textBox1.Text = regMW[0].ToString();
                textBox2.Text = regMW[1].ToString();
                textBox3.Text = regMW[2].ToString();
                textBox4.Text = regMW[3].ToString();
                textBox5.Text = regMW[4].ToString();
                textBox6.Text = regMW[5].ToString();
                textBox7.Text = regMW[6].ToString();
                textBox8.Text = regMW[7].ToString();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            } 
        }

        private void buttonWriteReg_Click(object sender, EventArgs e)
        {
            ushort[] value = { 0 };
            try
            {
                value[0] = ushort.Parse(textBox7.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            modbusMasterRTU.WriteMultipleRegisters(1, 6, value); 

        }

    
       
    }
}
