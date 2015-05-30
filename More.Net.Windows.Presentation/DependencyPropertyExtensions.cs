using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq.Expressions;

namespace More.Net.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyPropertyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDependencyProperty"></typeparam>
        /// <typeparam name="TOwner"></typeparam>
        /// <param name="dependencyProperty"></param>
        public static void OverrideMetadata<TOwner, TProperty>(
            this DependencyProperty source,
            Expression<Func<TOwner, TProperty>> expression)
            where TOwner : DependencyObject
        {
            source.OverrideMetadata(
                typeof(TOwner),
                PropertyMetadataFactory.Create());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDependencyProperty"></typeparam>
        /// <typeparam name="TOwner"></typeparam>
        /// <param name="dependencyProperty"></param>
        public static void OverrideMetadata<TOwner, TProperty>(
            this DependencyProperty source,
            Expression<Func<TOwner, TProperty>> expression,
            Action<TOwner, TProperty, TProperty> propertyChanged)
            where TOwner : DependencyObject
        {
            source.OverrideMetadata(
                typeof(TOwner),
                PropertyMetadataFactory.Create(propertyChanged));
        }
    }
}
