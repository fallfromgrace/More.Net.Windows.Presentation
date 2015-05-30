using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;

namespace More.Net.Windows.Controls.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChartView : FrameworkElement
    {
        public Double MaxValue
        {
            get;
            set;
        }

        public Double MinValue
        {
            get;
            set;
        }

        public Double MaxArgument
        {
            get;
            set;
        }

        public Double MinArgument
        {
            get;
            set;
        }

        public String ChartTitle
        {
            get { return chartTitle; }
            set
            {
                if (ChartTitle != value)
                {
                    chartTitle = value;
                }
            }
        }

        public String AxisXTitle
        {
            get { return axisXTitle; }
            set
            {
                if (AxisXTitle != value)
                {
                    axisXTitle = value;
                }
            }
        }

        public String AxisYTitle
        {
            get { return axisYTitle; }
            set
            {
                if (AxisYTitle != value)
                {
                    axisYTitle = value;
                }
            }
        }

        public Rect ChartBounds
        {
            get { return chartBounds; }
            set
            {
                chartBounds = value;
            }
        }

        public Rect ChartSeriesBounds
        {
            get { return chartSeriesBounds; }
            set
            {
                chartSeriesBounds = value;
            }
        }

        public Double[] Series
        {
            get { return series; }
            set
            {
                if (Series != value)
                {
                    series = value;
                    OnSeriesChanged();
                }
            }
        }

        private void OnSeriesChanged()
        {
            if (Series != null)
            {
                DrawingContext seriesLayer = ((DrawingVisual)children[1]).RenderOpen();
                DrawSeries(seriesLayer);
                seriesLayer.Close();
            }
        }

        public ChartView()
        {
            Initialize();
        }

        public void Initialize()
        {
            axisXTitle = "";
            axisYTitle = "";
            chartTitle = "";
            chartBounds = new Rect(0, 0, 400, 200);
            children = new VisualCollection(this);
            children.Add(new DrawingVisual());
            children.Add(new DrawingVisual());
            children.Add(new DrawingVisual());
            chartBounds = new Rect();
            maxChartOffset = new Rect(40, 40, 50, 80);
            maxChartSeriesOffset = new Rect(10, 10, 20, 20);
            series = new Double[0];
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            SetBounds();
            DrawChartElements();
            base.OnRenderSizeChanged(sizeInfo);
        }

        private void SetBounds()
        {
            ChartBounds = GetBounds(new Rect(0, 0, ActualWidth, ActualHeight), maxChartOffset);
            ChartSeriesBounds = GetBounds(ChartBounds, maxChartSeriesOffset);
        }

        private Rect GetBounds(Rect bounds, Rect offset)
        {
            Rect chartBounds = new Rect();
            Double offsetFactorX = Math.Min(bounds.Width / offset.Width, 1d);
            Double offsetFactorY = Math.Min(bounds.Height / offset.Height, 1d);
            chartBounds.X = bounds.X + offset.X * offsetFactorX;
            chartBounds.Y = bounds.Y + offset.Y * offsetFactorY;
            chartBounds.Width = bounds.Width - offset.Width * offsetFactorX;
            chartBounds.Height = bounds.Height - offset.Height * offsetFactorY;

            return chartBounds;
        }

        private void DrawChartBackground(DrawingContext backgroundLayer)
        {
            Brush backgroundBrush = new SolidColorBrush(Color.FromArgb(255, 47, 47, 47));
            backgroundLayer.DrawRectangle(backgroundBrush, null, ChartBounds);
        }

        private void DrawChartElements()
        {
            DrawingContext backgroundLayer = ((DrawingVisual)children[0]).RenderOpen();
            DrawingContext seriesLayer = ((DrawingVisual)children[1]).RenderOpen();
            DrawingContext overlayLayer = ((DrawingVisual)children[2]).RenderOpen();
            
            DrawChartBackground(backgroundLayer);
            DrawSeries(seriesLayer);
            DrawAxisX(overlayLayer);
            DrawAxisY(overlayLayer);
            DrawChartTitle(overlayLayer);

            backgroundLayer.Close();
            seriesLayer.Close();
            overlayLayer.Close();
        }

        private Double tickLength = 4;
        private Double tickThickness = 2;
        private Double tickInc = 30;
        private Double lineThickness = 2;
        private Color backColor = Color.FromArgb(255, 47, 47, 47);
        private Color lineColor = Color.FromArgb(255, 95, 95, 95);
        private Typeface typeFace = new Typeface("Tahoma");

        private void DrawAxisX(DrawingContext overlayLayer)
        {
            Double y = ChartBounds.Y + ChartBounds.Height;
            overlayLayer.DrawLine(
                new Pen(new SolidColorBrush(lineColor), lineThickness),
                new Point(ChartBounds.X - 1, y),
                new Point(ChartBounds.X + ChartBounds.Width, y));

            for (Double x = ChartSeriesBounds.X; 
                x < ChartSeriesBounds.X + ChartSeriesBounds.Width; 
                x += tickInc)
            {
                overlayLayer.DrawLine(
                    new Pen(new SolidColorBrush(lineColor), tickThickness),
                    new Point(x, y),
                    new Point(x, y + tickLength));
                /*
                FormattedText formattedTickLabel = new FormattedText(
                    "1", Thread.CurrentThread.CurrentUICulture, 
                    FlowDirection.LeftToRight, typeFace, 10, Brushes.White);
                formattedTickLabel.TextAlignment = TextAlignment.Center;
                Point tickLabelLocation = new Point(x, y + tickLength);
                overlayLayer.DrawText(formattedTickLabel, tickLabelLocation);
                */
            }

            FormattedText formattedTitle = new FormattedText(
                AxisXTitle, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                typeFace, 16, whiteBrush);
            Point location = new Point(
                ChartBounds.X + ChartBounds.Width / 2,
                (ChartBounds.Y + ChartBounds.Height + ActualHeight - formattedTitle.Height / 2) / 2);
            formattedTitle.TextAlignment = TextAlignment.Center;

            overlayLayer.DrawText(formattedTitle, location);
        }

        private void DrawAxisY(DrawingContext overlayLayer)
        {
            Double y;
            overlayLayer.DrawLine(
                new Pen(new SolidColorBrush(lineColor), lineThickness),
                new Point(ChartBounds.X, ChartBounds.Y + ChartBounds.Height + 1),
                new Point(ChartBounds.X, ChartBounds.Y));

            for (y = ChartSeriesBounds.Y + ChartSeriesBounds.Height;
                y > ChartSeriesBounds.Y;
                y -= tickInc)
            {
                overlayLayer.DrawLine(
                    new Pen(new SolidColorBrush(lineColor), tickThickness),
                    new Point(ChartBounds.X - tickLength, y),
                    new Point(ChartBounds.X, y));
                /*
                overlayLayer.PushTransform(new RotateTransform(270));
                FormattedText formattedTickLabel = new FormattedText(
                    "1", Thread.CurrentThread.CurrentUICulture,
                    FlowDirection.LeftToRight, typeFace, 10, Brushes.White);
                formattedTickLabel.TextAlignment = TextAlignment.Center;
                Point tickLabelLocation = new Point(-y, ChartBounds.X - 2 * tickLength);
                overlayLayer.DrawText(formattedTickLabel, tickLabelLocation);
                overlayLayer.Pop();
                */
            }

            FormattedText minLabel = new FormattedText(
                MinValue.ToString(), Thread.CurrentThread.CurrentUICulture,
                FlowDirection.LeftToRight, typeFace, 10, whiteBrush);
            minLabel.TextAlignment = TextAlignment.Center;
            Point minLabelLocation = new Point(
                -(ChartSeriesBounds.Y + ChartSeriesBounds.Height),
                ChartBounds.X - tickLength - minLabel.Height);

            FormattedText maxLabel = new FormattedText(
                MaxValue.ToString(), Thread.CurrentThread.CurrentUICulture,
                FlowDirection.LeftToRight, typeFace, 10, whiteBrush);
            maxLabel.TextAlignment = TextAlignment.Center;
            Point maxLabelLocation = new Point(
                -(y + tickInc),
                ChartBounds.X - tickLength - minLabel.Height);

            FormattedText titleLabel = new FormattedText(
                AxisYTitle, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                typeFace, 16, whiteBrush);
            Point location = new Point(
                -(ChartBounds.Y + ChartBounds.Height / 2),
                ChartBounds.X - 2 * titleLabel.Height);
            titleLabel.TextAlignment = TextAlignment.Center;

            overlayLayer.PushTransform(new RotateTransform(270));
            overlayLayer.DrawText(minLabel, minLabelLocation);
            overlayLayer.DrawText(maxLabel, maxLabelLocation);
            overlayLayer.DrawText(titleLabel, location);
            overlayLayer.Pop();
        }

        private static readonly Brush whiteBrush = new SolidColorBrush(Color.FromArgb(191, 255, 255, 255));
        private static readonly Brush greenBrush = new SolidColorBrush(Color.FromArgb(255, 207, 255, 0));

        private void DrawChartTitle(DrawingContext overlayLayer)
        {
            FormattedText formattedTitle = new FormattedText(
                ChartTitle, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                typeFace, 20, greenBrush);
            Point location = new Point(
                ChartBounds.X + ChartBounds.Width / 2, 
                (ChartBounds.Y - formattedTitle.Height) / 2);
            formattedTitle.TextAlignment = TextAlignment.Center;
            overlayLayer.DrawText(formattedTitle, location);
        }

        private void DrawSeries(DrawingContext seriesLayer)
        {
            if (Series != null && Series.Length > 1)
            {
                double pixelInc = 2.0;
                double decimationFactor = Series.Length / ChartSeriesBounds.Width * pixelInc;
                if (decimationFactor < 1.0)
                {
                    pixelInc /= decimationFactor;
                    decimationFactor = 1.0;
                }
                WriteableBitmap bmp = new WriteableBitmap(
                    (int)ChartSeriesBounds.Width, (int)ChartSeriesBounds.Height,
                    96d, 96d, PixelFormats.Pbgra32, null);

                double xi = pixelInc;
                int x1 = 0;
                int y1 = PointToPixel(Series[0]);
                for (double yi = decimationFactor; yi < Series.Length; yi += decimationFactor)
                {
                    int x2 = (int)xi;
                    int y2 = PointToPixel(Series[(int)yi]);

                    var L = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                    int offsetPixels = 2;
                    int x1p = (int)(x1 + offsetPixels * (y2 - y1) / L);
                    int x2p = (int)(x2 + offsetPixels * (y2 - y1) / L);
                    int y1p = (int)(y1 + offsetPixels * (x1 - x2) / L);
                    int y2p = (int)(y2 + offsetPixels * (x1 - x2) / L);

                    bmp.DrawLineAa(x1, y1, x2, y2, Colors.YellowGreen);
                    x1 = x2;
                    y1 = y2;
                    xi += pixelInc;
                };

                //seriesLayer.PushTransform(new MatrixTransform(1, 0, 0, -1, 0, 1.5 * bmp.Height));
                seriesLayer.DrawImage(bmp.Flip(FlipMode.Horizontal), ChartSeriesBounds);
                //seriesLayer.Pop();
            }
        }

        private Int32 PointToPixel(Double val)
        {
            return (int)((val - MinValue) / (MaxValue - MinValue) * ChartSeriesBounds.Height);
        }
        
        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return children[index];
        }

        private String axisXTitle;
        private String axisYTitle;
        private String chartTitle;
        private Rect chartBounds;
        private Rect chartSeriesBounds;
        private VisualCollection children;
        private Rect maxChartOffset;
        private Rect maxChartSeriesOffset;
        private Double[] series;
    }
}
