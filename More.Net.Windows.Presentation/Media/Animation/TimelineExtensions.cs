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
    /// 
    /// </summary>
    public static class TimelineExtensions
    {
        /// <summary>
        /// Registers the timeline with the specified dependency property on the target object.
        /// </summary>
        /// <typeparam name="TTimeline"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="timeline"></param>
        /// <param name="target"></param>
        /// <param name="propertyExpression"></param>
        public static TTimeline Register<TTimeline, TTarget, TProperty>(
            this TTimeline timeline,
            TTarget target,
            Expression<Func<TTarget, TProperty>> propertyExpression)
            where TTimeline : Timeline
            where TTarget : DependencyObject
        {
            Storyboard.SetTarget(timeline, target);
            Storyboard.SetTargetProperty(
                timeline,
                new PropertyPath(propertyExpression.GetPropertyPath()));

            return timeline;
        }
    }
}
