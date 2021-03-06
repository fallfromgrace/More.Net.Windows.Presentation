﻿using System;
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

namespace More.Net.Windows.Controls.Charts
{
    /// <summary>
    /// Interaction logic for SwiftChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl
    {
        /// <summary>
        /// Gets this chart's view.
        /// </summary>
        public ChartView ChartView
        {
            get { return chartView; }
        }

        public ChartControl()
        {
            InitializeComponent();
        }
    }
}
