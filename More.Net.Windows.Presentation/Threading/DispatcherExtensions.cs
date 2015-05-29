using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EZMetrology.Windows.Threading
{
    public static class DispatcherExtensions
    {
        public static DispatcherOperation BeginInvoke(
            this Dispatcher dispatcher, 
            Action action)
        {
            return dispatcher.BeginInvoke(action);
        }

        public static DispatcherOperation BeginInvoke(
            this Dispatcher dispatcher,
            Action action,
            DispatcherPriority priority)
        {
            return dispatcher.BeginInvoke(action, priority);
        }

        public static DispatcherOperation BeginInvoke<TParam1>(
            this Dispatcher dispatcher, 
            Action<TParam1> action, 
            TParam1 param1)
        {
            return dispatcher.BeginInvoke(action, param1);
        }

        public static DispatcherOperation BeginInvoke<TParam1>(
            this Dispatcher dispatcher,
            Action<TParam1> action,
            DispatcherPriority priority,
            TParam1 param1)
        {
            return dispatcher.BeginInvoke(action, priority, param1);
        }
    }
}
