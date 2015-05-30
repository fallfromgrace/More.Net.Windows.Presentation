using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for CheckButton.xaml
    /// </summary>
    public partial class CheckButton : ToggleButton
    {
        /// <summary>
        /// 
        /// </summary>
        public ImageSource Glyph
        {
            get { return (ImageSource)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty GlyphProperty;

        static CheckButton()
        {
            GlyphProperty = DependencyPropertyFactory.Register(
                (CheckButton button) => button.Glyph);
        }

        public CheckButton()
        {
            InitializeComponent();
        }
    }
}
