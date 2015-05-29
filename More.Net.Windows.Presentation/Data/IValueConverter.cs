using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EZMetrology.Windows.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IValueConverter<TSource, TResult> : IValueConverter
    {
        TResult Convert(TSource value);
        TSource ConvertBack(TResult value);
    }
}
