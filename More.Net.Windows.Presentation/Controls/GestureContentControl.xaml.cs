using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EZMetrology.Windows.Controls
{
    /// <summary>
    /// Interaction logic for GestureContentControl.xaml
    /// </summary>
    public partial class GestureContentControl : ScrollViewer
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsMouseEnabled
        {
            get { return (Boolean)GetValue(IsMouseEnabledProperty); }
            set { SetValue(IsMouseEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsPanEnabled
        {
            get { return (Boolean)GetValue(IsPanEnabledProperty); }
            set { SetValue(IsPanEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsRotateEnabled
        {
            get { return (Boolean)GetValue(IsRotateEnabledProperty); }
            set { SetValue(IsRotateEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsTouchEnabled
        {
            get { return (Boolean)GetValue(IsTouchEnabledProperty); }
            set { SetValue(IsTouchEnabledProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsZoomEnabled
        {
            get { return (Boolean)GetValue(IsZoomEnabledProperty); }
            set { SetValue(IsZoomEnabledProperty, value); }
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsMouseEnabledProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsPanEnabledProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsRotateEnabledProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsTouchEnabledProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsZoomEnabledProperty;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        static GestureContentControl()
        {
            IsManipulationEnabledProperty.OverrideMetadata(
                (GestureContentControl control) => control.IsManipulationEnabled,
                (control, oldValue, newValue) => control.IsTouchEnabled = newValue);

            IsMouseEnabledProperty = DependencyPropertyFactory.Register(
                (GestureContentControl control) => control.IsMouseEnabled);

            IsPanEnabledProperty = DependencyPropertyFactory.Register(
                (GestureContentControl control) => control.IsPanEnabled);

            IsRotateEnabledProperty = DependencyPropertyFactory.Register(
                (GestureContentControl control) => control.IsRotateEnabled);

            IsTouchEnabledProperty = DependencyPropertyFactory.Register(
                (GestureContentControl control) => control.IsTouchEnabled,
                (control, oldValue, newValue) => control.IsManipulationEnabled = newValue);

            IsZoomEnabledProperty = DependencyPropertyFactory.Register(
                (GestureContentControl control) => control.IsZoomEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        public GestureContentControl()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void ResetContentView()
        {
            UIElement element = Content as UIElement;
            if (element != null)
                element.RenderTransform = new MatrixTransform(Matrix.Identity);
        }

        #region Touch Events

        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            //ManipulationModes mode = ManipulationModes.None;

            //if (IsPanEnabled)
            //    mode |= ManipulationModes.Translate;
            //if (IsRotateEnabled)
            //    mode |= ManipulationModes.Rotate;
            //if (IsZoomEnabled)
            //    mode |= ManipulationModes.Scale;

            //e.Mode = mode;
            //e.Handled = true;

            base.OnManipulationStarting(e);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            //UIElement element = Content as UIElement;
            //FrameworkElement container = e.ManipulationContainer as FrameworkElement;
            //if (element != null && container != null)
            //{
            //    MatrixTransform xform = element.RenderTransform as MatrixTransform;
            //    Matrix matrix = xform.Matrix;
            //    ManipulationDelta delta = e.DeltaManipulation;
            //    Point center = e.ManipulationOrigin;

            //    matrix.ScaleAt(
            //        delta.Scale.X, delta.Scale.Y, center.X, center.Y);

            //    //Rect bounds = newTransform
            //    //    .TransformBounds(VisualTreeHelper.GetDrawing(element).Bounds);

            //    element.RenderTransform = new MatrixTransform(matrix);
            //}

            base.OnManipulationDelta(e);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            //e.TranslationBehavior.DesiredDeceleration = 0.02;
            //e.ExpansionBehavior.DesiredDeceleration = 0.02;
            //e.Handled = true;
            base.OnManipulationInertiaStarting(e);
        }

        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            //UIElement element = Content as UIElement;
            //if (element != null)
            //{
            //    MatrixTransform xform = element.RenderTransform as MatrixTransform;
            //    Matrix matrix = xform.Matrix;
            //    //matrix.Translate(
            //    //    -e.BoundaryFeedback.Translation.X, -e.BoundaryFeedback.Translation.Y);
            //    //matrix.ScaleAt(-e.BoundaryFeedback.Scale.X, -e.BoundaryFeedback.Scale.Y, e.m
            //    element.RenderTransform = new MatrixTransform(matrix);
            //    e.Handled = true;
            //}
            base.OnManipulationBoundaryFeedback(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
        }

        private Boolean CalculateOvershoot(UIElement element, FrameworkElement container, out Vector overshoot)
        {
            // Get axis aligned element bounds
            var elementBounds = element.RenderTransform
                .TransformBounds(VisualTreeHelper.GetDrawing(element).Bounds);

            //double extraX = 0.0, extraY = 0.0;
            overshoot = new Vector();

            // Calculate overshoot.  
            if (elementBounds.Left < 0)
                overshoot.X = elementBounds.Left;
            else if (elementBounds.Right > container.ActualWidth)
                overshoot.X = elementBounds.Right - container.ActualWidth;

            if (elementBounds.Top < 0)
                overshoot.Y = elementBounds.Top;
            else if (elementBounds.Bottom > container.ActualHeight)
                overshoot.Y = elementBounds.Bottom - container.ActualHeight;

            // Return false if Overshoot is empty; otherwsie, return true.
            return !Vector.Equals(overshoot, new Vector());
        }

        #endregion

        #region Mouse Events

        #endregion
    }
}
