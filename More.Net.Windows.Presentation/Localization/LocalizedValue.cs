using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace EZMetrology.Windows.Localization
{

    public class LocalizedResourceValue
    {
        public LocalizedResourceValue(LocalizedDependencyProperty property, String key)
        {
            this.property = property;
            this.resourcekey = key;
        }

        public LocalizedDependencyProperty Property
        {
            get { return property; }
        }

        public void UpdateValue()
        {
            property.SetValue(GetValue());
        }

        public Object GetValue()
        {
            var localizedValue = GetLocalizedValue();
            var converter = property.Converter;

            if (converter != null)
            {
                localizedValue = converter.Convert(
                    localizedValue,
                    property.GetValueType(),
                    property.ConverterParameter,
                    property.GetCulture()
                    );
            }

            return localizedValue;
        }


        public Object GetLocalizedValue()
        {
            var resourceManager = property.GetResourceManager();
            if (resourceManager == null)
                return "No Resource Manager";

            CultureInfo culture = null;

            if (property.IsInDesignMode)
                culture = property.GetDesignCulture();
            if (culture == null)
                culture = property.GetCulture();
            if (culture == null)
                culture = CultureInfo.CurrentUICulture;

            if (culture == null)
                return "No Culture";

            var value = resourceManager.GetObject(resourcekey, culture);

            return value;
        }

        private readonly LocalizedDependencyProperty property;
        private readonly String resourcekey;
    }
}
