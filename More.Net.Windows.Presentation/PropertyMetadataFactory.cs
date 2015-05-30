using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace More.Net.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyMetadataFactory
    {
        /// <summary>
        /// Gets the default property meta data.
        /// </summary>
        /// <returns></returns>
        public static PropertyMetadata Create()
        {
            return new PropertyMetadata();
        }

        /// <summary>
        /// Transforms a default value for a dependency property into property meta data.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static PropertyMetadata Create<TProperty>(
            TProperty defaultValue)
        {
            return new PropertyMetadata(defaultValue);
        }

        /// <summary>
        /// Transforms a property changed handler for a dependency property 
        /// into property meta data.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyChanged"></param>
        /// <returns></returns>
        public static PropertyMetadata Create<TOwner, TProperty>(
            Action<TOwner, TProperty, TProperty> propertyChanged)
            where TOwner : DependencyObject
        {
            return new PropertyMetadata(
                (d, e) => propertyChanged((TOwner)d, (TProperty)e.OldValue, (TProperty)e.NewValue));
        }

        /// <summary>
        /// Transforms a default value and a property changed handler for a dependency property 
        /// into property meta data.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChanged"></param>
        /// <returns></returns>
        public static PropertyMetadata Create<TOwner, TProperty>(
            TProperty defaultValue,
            Action<TOwner, TProperty, TProperty> propertyChanged)
            where TOwner : DependencyObject
        {
            return new PropertyMetadata(
                defaultValue,
                (d, e) => propertyChanged((TOwner)d, (TProperty)e.OldValue, (TProperty)e.NewValue));
        }

        /// <summary>
        /// Transforms a default value, a property changed handler, and a coerce value handler for 
        /// a dependency property into property meta data.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChanged"></param>
        /// <param name="coerceValue"></param>
        /// <returns></returns>
        public static PropertyMetadata Create<TOwner, TProperty>(
            TProperty defaultValue,
            Action<TOwner, TProperty, TProperty> propertyChanged,
            Func<TOwner, TProperty, TProperty> coerceValue)
            where TOwner : DependencyObject
        {
            return new PropertyMetadata(
                defaultValue,
                (d, e) => propertyChanged((TOwner)d, (TProperty)e.OldValue, (TProperty)e.NewValue),
                (d, value) => coerceValue((TOwner)d, (TProperty)value));
        }
    }
}
