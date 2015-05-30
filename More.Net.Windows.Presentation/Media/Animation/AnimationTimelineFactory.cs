using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Linq.Expressions;
using More.Net.Linq.Expressions;

namespace More.Net.Windows.Media.Animation
{
    /// <summary>
    /// Factory methods for creating animations.
    /// </summary>
    public static class AnimationTimelineFactory
    {
        /// <summary>
        /// Creates the appropriate derived animation timeline based on the input type.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static AnimationTimeline Create<TValue>(
            TValue from,
            TValue to,
            Duration duration)
        {
            if (typeof(TValue) == typeof(Double))
            {
                Double fromValue = Convert.ToDouble(from);
                Double toValue = Convert.ToDouble(to);
                return new DoubleAnimation(fromValue, toValue, duration);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Creates a new animation timeline and registers it with the appropriate property on the 
        /// target object.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="target"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static AnimationTimeline Create<TValue, TTarget, TProperty>(
            TValue from,
            TValue to,
            Duration duration,
            TTarget target,
            Expression<Func<TTarget, TProperty>> propertyExpression)
            where TTarget : DependencyObject
        {
            return Create(from, to, duration).Register(target, propertyExpression);
        }
    }
}
