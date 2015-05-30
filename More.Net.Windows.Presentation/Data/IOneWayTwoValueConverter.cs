using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace More.Net.Windows.Data
{
    public interface IOneWayTwoValueConverter<TSource1, TSource2, TResult> : IMultiValueConverter
    {
        TResult Convert(TSource1 value1, TSource2 value2);
    }
}
