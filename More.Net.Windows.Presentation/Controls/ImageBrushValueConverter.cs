using More.Net.Windows.Data;
using System;
using System.Globalization;
using System.Windows.Media;

namespace More.Net.Windows.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageBrushValueConverter : IValueConverter<ImageSource, ImageBrush>
    {

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Convert((ImageSource)value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public ImageBrush Convert(ImageSource value)
        {
            return new ImageBrush(value);
        }

        public ImageSource ConvertBack(ImageBrush value)
        {
            throw new NotImplementedException();
        }
    }
}
