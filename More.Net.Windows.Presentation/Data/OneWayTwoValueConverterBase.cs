using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace More.Net.Windows.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource1"></typeparam>
    /// <typeparam name="TSource2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class OneWayTwoValueConverterBase<TSource1, TSource2, TResult> : 
        IOneWayTwoValueConverter<TSource1, TSource2, TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract TResult Convert(TSource1 value1, TSource2 value2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Object)Convert((TSource1)values[0], (TSource2)values[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
