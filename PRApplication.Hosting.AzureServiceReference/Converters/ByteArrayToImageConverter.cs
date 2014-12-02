using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace PRApplication.Hosting.AzureServiceReference.Converters
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !(value is byte[]))
                return null;

            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes((byte[])value);
                    writer.StoreAsync().GetResults();
                }
                BitmapImage image = new BitmapImage();
                image.DecodePixelHeight = 200;
                image.DecodePixelWidth = 150;
                image.SetSource(stream);
                return image;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)                                                                         
        {
            throw new NotImplementedException();
        }


    }
}
