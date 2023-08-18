using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
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

namespace Quality_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort serialPort;
        public AppData appData;

        public MainWindow()
        {
            appData = new AppData();
            serialPort = new SerialPort();
            InitializeComponent();
            InitializeSerialPorts();


            /*Thread thread = new Thread(keyboardInterupt);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();*/
        }

        public void keyboardInterupt()
        {
            while(true)
            {
                Thread.Sleep(40);
                if (Keyboard.GetKeyStates(Key.Down) > 0)
                {
                    appData.motorOutput1 += 1;
                    motorOutputDisplay1.Value += 1;

                }
                else
                {
                    appData.motorOutput1 -= 1;
                    motorOutputDisplay1.Value -= 1;
                }
                //motorOutputDisplay1.Value = appData.motorOutput1;
            }
        }

        private void InitializeSerialPorts()
        {

            string[] ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                portSelectionBox.Items.Add(port);
            }
            
        }

        private void openSerialPort(object sender, RoutedEventArgs e)
        {
            serialPort.PortName = portSelectionBox.Text;
            serialPort.Open();
            selectedPort.Content = serialPort.PortName;
            connectionState.Content = "połączono";
            communicationState.Content = "nie nawiązano";
        }

        private void getPacket(object sender, RoutedEventArgs e)
        {
            /*serialPort.Write("?");
            rawData.Text = serialPort.ReadExisting();
            communicationState.Content = "pobrano Pakiet";*/

            rawData.Text = "{" +
                "\"distanceSensor1\":\"off\"," +
                "\"distanceSensor2\":\"off\"," +
                "\"distanceSensor3\":\"off\"," +
                "\"distanceSensor4\":\"off\"," +
                "\"colorSensor1\":\"off\"," +
                "\"colorSensor2\":\"off\"" +
                "}";



            appData = JsonSerializer.Deserialize<AppData>(rawData.Text);

            distanceSensorDisplay1.Text = appData.distanceSensor1;
            distanceSensorDisplay2.Text = appData.distanceSensor2;
            distanceSensorDisplay3.Text = appData.distanceSensor3;
            distanceSensorDisplay4.Text = appData.distanceSensor4;

            colorSensorDisplay1.Text = appData.colorSensor1;
            colorSensorDisplay2.Text = appData.colorSensor2;

            if(appData.distanceSensor1 == "off")
            {
                distanceSensorDisplay1.Background = Brushes.Gray;
            }
            else
            {
                distanceSensorDisplay1.Background = Brushes.Green;
            }
            if (appData.distanceSensor2 == "off")
            {
                distanceSensorDisplay2.Background = Brushes.Gray;
            }
            else
            {
                distanceSensorDisplay1.Background = Brushes.Green;
            }
            if (appData.distanceSensor3 == "off")
            {
                distanceSensorDisplay3.Background = Brushes.Gray;
            }
            else
            {
                distanceSensorDisplay1.Background = Brushes.Green;
            }
            if (appData.distanceSensor4 == "off")
            {
                distanceSensorDisplay4.Background = Brushes.Gray;
            }
            else
            {
                distanceSensorDisplay1.Background = Brushes.Green;
            }

            if (appData.colorSensor1 == "off")
            {
                colorSensorDisplay1.Background = Brushes.Black;
                colorSensorDisplay1.Foreground = Brushes.White;

            }
            else
            {
                colorSensorDisplay1.Background = Brushes.White;
                colorSensorDisplay1.Foreground = Brushes.Black;
            }
            if (appData.colorSensor2 == "off")
            {
                colorSensorDisplay2.Background = Brushes.Black;
                colorSensorDisplay2.Foreground = Brushes.White;

            }
            else
            {
                colorSensorDisplay2.Background = Brushes.White;
                colorSensorDisplay2.Foreground = Brushes.Black;
            }

        }
    }
    public class AppData
    {
        public string distanceSensor1 { get; set; }
        public string distanceSensor2 { get; set; }
        public string distanceSensor3 { get; set; }
        public string distanceSensor4 { get; set; }
        public string colorSensor1 { get; set; }
        public string colorSensor2 { get; set; }

        public int motorOutput1 { get; set; }   

        public int motorOutput2 { get; set; }
        public AppData()
        {
            distanceSensor1 = string.Empty;
            distanceSensor2 = string.Empty;
            distanceSensor3 = string.Empty;
            distanceSensor4 = string.Empty;
            colorSensor1 = string.Empty;
            colorSensor2 = string.Empty;
            motorOutput1 = 0;
            motorOutput2 = 0;

        }
    }
}
