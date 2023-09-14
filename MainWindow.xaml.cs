using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            
        }

        public void keyboardHandler(object slander, KeyEventArgs e)
        {
            if(motorCheckBox1.IsChecked == true)
            {
                switch (e.Key)
                {
                    case Key.W:
                        appData.motorOutput1 += 10;
                        motorOutputDisplay1.Value += 10;
                        break;
                    case Key.S:
                        appData.motorOutput1 -= 10;
                        motorOutputDisplay1.Value -= 10;
                        break;
                }
            }
            if (motorCheckBox2.IsChecked == true)
            {
                switch (e.Key)
                {
                    case Key.W:
                        appData.motorOutput2 += 10;
                        motorOutputDisplay2.Value += 10;
                        break;
                    case Key.S:
                        appData.motorOutput2 -= 10;
                        motorOutputDisplay2.Value -= 10;
                        break;
                }
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
            if(portSelectionBox.Text == "")
            {
                MessageBox.Show("Nie wyrbano portu szeregowego. Wybierz port i spróbuj jeszcze raz.");
            }
            else
            {
                serialPort.PortName = portSelectionBox.Text;
                serialPort.Open();
                selectedPort.Content = serialPort.PortName;
                connectionState.Content = "połączono";
                communicationState.Content = "nie nawiązano";
            }
            
        }
        private void startCommunication(object sender, RoutedEventArgs e)
        {

            
        }

        private static void getPacket2()
        {

        }
        

        private void getPacket(object sender, RoutedEventArgs e)
        {
            if (portSelectionBox.Text == "")
            {
                MessageBox.Show("Nie wyrbano portu szeregowego. Wybierz port i spróbuj jeszcze raz.");
            }
            else
            {
                serialPort.Write("?");
                rawData.Text = serialPort.ReadExisting();
                communicationState.Content = "pobrano Pakiet";

                if (rawData.Text != "")
                {
                    try
                    {
                        appData = JsonSerializer.Deserialize<AppData>(rawData.Text);

                        if (appData.distanceSensor1 == "0") distanceSensorDisplay1.Text = "off";
                        else distanceSensorDisplay1.Text = "on";

                        if (appData.distanceSensor2 == "0") distanceSensorDisplay2.Text = "off";
                        else distanceSensorDisplay2.Text = "on";

                        if (appData.distanceSensor3 == "0") distanceSensorDisplay3.Text = "off";
                        else distanceSensorDisplay3.Text = "on";

                        if (appData.distanceSensor4 == "0") distanceSensorDisplay4.Text = "off";
                        else distanceSensorDisplay4.Text = "on";

                        if (appData.colorSensor1 == "0") colorSensorDisplay1.Text = "off";
                        else colorSensorDisplay1.Text = "on";

                        if (appData.colorSensor2 == "0") colorSensorDisplay2.Text = "off";
                        else colorSensorDisplay2.Text = "on";


                    }
                    catch
                    {
                        outputDisplay.Text = "stravono pojedyczny pakiet";
                    }



                }


                if (appData.distanceSensor1 == "0")
                {
                    distanceSensorDisplay1.Background = Brushes.Gray;

                }
                else
                {
                    distanceSensorDisplay1.Background = Brushes.Green;
                }
                if (appData.distanceSensor2 == "0")
                {
                    distanceSensorDisplay2.Background = Brushes.Gray;
                }
                else
                {
                    distanceSensorDisplay2.Background = Brushes.Green;
                }
                if (appData.distanceSensor3 == "0")
                {
                    distanceSensorDisplay3.Background = Brushes.Gray;
                }
                else
                {
                    distanceSensorDisplay3.Background = Brushes.Green;
                }
                if (appData.distanceSensor4 == "0")
                {
                    distanceSensorDisplay4.Background = Brushes.Gray;
                }
                else
                {
                    distanceSensorDisplay4.Background = Brushes.Green;
                }

                if (appData.colorSensor1 == "0")
                {
                    colorSensorDisplay1.Background = Brushes.Black;
                    colorSensorDisplay1.Foreground = Brushes.White;

                }
                else
                {
                    colorSensorDisplay1.Background = Brushes.White;
                    colorSensorDisplay1.Foreground = Brushes.Black;
                }
                if (appData.colorSensor2 == "0")
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
