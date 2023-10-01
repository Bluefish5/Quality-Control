using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
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
        public bool isPortSelected = false;


        public MainWindow()
        {
            appData = new AppData();
            serialPort = new SerialPort();
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(500);
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
                serialPort.BaudRate = 115200;
                serialPort.Open();
                selectedPort.Content = serialPort.PortName;
                connectionState.Content = "połączono";
                communicationState.Content = "nie nawiązano";
                isPortSelected = true;
            }

        }
        private async void startCommunication(object sender, RoutedEventArgs e)
        {
            if (!stateOfTransfer)
            {
                stateOfTransfer = true;
                startCommunicationButton.Content = "Wyłącz komunikacje";
                communicationState.Content = "transmisja danych";
            }
            else
            {
                stateOfTransfer = false;
                startCommunicationButton.Content = "Włącz komunikacje";
                communicationState.Content = "zatrzymano transmisje";
            }
            
        }
        private void refreshUi()
        {
            var mapValue = new Dictionary<int, string>();
            mapValue[1] = "On";
            mapValue[0] = "Off";

            var mapColorValue = new Dictionary<int, SolidColorBrush>();
            
            mapColorValue[1] = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 115, 242, 128)); ;
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

            leftEncoderDisplay.angle = appData.temperature;
            rightEncoderDisplay.angle = appData.temperature;
            leftEncoderDisplay.updateArrowAngle();
            rightEncoderDisplay.updateArrowAngle();




            rawData.Text = appData.rawData;

        }
        private void getPacket()
        {
            if (isPortSelected)
            {
                serialPort.Write("ping\n");


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
            else
            {
                stateOfTransfer = false;
                MessageBox.Show("Nie wyrbano portu szeregowego. Wybierz port i spróbuj jeszcze raz.");
            }

        }
        private void getPacketClick(object sender, RoutedEventArgs e)
        {
            
            if (!isPortSelected)
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

        private void vescOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("vescOn\n");
        }
        private void vescOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("vescOff\n");
        }
        private void analogOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("analogOn\n");
        }
        private void analogOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("analogOff\n");
        }
        private void starterOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("starterOn\n");
        }
        private void starterOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("starterOff\n");
        }
        private void lineOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("lineOn\n");
        }
        private void lineOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("lineOff\n");
        }
        private void digitalOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("digitalOn\n");
        }
        private void digitalOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("digitalOff\n");
        }
        private void starterJpOn(object sender, RoutedEventArgs e)
        {
            serialPort.Write("starterJpOn\n");
        }
        private void starterJpOff(object sender, RoutedEventArgs e)
        {
            serialPort.Write("starterJpOff\n");
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

