using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using PrApplication.Clients.Windows8.Core.Messeges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PRApplication.Hosting.AzureServiceReference.Controls
{
    
    public sealed partial class AddGuestControl : UserControl
    {
        FileOpenPicker filePicker;
        public AddGuestControl()
        {
            filePicker = new FileOpenPicker();
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");

            var Picfile = await filePicker.PickSingleFileAsync();

            if (Picfile != null)
            {
                TBPicName.Text = Picfile.Name;
                //byte[] picAsBytePixel = await ImageFileToByteArrayAsync(Picfile);//debug
                byte[] picAsByte = await ImageFileToByteArrayAsync(Picfile);

                //post to the MvxMessenger the selected image as byte array so it could save it as is to the DB.
                IMvxMessenger messenger = Mvx.Resolve<IMvxMessenger>();
                messenger.Publish(new ByteArrMessage(this, picAsByte));
            }

            
            
        }

        //public async Task<byte[]> ImageFileToByteArrayAsync(StorageFile file)
        //{
        //    IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
        //    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
        //    PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
        //    return pixelData.DetachPixelData();
        //}

        public async Task<byte[]> ImageFileToByteArrayAsync(StorageFile imageFile)
        {
            var inputStream = await imageFile.OpenSequentialReadAsync();
            var readStream = inputStream.AsStreamForRead();
            var buffer = new byte[readStream.Length];
            await readStream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }

    }
}
