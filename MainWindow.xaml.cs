using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace Quality_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort serialPort;
        public MainWindow()
        {
            serialPort = new SerialPort();
            InitializeComponent();
            InitializeSerialPorts();
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
            serialPort.Write("?");
            rawData.Text = serialPort.ReadExisting();
            communicationState.Content = "pobrano Pakiet";
        }
    }
}
