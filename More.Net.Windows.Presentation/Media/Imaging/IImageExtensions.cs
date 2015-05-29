using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZMetrology.Media;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EZMetrology.Windows.Interop.Win32;
using System.Diagnostics.Contracts;

namespace EZMetrology.Windows.Media.Imaging
{
    /// <summary>
    /// 
    /// </summary>
    public static class IImageExtensions
    {
        /// <summary>
        /// Copies image data to the specified bitmap.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="writeableBitmap"></param>
        public static void CopyTo(
            this IBitmapImage image, WriteableBitmap writeableBitmap)
        {
            Kernel32.MoveMemory(
                writeableBitmap.BackBuffer,
                image.Data,
                Convert.ToUInt32(
                    writeableBitmap.PixelWidth *
                    writeableBitmap.PixelHeight *
                    (image.Format.BitsPerPixel / 8)));
        }

        /// <summary>
        /// Converts an image to a WriteableBitmap, copying image data.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static WriteableBitmap ToWriteableBitmap(this IBitmapImage image)
        {
            //Contract.Requires<ArgumentNullException>(image != null);

            WriteableBitmap writeableBitmap = new WriteableBitmap(
                image.Width, image.Height, 96, 96, PixelFormats.Gray8, null);
            image.CopyTo(writeableBitmap);
            return writeableBitmap;
        }
    }
}
