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
using System.Resources;

namespace More.Net.Windows.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LocalizedPropertyBase
    {

        /// <summary>
        /// The convert to use to convert the retrieved resource to a value suitable
        /// for the property.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// The parameter to pass to the converter.
        /// </summary>
        public Object ConverterParameter { get; set; }

        /// <summary>
        /// Indicates if the object to the property belongs has been garbage collected.
        /// </summary>
        public Boolean IsAlive
        {
            get
            {
                return target.IsAlive;
            }
        }

        /// <summary>
        /// Indicates if the object is in design mode.
        /// </summary>
        public Boolean IsInDesignMode
        {
            get
            {
                DependencyObject target = Target;
                return 
                    target != null && 
                    DesignerProperties.GetIsInDesignMode(target);
            }
        }


        /// <summary>
        /// The object to which the property belongs.
        /// </summary>
        public DependencyObject Target
        {
            get
            {
                return (DependencyObject)target.Target;
            }
        }

        public LocalizedPropertyBase(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            this.target = new WeakReference(target);
        }

        public ResourceManager GetResourceManager()
        {
            DependencyObject target = Target;
            if (target == null)
                return null;

            if (target.CheckAccess() == true)
                return LocalizationScope.GetResourceManager(target);
            else
                return target.Dispatcher.Invoke(() => LocalizationScope.GetResourceManager(target));
        }

        public CultureInfo GetCulture()
        {
            DependencyObject target = Target;
            if (target == null)
                return null;

            if (target.CheckAccess() == true)
                return LocalizationScope.GetCulture(target);
            else
                return target.Dispatcher.Invoke(() => LocalizationScope.GetCulture(target));
        }

        public CultureInfo GetDesignCulture()
        {
            DependencyObject target = Target;
            if (target == null)
                return null;

            if (target.CheckAccess() == true)
                return LocalizationScope.GetDesignCulture(target);
            else
                return target.Dispatcher.Invoke(() => LocalizationScope.GetDesignCulture(target));
        }

        public abstract Object GetValue();
        public abstract void SetValue(Object value);
        public abstract Type GetValueType();

        private readonly WeakReference target;
    }

    public class LocalizedDependencyProperty : LocalizedPropertyBase
    {

        public override Object GetValue()
        {
            DependencyObject target = Target;
            if (target == null)
                return null;

            if (target.CheckAccess() == true)
                return target.GetValue(property);
            else
                return target.Dispatcher.Invoke(() => target.GetValue(property));
        }

        public override void SetValue(Object value)
        {
            DependencyObject target = Target;
            if (target == null)
                return;

            if (target.CheckAccess() == true)
                target.SetValue(property, value);
            else
                target.Dispatcher.Invoke(() => target.SetValue(property, value));
        }

        /// <summary>
        /// Gets the type of the value of the property.
        /// </summary>
        /// <returns>
        /// The type of the value of the property.
        /// </returns>
        public override Type GetValueType()
        {
            return property.PropertyType;
        }

        public LocalizedDependencyProperty(DependencyObject target, DependencyProperty property) : 
            base(target)
        {
            this.property = property;
        }

        private readonly DependencyProperty property;
    }
}
