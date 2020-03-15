using System;
using System.Collections.Generic;
using System.Diagnostics;
using NamedPipeSender.PipeServer;
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
using System.ComponentModel;

namespace NamedPipeSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public string InputText { get; set; }


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

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private PipeServer.PipeServer pipeServer;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.pipeServer = new PipeServer.PipeServer();
            StatusMessage = "Waiting to send message...";           
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            StatusMessage = "Sending Message";
       
            await Task.Run(() =>
            {
                Debug.WriteLine(InputText);
                pipeServer.SendMessageToClients(InputText);
               
            });

            StatusMessage = "Waiting to send message...";
        }
    }
}
