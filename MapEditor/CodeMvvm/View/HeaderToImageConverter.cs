using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media;
using System.IO;
using System.Drawing.Imaging;

namespace WPF_Explorer_Tree
{
    #region HeaderToImageConverter

    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance =
            new HeaderToImageConverter();

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            //Console.WriteLine(value as string);
            if ((value as string).Length == 3)
            {
                Uri uri = new Uri
                ("pack://application:,,,/View/Icons/hard_drive.png");
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
            else if ((value as string).Contains(".png"))
            {
                string s = (value as string);
                var icon = Icon.ExtractAssociatedIcon(s);
                var bmp = icon.ToBitmap();
                BitmapImage source = BitMapConverter.ToBitmapImage(bmp);
                return source;

            }
            {
                Uri uri = new Uri("pack://application:,,,/View/Icons/folder.ico");
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }

    }

    public static class BitMapConverter
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }


    #endregion // HeaderToImageConverter
}
