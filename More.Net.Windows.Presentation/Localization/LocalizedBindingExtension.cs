using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace More.Net.Windows.Localization
{
    [ContentProperty("ResourceKey")]
    [MarkupExtensionReturnType(typeof(Object))]
    public class LocalizedBindingExtension : MarkupExtension
    {

        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
