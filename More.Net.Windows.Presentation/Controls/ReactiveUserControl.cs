using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Reactive.Disposables;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;

namespace More.Net.Windows.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ViewModelT"></typeparam>
    [ContentProperty("AdditionalContent")]
    public class ReactiveUserControl<ViewModelT> : UserControl, IViewFor<ViewModelT>
        where ViewModelT : class
    {
        #region IViewFor<ViewModelT>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ViewModelT), 
            typeof(ReactiveUserControl<ViewModelT>), new PropertyMetadata(null));

        public ViewModelT ViewModel
        {
            get { return (ViewModelT)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return (ViewModelT)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion

        /// <summary>
        /// Gets or sets additional content for the UserControl. The additional
        /// content will have it's DataContext set to an instance of the ViewModel
        /// </summary>
        public UIElement AdditionalContent
        {
            get { return (UIElement)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(UIElement), 
            typeof(ReactiveUserControl<ViewModelT>), new PropertyMetadata(null));

        public IObservable<ViewModelT> ViewModelObservable()
        {
            return this.WhenAny(x => x.ViewModel, x => x.Value)
                .Where(v => v != null);
        }

        public UniformGrid DataContextHost { get; private set; }

        public ReactiveUserControl()
        {
            DataContextHost = new UniformGrid();
            this.Content = DataContextHost;

            // If AdditionalContent changes then we need to
            // add it to the visual tree
            this.WhenAny(p => p.AdditionalContent, p => p.Value)
                .Where(v => v != null)
                .Subscribe(v =>
                {
                    DataContextHost.Children.Clear();
                    DataContextHost.Children.Add(v);
                });

            // If the ViewModel changes we need to ensure the
            // AdditionalContent get's the correct DataContext
            this.WhenAny(x => x.ViewModel, x => x.Value)
                .BindTo(this, x => x.DataContextHost.DataContext);

            DesignUnsafeConstruct();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {

                DesignSafeConstruct();

            }
        }

        public virtual void DesignUnsafeConstruct()
        {
        }

        public virtual void DesignSafeConstruct()
        {
        }
    }

    public static class ReactiveUserControlMixins
    {
        // Dispose the previous disposable when the next is registered
        // and then dispose the last one when the UI is shut down
        public static void SeriallyDisposeWith(this IObservable<IDisposable> This, Control control)
        {
            var disposer = new SerialDisposable();
            This.Subscribe(d => disposer.Disposable = d);
            control.Unloaded += (s, e) => disposer.Dispose();
            control.Dispatcher.ShutdownStarted += (s, e) => disposer.Dispose();
        }

        // Dispose when unloaded or dispatcher is shut down
        public static void DisposeWith(this IDisposable This, Control contrl)
        {
            contrl.Unloaded += (s, e) => This.Dispose();
            contrl.Dispatcher.ShutdownStarted += (s, e) => This.Dispose();
        }
    }

}