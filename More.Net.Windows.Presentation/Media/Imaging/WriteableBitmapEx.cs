using System.Runtime;
using System.Runtime.InteropServices;

namespace System.Windows.Media.Imaging
{
    internal static class NativeMethods
    {
        [TargetedPatchingOptOut("Internal method only, inlined across NGen boundaries for performance reasons")]
        internal static unsafe void CopyUnmanagedMemory(byte* srcPtr, int srcOffset, byte* dstPtr, int dstOffset, int count)
        {
            srcPtr += srcOffset;
            dstPtr += dstOffset;

            memcpy(dstPtr, srcPtr, count);
        }

        [TargetedPatchingOptOut("Internal method only, inlined across NGen boundaries for performance reasons")]
        internal static void SetUnmanagedMemory(IntPtr dst, int filler, int count)
        {
            memset(dst, filler, count);
        }

        // Win32 memory copy function
        //[DllImport("ntdll.dll")]
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern unsafe byte* memcpy(
            byte* dst,
            byte* src,
            int count);

        // Win32 memory set function
        //[DllImport("ntdll.dll")]
        //[DllImport("coredll.dll", EntryPoint = "memset", SetLastError = false)]
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern void memset(
            IntPtr dst,
            int filler,
            int count);
    }

    public static class WriteableBitmapContextExtensions
    {
        /// <summary>
        /// Gets a BitmapContext within which to perform nested IO operations on the bitmap
        /// </summary>
        /// <remarks>For WPF the BitmapContext will lock the bitmap. Call Dispose on the context to unlock</remarks>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static BitmapContext GetBitmapContext(this WriteableBitmap bmp)
        {
            return new BitmapContext(bmp);
        }

        /// <summary>
        /// Gets a BitmapContext within which to perform nested IO operations on the bitmap
        /// </summary>
        /// <remarks>For WPF the BitmapContext will lock the bitmap. Call Dispose on the context to unlock</remarks>
        /// <param name="bmp">The bitmap.</param>
        /// <param name="mode">The ReadWriteMode. If set to ReadOnly, the bitmap will not be invalidated on dispose of the context, else it will</param>
        /// <returns></returns>
        public static BitmapContext GetBitmapContext(this WriteableBitmap bmp, ReadWriteMode mode)
        {
            return new BitmapContext(bmp, mode);
        }
    }

    public unsafe static class WriteableBitmapExtensions
    {
        internal const int SizeOfArgb = 4;

        private static int ConvertColor(Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24)
                     | ((byte)((color.R * a) >> 8) << 16)
                     | ((byte)((color.G * a) >> 8) << 8)
                     | ((byte)((color.B * a) >> 8));
            return col;
        }

        /// <summary> 
        /// Draws an anti-aliased line, using an optimized version of Gupta-Sproull algorithm 
        /// From http://nokola.com/blog/post/2010/10/14/Anti-aliased-Lines-And-Optimizing-Code-for-Windows-Phone-7e28093First-Look.aspx
        /// <param name="context">The context containing the pixels as int RGBA value.</param>
        /// <param name="pixelWidth">The width of one scanline in the pixels array.</param>
        /// <param name="pixelHeight">The height of the bitmap.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// </summary> 
        public static void DrawLineAa(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, Color c)
        {
            using (var context = bmp.GetBitmapContext())
            {
                int pixelWidth = context.Width;
                int pixelHeight = context.Height;
                int color = ConvertColor(c);

                if ((x1 == x2) && (y1 == y2)) return; // edge case causing invDFloat to overflow, found by Shai Rubinshtein

                if (x1 < 1) x1 = 1;
                if (x1 > pixelWidth - 2) x1 = pixelWidth - 2;
                if (y1 < 1) y1 = 1;
                if (y1 > pixelHeight - 2) y1 = pixelHeight - 2;

                if (x2 < 1) x2 = 1;
                if (x2 > pixelWidth - 2) x2 = pixelWidth - 2;
                if (y2 < 1) y2 = 1;
                if (y2 > pixelHeight - 2) y2 = pixelHeight - 2;

                var addr = y1 * pixelWidth + x1;
                var dx = x2 - x1;
                var dy = y2 - y1;

                int du;
                int dv;
                int u;
                int v;
                int uincr;
                int vincr;

                // Extract color
                var a = (color >> 24) & 0xFF;
                var srb = (uint)(color & 0x00FF00FF);
                var sg = (uint)((color >> 8) & 0xFF);

                // By switching to (u,v), we combine all eight octants 
                int adx = dx, ady = dy;
                if (dx < 0) adx = -dx;
                if (dy < 0) ady = -dy;

                if (adx > ady)
                {
                    du = adx;
                    dv = ady;
                    u = x2;
                    v = y2;
                    uincr = 1;
                    vincr = pixelWidth;
                    if (dx < 0) uincr = -uincr;
                    if (dy < 0) vincr = -vincr;
                }
                else
                {
                    du = ady;
                    dv = adx;
                    u = y2;
                    v = x2;
                    uincr = pixelWidth;
                    vincr = 1;
                    if (dy < 0) uincr = -uincr;
                    if (dx < 0) vincr = -vincr;
                }

                var uend = u + du;
                var d = (dv << 1) - du;        // Initial value as in Bresenham's 
                var incrS = dv << 1;    // &#916;d for straight increments 
                var incrD = (dv - du) << 1;    // &#916;d for diagonal increments

                var invDFloat = 1.0 / (4.0 * Math.Sqrt(du * du + dv * dv));   // Precomputed inverse denominator 
                var invD2DuFloat = 0.75 - 2.0 * (du * invDFloat);   // Precomputed constant

                const int PRECISION_SHIFT = 10; // result distance should be from 0 to 1 << PRECISION_SHIFT, mapping to a range of 0..1 
                const int PRECISION_MULTIPLIER = 1 << PRECISION_SHIFT;
                var invD = (int)(invDFloat * PRECISION_MULTIPLIER);
                var invD2Du = (int)(invD2DuFloat * PRECISION_MULTIPLIER * a);
                var zeroDot75 = (int)(0.75 * PRECISION_MULTIPLIER * a);

                var invDMulAlpha = invD * a;
                var duMulInvD = du * invDMulAlpha; // used to help optimize twovdu * invD 
                var dMulInvD = d * invDMulAlpha; // used to help optimize twovdu * invD 
                //int twovdu = 0;    // Numerator of distance; starts at 0 
                var twovduMulInvD = 0; // since twovdu == 0 
                var incrSMulInvD = incrS * invDMulAlpha;
                var incrDMulInvD = incrD * invDMulAlpha;

                do
                {
                    AlphaBlendNormalOnPremultiplied(context, addr, (zeroDot75 - twovduMulInvD) >> PRECISION_SHIFT, srb, sg);
                    AlphaBlendNormalOnPremultiplied(context, addr + vincr, (invD2Du + twovduMulInvD) >> PRECISION_SHIFT, srb, sg);
                    AlphaBlendNormalOnPremultiplied(context, addr - vincr, (invD2Du - twovduMulInvD) >> PRECISION_SHIFT, srb, sg);

                    if (d < 0)
                    {
                        // choose straight (u direction) 
                        twovduMulInvD = dMulInvD + duMulInvD;
                        d += incrS;
                        dMulInvD += incrSMulInvD;
                    }
                    else
                    {
                        // choose diagonal (u+v direction) 
                        twovduMulInvD = dMulInvD - duMulInvD;
                        d += incrD;
                        dMulInvD += incrDMulInvD;
                        v++;
                        addr += vincr;
                    }
                    u++;
                    addr += uincr;
                } while (u < uend);
            }
        }

        /// <summary> 
        /// Blends a specific source color on top of a destination premultiplied color 
        /// </summary> 
        /// <param name="context">Array containing destination color</param> 
        /// <param name="index">Index of destination pixel</param> 
        /// <param name="sa">Source alpha (0..255)</param> 
        /// <param name="srb">Source non-premultiplied red and blue component in the format 0x00rr00bb</param> 
        /// <param name="sg">Source green component (0..255)</param> 
        private static void AlphaBlendNormalOnPremultiplied(BitmapContext context, int index, int sa, uint srb, uint sg)
        {
            var pixels = context.Pixels;
            var destPixel = (uint)pixels[index];

            var da = (destPixel >> 24);
            var dg = ((destPixel >> 8) & 0xff);
            var drb = destPixel & 0x00FF00FF;

            // blend with high-quality alpha and lower quality but faster 1-off RGBs 
            pixels[index] = (int)(
               ((sa + ((da * (255 - sa) * 0x8081) >> 23)) << 24) | // aplha 
               (((sg - dg) * sa + (dg << 8)) & 0xFFFFFF00) | // green 
               (((((srb - drb) * sa) >> 8) + drb) & 0x00FF00FF) // red and blue 
            );
        }

        /// <summary>
        /// Flips (reflects the image) eiter vertical or horizontal.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="flipMode">The flip mode.</param>
        /// <returns>A new WriteableBitmap that is a flipped version of the input.</returns>
        public static WriteableBitmap Flip(this WriteableBitmap bmp, FlipMode flipMode)
        {
            using (var context = bmp.GetBitmapContext())
            {
                // Use refs for faster access (really important!) speeds up a lot!
                var w = context.Width;
                var h = context.Height;
                var p = context.Pixels;
                var i = 0;
                WriteableBitmap result = null;

                if (flipMode == FlipMode.Horizontal)
                {
                    result = new WriteableBitmap(w, h, 96.0, 96.0, PixelFormats.Pbgra32, null);
                    using (var destContext = result.GetBitmapContext())
                    {
                        var rp = destContext.Pixels;
                        for (var y = h - 1; y >= 0; y--)
                        {
                            for (var x = 0; x < w; x++)
                            {
                                var srcInd = y * w + x;
                                rp[i] = p[srcInd];
                                i++;
                            }
                        }
                    }
                }
                else if (flipMode == FlipMode.Vertical)
                {
                    result = new WriteableBitmap(w, h, 96.0, 96.0, PixelFormats.Pbgra32, null);
                    using (var destContext = result.GetBitmapContext())
                    {
                        var rp = destContext.Pixels;
                        for (var y = 0; y < h; y++)
                        {
                            for (var x = w - 1; x >= 0; x--)
                            {
                                var srcInd = y * w + x;
                                rp[i] = p[srcInd];
                                i++;
                            }
                        }
                    }
                }

                return result;
            }
        }
    }

    /// <summary>
    /// The mode for flipping.
    /// </summary>
    public enum FlipMode
    {
        /// <summary>
        /// Flips the image vertical (around the center of the y-axis).
        /// </summary>
        Vertical,

        /// <summary>
        /// Flips the image horizontal (around the center of the x-axis).
        /// </summary>
        Horizontal
    }
}
