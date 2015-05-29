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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EZMetrology.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ToastNotificationControl.xaml
    /// </summary>
    public partial class ToastNotificationControl : UserControl
    {
        public static readonly RoutedEvent CloseButtonClickedEvent = EventManager
            .RegisterRoutedEvent("CloseButtonClicked", RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), typeof(ToastNotificationControl));

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler CloseButtonClicked
        {
            add { AddHandler(CloseButtonClickedEvent, value); }
            remove { RemoveHandler(CloseButtonClickedEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ToastNotificationControl()
        {
            InitializeComponent();
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseButtonClickedEvent));
        }



        Window ParentWindow { get; set; }
        public void ShowDialogBox(Window parentWindow, string message)
        {
            ParentWindow = parentWindow;
            popupLabel.Content = message;
            Storyboard StatusFader = (Storyboard)Resources["StatusFader"];
            ParentWindow.IsEnabled = false;
            FrameworkElement root = (FrameworkElement)ParentWindow.Content;
            this.Height = root.ActualHeight;
            this.Width = root.ActualWidth;
            //TODO: Determine why there is 1 pixel extra whitespace.
            //Tried playing with Margins and Alignment to no avail.
            popup.Height = root.ActualHeight + 1;
            popup.Width = root.ActualWidth + 1;
            popup.IsOpen = true;
            StatusFader.Begin(popupBackground);
        }

        void StatusFader_Completed(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            ParentWindow.IsEnabled = true;
        }
    }
}
