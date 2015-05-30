using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Reflection;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace More.Net.Windows.Localization
{
    /// <summary>
    /// Enables localization of properties.
    /// </summary>
    [ContentProperty("Key")]
    [MarkupExtensionReturnType(typeof(Object))]
    public class LocalizedResourceExtension : MarkupExtension
    {
        /// <summary>
        /// The resource key to use to format the values.
        /// </summary>
        public String Key
        {
            get { return key; }
            set { this.key = value; }
        }

        /// <summary>
        /// The converter to use to convert the value before it is assigned to the property.
        /// </summary>
        public IValueConverter Converter
        {
            get;
            set;
        }

        /// <summary>
        /// The parameter to pass to the converter.
        /// </summary>
        public Object ConverterParameter
        {
            get;
            set;
        }

        /// <summary>
        /// The parameter to pass to the converter.
        /// </summary>
        public CultureInfo ConverterCulture
        {
            get;
            set;
        }

        public LocalizedResourceExtension()
        {

        }

        public LocalizedResourceExtension(String key)
        {
            this.key = key;
        }
        private String key;

        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget service = serviceProvider
                .GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (service == null)
            {
                return null;
            }

            if (service.TargetObject is DependencyObject)
            {
                LocalizedDependencyProperty property = null;

                if (service.TargetProperty is DependencyProperty)
                {
                    property = new LocalizedDependencyProperty(
                        (DependencyObject)service.TargetObject,
                        (DependencyProperty)service.TargetProperty);
                }
                else if (service.TargetProperty is PropertyInfo)
                {
                    //property = new LocalizedNonDependencyProperty(
                    //    (DependencyObject)service.TargetObject,
                    //    (PropertyInfo)service.TargetProperty
                    //    );
                }
                else
                {
                    return null;
                }

                property.Converter = Converter;

                property.ConverterParameter = ConverterParameter;

                var localizedValue = new LocalizedResourceValue(property, key);

                if (localizedValue == null)
                {
                    return null;
                }

                LocalizationScope.AddLocalizedValue(localizedValue);

                if (property.IsInDesignMode)
                {
                    // At design time VS designer does not set the parent of any control
                    // before its properties are set. For this reason the correct values
                    // of inherited attached properties cannot be obtained.
                    // Therefore, to display the correct localized value it must be updated
                    // later ater the parrent of the control has been set.

                    ((DependencyObject)service.TargetObject).Dispatcher.BeginInvoke(
                        new SendOrPostCallback(x => ((LocalizedResourceValue)x).UpdateValue()),
                        DispatcherPriority.ApplicationIdle,
                        localizedValue);
                }

                return localizedValue.GetValue();
            }
            else if (
                service.TargetProperty is DependencyProperty || 
                service.TargetProperty is PropertyInfo)
            {
                // The extension is used in a template

                return this;
            }
            else
            {
                return null;
            }
        }
    }
}
