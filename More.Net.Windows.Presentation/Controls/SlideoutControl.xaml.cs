using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive;
using More.Net.Windows.Media.Animation;
using System.Reactive.Linq;

namespace More.Net.Windows.Controls
{
    /// <summary>
    /// Interaction logic for SlideoutControl.xaml
    /// </summary>
    public partial class SlideoutControl : ContentControl
    {
        #region Public Properties

        public Visibility DesignVisibility
        {
            get;
            set;
        }

        public Boolean IsTouchEnabled
        {
            get;
            set;
        }

        public Boolean IsMouseEnabled
        {
            get;
            set;
        }

        public Boolean IsExpanded
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Double TouchWidth
        {
            get { return (Double)GetValue(TouchWidthProperty); }
            set { SetValue(TouchWidthProperty, value); }
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TouchWidthProperty;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        static SlideoutControl()
        {
            TouchWidthProperty = DependencyPropertyFactory.Register(
                (SlideoutControl control) => control.TouchWidth, 50.0);
        }

        /// <summary>
        /// 
        /// </summary>
        public SlideoutControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Touch Events

        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            base.OnManipulationBoundaryFeedback(e);
        }

        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            TranslateTransform transform = this.RenderTransform as TranslateTransform;
            if (transform != null)
            {
                transform.X = -ActualWidth;
                this.Opacity = 1.0;
            }

            e.Mode = ManipulationModes.TranslateX;
            e.Handled = true;

            base.OnManipulationStarting(e);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            UIElement element = e.Source as UIElement;
            if (element != null)
            {
                TranslateTransform transform = this.RenderTransform as TranslateTransform;
                if (transform != null)
                {
                    transform.X += e.CumulativeManipulation.Translation.X;
                    if (transform.X > 0.0)
                        transform.X = 0.0;

                    e.Handled = true;
                }
            }

            base.OnManipulationDelta(e);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = 0.001;
            base.OnManipulationInertiaStarting(e);
        }

        protected override async void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            TranslateTransform transform = this.RenderTransform as TranslateTransform;
            if (transform != null)
            {
                if (transform.X < 0.0)
                {
                    await CollapseAnimatedAsync();
                    this.Opacity = 0.0;
                    transform.X = TouchWidth - ActualWidth;
                }
                else
                {
                    this.IsManipulationEnabled = false;
                }

                e.Handled = true;
            }
            base.OnManipulationCompleted(e);
        }

        private async void MainWindow_PreviewTouchDown(Object sender, TouchEventArgs e)
        {
            if (this.IsManipulationEnabled == false)
            {
                if (e.GetTouchPoint(this).Position.X > this.ActualWidth)
                {
                    await CollapseAnimatedAsync();
                    this.Opacity = 0.0;
                    TranslateTransform transform = this.RenderTransform as TranslateTransform;
                    if (transform != null)
                        transform.X = TouchWidth - ActualWidth;
                    this.IsManipulationEnabled = true;
                }
            }
        }

        #endregion

        #region Load Events

        private void ContentControl_Loaded(Object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                this.Opacity = 0.0;
                TranslateTransform transform = this.RenderTransform as TranslateTransform;
                if (transform != null)
                    transform.X = TouchWidth - ActualWidth;

                Application.Current.MainWindow.PreviewTouchDown += MainWindow_PreviewTouchDown;
            }
        }

        private void ContentControl_Unloaded(Object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Application.Current.MainWindow.PreviewTouchDown -= MainWindow_PreviewTouchDown;
            }
        }

        #endregion

        #region Behaviors

        private async Task CollapseAnimatedAsync()
        {
            TranslateTransform transform = this.RenderTransform as TranslateTransform;
            if (transform != null && IsAnimating == false)
            {
                Storyboard storyboard = StoryboardFactory.Create(
                    AnimationTimelineFactory.Create(
                        transform.X,
                        -ActualWidth,
                        new Duration(TimeSpan.FromMilliseconds(300)),
                        this,
                        control => ((TranslateTransform)control.RenderTransform).X));
                IsAnimating = true;
                await storyboard.AnimateAsync();
                IsAnimating = false;
            }
        }

        private Boolean IsAnimating
        {
            get;
            set;
        }

        #endregion
    }
}
