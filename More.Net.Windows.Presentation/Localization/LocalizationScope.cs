using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Resources;
using System.ComponentModel;
using System.Windows.Markup;
using System.Threading;

// Register the types in the Microsoft's default namespaces
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "More.Net.Windows.Localization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "More.Net.Windows.Localization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "More.Net.Windows.Localization")]

namespace More.Net.Windows.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalizationScope
    {
        /// <summary>
        /// The <see cref="CultureInfo"/> according to which values are formatted.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty CultureProperty;

        /// <summary>
        /// 
        /// </summary>
        [DesignOnly(true)]
        public static DependencyProperty DesignCultureProperty;

        /// <summary>
        /// The <see cref="CultureInfo"/> used to retrieve resources from <see cref="ResourceManager"/>.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty UICultureProperty;

        /// <summary>
        /// The resource manager to use to retrieve resources.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty ResourceManagerProperty;

        static LocalizationScope()
        {
            CultureProperty = DependencyProperty.RegisterAttached(
                "Culture",
                typeof(CultureInfo),
                typeof(LocalizationScope),
                new FrameworkPropertyMetadata(
                    null, 
                    FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback((sender, e) => 
                        UpdateValues(sender))));

            UICultureProperty = DependencyProperty.RegisterAttached(
                "UICulture",
                typeof(CultureInfo),
                typeof(LocalizationScope),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.None,
                    OnCurrentUICultureChanged));

            DesignCultureProperty = DependencyProperty.RegisterAttached(
                "DesignCulture",
                typeof(CultureInfo),
                typeof(LocalizationScope),
                new FrameworkPropertyMetadata(
                    null, 
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnCurrentCultureChanged));

            ResourceManagerProperty = DependencyProperty.RegisterAttached(
                "ResourceManager",
                typeof(ResourceManager),
                typeof(LocalizationScope),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        }

        public static CultureInfo GetCulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(CultureProperty);
        }

        public static void SetCulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(CultureProperty, value);
            UpdateValues();
        }

        [DesignOnly(true)]
        public static CultureInfo GetDesignCulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(DesignCultureProperty);
        }

        [DesignOnly(true)]
        public static void SetDesignCulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(DesignCultureProperty, value);
            UpdateValues();
        }

        public static CultureInfo GetUICulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(UICultureProperty);
        }

        public static void SetUICulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(UICultureProperty, value);
            UpdateValues();
        }

        public static ResourceManager GetResourceManager(DependencyObject obj)
        {
            return (ResourceManager)obj.GetValue(ResourceManagerProperty);
        }

        public static void SetResourceManager(DependencyObject obj, ResourceManager value)
        {
            obj.SetValue(ResourceManagerProperty, value);
        }

        public static void AddLocalizedValue(LocalizedResourceValue value)
        {
            List<LocalizedResourceValue> resourceValues;
            if (values.TryGetValue(value.Property.Target, out resourceValues) == true)
                resourceValues.Add(value);
            else
                values[value.Property.Target] = new List<LocalizedResourceValue>() { value };
        }

        public static void RemoveLocalizedValue(LocalizedResourceValue value)
        {

        }

        public static void UpdateValues()
        {
            foreach (LocalizedResourceValue value in values.Values.SelectMany(v => v))
                value.UpdateValue();
        }

        public static void UpdateValues(DependencyObject target)
        {
            List<LocalizedResourceValue> resourceValues;
            if (values.TryGetValue(target, out resourceValues) == true)
                foreach (LocalizedResourceValue resourceValue in resourceValues)
                    resourceValue.UpdateValue();
        }

        private static void OnCurrentCultureChanged(
            DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            UpdateValues(sender);
        }

        private static void OnCurrentUICultureChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            CultureInfo newCulture = GetUICulture(sender);
            if (newCulture != null)
            {
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;

                UpdateValues();
            }
        }

        private static readonly Dictionary<DependencyObject, List<LocalizedResourceValue>> values = new Dictionary<DependencyObject, List<LocalizedResourceValue>>();
    }
}
