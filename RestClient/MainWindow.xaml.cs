using GalaSoft.MvvmLight.Messaging;
using RestClient.ViewModel;
using System;
using System.Windows;

namespace RestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new RestClientViewModel();

            Messenger.Default.Register<GenericMessage<string>>(this, "RestException", new Action<GenericMessage<string>>(a =>
            {
                MessageBox.Show(a.Content, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }));
        }
    }
}
