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
using System.Timers;
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
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer();
        public SerialPort serialPort;
        public AppData appData;
        public string rawDataTmp;
        public bool stateOfTransfer = false;


        public MainWindow()
        {
            appData = new AppData();
            serialPort = new SerialPort();
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(1000);
            gameTickTimer.IsEnabled = true;

        }

        private void GameTickTimer_Tick(object? sender, EventArgs e)
        {
            if(stateOfTransfer)
            {
                getPacket();
                refreshUi();
            }
            
        }

        public void keyboardHandler(object slander, KeyEventArgs e)
        {
            if (motorCheckBox1.IsChecked == true)
            {
                switch (e.Key)
                {
                    case Key.W:
                        appData.motorOutputs[0] += 10;
                        motorOutputDisplay1.Value += 10;
                        break;
                    case Key.S:
                        appData.motorOutputs[1] -= 10;
                        motorOutputDisplay1.Value -= 10;
                        break;
                }
            }
            if (motorCheckBox2.IsChecked == true)
            {
                switch (e.Key)
                {
                    case Key.W:
                        appData.motorOutputs[0] += 10;
                        motorOutputDisplay2.Value += 10;
                        break;
                    case Key.S:
                        appData.motorOutputs[1] -= 10;
                        motorOutputDisplay2.Value -= 10;
                        break;
                }
            }

        }

        private void openSerialPort(object sender, RoutedEventArgs e)
        {
            if (portSelectionBox.Text == "")
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
        private async void startCommunication(object sender, RoutedEventArgs e)
        {
            stateOfTransfer = true;
        }
        private void refreshUi()
        {
            var mapValue = new Dictionary<int, string>();
            mapValue[1] = "On";
            mapValue[0] = "Off";

            var mapColorValue = new Dictionary<int, SolidColorBrush>();
            mapColorValue[1] = Brushes.Green;
            mapColorValue[0] = Brushes.Gray;

            distanceSensorDisplay1.Text = mapValue[appData.distanceSensors[0]];
            distanceSensorDisplay2.Text = mapValue[appData.distanceSensors[1]];
            distanceSensorDisplay3.Text = mapValue[appData.distanceSensors[2]];
            distanceSensorDisplay4.Text = mapValue[appData.distanceSensors[3]];

            colorSensorDisplay1.Text = mapValue[appData.colorSensors[0]];
            colorSensorDisplay2.Text = mapValue[appData.colorSensors[1]];

            distanceSensorDisplay1.Background = mapColorValue[appData.distanceSensors[0]];
            distanceSensorDisplay2.Background = mapColorValue[appData.distanceSensors[1]];
            distanceSensorDisplay3.Background = mapColorValue[appData.distanceSensors[2]];
            distanceSensorDisplay4.Background = mapColorValue[appData.distanceSensors[3]];

            colorSensorDisplay1.Background = mapColorValue[appData.colorSensors[0]];
            colorSensorDisplay2.Background = mapColorValue[appData.colorSensors[1]];

            batteryDisplay.Content = $"{appData.battery}V / {(float)appData.battery/25*100} %";
            temperatureDisplay.Content = $"{appData.temperature} C*";

            

            rawData.Text = appData.rawData;

        }
        private void getPacket()
        {
            serialPort.Write("?");
            appData.rawData = serialPort.ReadExisting();
            if (appData.rawData != "")
            {
                try
                {
                    rawDataTmp = appData.rawData;
                    appData = JsonSerializer.Deserialize<AppData>(appData.rawData);
                    appData.rawData = rawDataTmp;
                }
                catch
                {
                    
                }
            }

        }
        private void getPacketClick(object sender, RoutedEventArgs e)
        {
            
            if (portSelectionBox.Text == "")
            {
                MessageBox.Show("Nie wyrbano portu szeregowego. Wybierz port i spróbuj jeszcze raz.");
            }
            else
            {
                getPacket();
                refreshUi();

            }
        }

        private void refreshSerialPorts(object sender, RoutedEventArgs e)
        {

            string[] ports = SerialPort.GetPortNames();
            
            if (!portSelectionBox.Items.IsEmpty)
            {
                portSelectionBox.Items.Clear();
            }

            foreach (var port in ports)
            {
                portSelectionBox.Items.Add(port);
            }

        }

        
    }

}
    public class AppData
    {
        public string rawData { get;set; }
        public int[] distanceSensors { get; set; }
        public int[] colorSensors { get; set; }
        public int[] motorOutputs { get; set; }
        public int battery { get; set; }
        public int temperature { get; set; }
    public AppData()
        {
            rawData = "";   
            distanceSensors = new int[4];
            colorSensors = new int[2];
            motorOutputs = new int[2];
        }
        


}

