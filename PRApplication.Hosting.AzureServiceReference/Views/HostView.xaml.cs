
using Cirrious.MvvmCross.WindowsCommon.Views;
using OnBarcode.Barcode.BarcodeScanner;
using PrApplication.Clients.Windows8.Core.PrServiceReference;
using PRApplication.Hosting.AzureServiceReference.QrCodeServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PRApplication.Hosting.AzureServiceReference.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HostView : MvxWindowsPage
    {
        public HostView()
        {
            this.InitializeComponent();
        }

        private async void ReadQrCode_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI image = new CameraCaptureUI();
            StorageFile file = await image.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                var service = new Service1Client();
                string guestQrCode = await service.ReadQRCodeAsync(file.Path);
                txtSearch.Text = guestQrCode;
            }
        }
    }
}
