using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EZMetrology.Windows.Controls.Charts
{
    public class AxisYView : FrameworkElement
    {
        public Pen Pen
        {
            get;
            set;
        }

        public void Initialize()
        {
            children = new VisualCollection(this);
            children.Add(new DrawingVisual());

            Pen = new Pen(Brushes.LightGray, 2);
        }

        public AxisYView()
        {
            Initialize();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
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

        private void DrawAxis()
        {
            DrawingVisual visual = (DrawingVisual)children[0];
            DrawingContext ctx = visual.RenderOpen();
            Double x = ActualWidth * .9;
            ctx.DrawLine(Pen, new Point(x, 0), new Point(x, ActualHeight));
            ctx.Close();
        }

        private VisualCollection children;
    }
}
