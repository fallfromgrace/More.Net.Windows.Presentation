using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsEventManager = System.Windows.EventManager;

namespace EZMetrology.Windows
{
    //public static class EventManager
    //{
    //    public static RoutedEvent RegisterRoutedEvent<TOwner, TEventHandler>(
    //        Expression<Func<TOwner, TEventHandler>> eventExpression,
    //        RoutingStrategy routingStrategy)
    //        where TOwner : UIElement
    //        where TEventHandler : RoutedEventHandler
    //    {
    //        if (eventExpression == null)
    //            throw new ArgumentNullException("eventExpression");

    //        MemberExpression body = eventExpression.Body as MemberExpression;

    //        if (body == null)
    //            throw new ArgumentException("Invalid argument", "propertyExpression");

    //        EventInfo handler = body.Member as EventInfo;

    //        if (handler == null)
    //            throw new ArgumentException("Argument is not a handler", "propertyExpression");

    //        return WindowsEventManager.RegisterRoutedEvent(
    //            handler.Name, routingStrategy, typeof(TEventHandler), typeof(TOwner));
    //    }
    //}

    public static class EventManagerEx
    {
        //public static void RegisterClassHandler<TOwner, TEvent>()
    }
}
