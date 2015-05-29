using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace EZMetrology.Windows.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class IValueConverterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TValueConverter"></typeparam>
        /// <param name="valueConverter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult Convert<TSource, TResult, TValueConverter>(
            this TValueConverter valueConverter,
            TSource value)
            where TValueConverter : IValueConverter
        {
            return (TResult)valueConverter
                .Convert(value, typeof(TResult), null, CultureInfo.CurrentCulture);
        }
    }
}
