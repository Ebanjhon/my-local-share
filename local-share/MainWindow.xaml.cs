using local_share.Models;
using local_share.Utils;
using local_share.ViewModels;
using System.Data.Common;
using System.IO;
using System.Windows;

namespace local_share
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
            string ip = Utils.Utils.GetLocalIPAddress();
            string localhost = "http://" + ip + ":5000/";
            TxtUrl.Text = localhost;
            QrImage.Source = Utils.Utils.GenerateQr();

            Loaded += async (_, __) =>
            {
                string ip = Utils.Utils.GetLocalIPAddress();
                string localhost = "http://" + ip + ":5000/";

                TxtUrl.Text = localhost;
                QrImage.Source = Utils.Utils.GenerateQr();

                await _vm.Start(localhost); // 🔥 BẮT BUỘC
            };
        }

    }
}