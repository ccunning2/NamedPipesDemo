using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
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

namespace NamedPipeConsumer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _StatusMessage;
        public string StatusMessage
        {
            get
            {
                return _StatusMessage;
            }
            set
            {
                _StatusMessage = value;
                RaisePropertyChanged("StatusMessage");

            }
        }

        private NamedPipeClientStream pipeClient;
     
        StreamReader sr;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StatusMessage = "Awaiting Message";

            this.pipeClient =
                       new NamedPipeClientStream(".", "testpipe", PipeDirection.In);
            this.sr = new StreamReader(pipeClient);
            ConnectToServer();

        }

        public async void ConnectToServer()
        {
            await Task.Run(() =>
           {


               // Connect to the pipe or wait until the pipe is available.
               Console.Write("Attempting to connect to pipe...");
             
               pipeClient.Connect();


               Console.WriteLine("Connected to pipe.");
               Console.WriteLine("There are currently {0} pipe server instances open.",
                  pipeClient.NumberOfServerInstances);

               // Display the read text to the console
               string temp;
               while ((temp = sr.ReadLine()) != null)
               {
                   StatusMessage = temp;
               }




           });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
