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

namespace More.Net.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        static ImageView()
        {
            SourceProperty = DependencyPropertyFactory
                .Register((ImageView c) => c.Source, OnSourceChanged);
        }

        private static void OnSourceChanged(
            ImageView owner, 
            ImageSource oldValue, 
            ImageSource newValue)
        {
            owner.OnSourceChanged(oldValue, newValue);
        }

        protected virtual void OnSourceChanged(ImageSource oldValue, ImageSource newValue)
        {
            
        }

        public static readonly DependencyProperty SourceProperty;

        public ImageSource Source
        {
            get { return (ImageSource)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }
    }
}
