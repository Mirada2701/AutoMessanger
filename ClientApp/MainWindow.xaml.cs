using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverPoint;
        UdpClient client;
        ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient();
            model = new ViewModel();
            serverPoint = new IPEndPoint(IPAddress.Parse(model.Address), model.Port);
            this.DataContext = model;
        }

        private async void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            Message msg = new Message(model.MsgText);
            model.AddMessage(msg);
            byte[] data = Encoding.UTF8.GetBytes(model.MsgText);
            await client.SendAsync(data, serverPoint);

            while (true)
            {
                var result = await client.ReceiveAsync();
                string response = Encoding.UTF8.GetString(result.Buffer);
                model.AddMessage(new Message(response));
            }
           
        }        
    }
    public class ViewModel
    {
        public string MsgText { get; set; }
        public string Address { get; set; }
        public short Port { get; set; }
        private ObservableCollection<Message> messages;
        public IEnumerable<Message> Messages => messages;
        public ViewModel()
        {
            messages = new ObservableCollection<Message>();
            Address = ConfigurationManager.AppSettings["server_ip"]!;
            Port = short.Parse(ConfigurationManager.AppSettings["server_port"]!);
        }
        public void AddMessage(Message ms)
        {
            messages.Add(ms);
        }
    }
    public class Message
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public Message(string text)
        {
            this.Text = text;
            Time = DateTime.Now;
        }
        public override string ToString()
        {
            return $"You : {Text}\t\t{Time.ToShortTimeString()}";
        }
    }
}