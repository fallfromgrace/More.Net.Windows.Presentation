using More.Net.Linq.Expressions;
using System;
using System.Linq.Expressions;
using System.Windows;

namespace More.Net.Windows
{
    /// <summary>
    /// Provides type safe register methods.
    /// </summary>
    public static class DependencyPropertyFactory
    {
        #region Register

        /// <summary>
        /// Registers a dependency property using the specified property expression.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression)
            where TOwner : DependencyObject
        {
            return DependencyProperty.Register(
                propertyExpression.GetPropertyPath(),
                typeof(TProperty),
                typeof(TOwner));
        }

        /// <summary>
        /// Registers a dependency property using the specified property expression and default 
        /// value.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            TProperty defaultValue)
            where TOwner : DependencyObject
        {
            return Register(
                propertyExpression, 
                PropertyMetadataFactory.Create(defaultValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="propertyChanged"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            Action<TOwner, TProperty, TProperty> propertyChanged)
            where TOwner : DependencyObject
        {
            return Register(
                propertyExpression, 
                PropertyMetadataFactory.Create(propertyChanged));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChanged"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            TProperty defaultValue,
            Action<TOwner, TProperty, TProperty> propertyChanged)
            where TOwner : DependencyObject
        {
            return Register(
                propertyExpression, 
                PropertyMetadataFactory.Create(defaultValue, propertyChanged));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChanged"></param>
        /// <param name="coerceValue"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            TProperty defaultValue,
            Action<TOwner, TProperty, TProperty> propertyChanged,
            Func<TOwner, TProperty, TProperty> coerceValue)
            where TOwner : DependencyObject
        {
            return Register(
                propertyExpression,
                PropertyMetadataFactory.Create(defaultValue, propertyChanged, coerceValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="validateValue"></param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            Func<TProperty, Boolean> validateValue)
            where TOwner : DependencyObject
        {
            return DependencyProperty.Register(
                propertyExpression.GetPropertyPath(),
                typeof(TProperty),
                typeof(TOwner),
                PropertyMetadataFactory.Create(),
                (value) => validateValue((TProperty)value));
        }

        /// <summary>
        /// Registers a dependency property using the specified property expression, default value, 
        /// property changed handler, coercion handler, and validation handler.
        /// </summary>
        /// <typeparam name="TOwner">
        /// The type of the owner of the property.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// The type of the property.
        /// </typeparam>
        /// <param name="propertyExpression"></param>
        /// An expression that selects the property to register as a dependency property.
        /// <param name="defaultValue"></param>
        /// The default value for this property if the property is not set.
        /// <param name="coerceValue"></param>
        /// A function that returns a new coerced value after inspecting the new value.
        /// <param name="propertyChanged"></param>
        /// An action that is invoked when the property changes, passing the old and new value.
        /// <param name="validateValue">
        /// A predicate returns true if the new value is valid for this property.
        /// </param>
        /// <returns></returns>
        public static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            TProperty defaultValue,
            Action<TOwner, TProperty, TProperty> propertyChanged,
            Func<TOwner, TProperty, TProperty> coerceValue,
            Func<TProperty, Boolean> validateValue)
            where TOwner : DependencyObject
        {
            return Register(
                propertyExpression,
                PropertyMetadataFactory.Create(defaultValue, propertyChanged, coerceValue),
                (value) => validateValue((TProperty)value));
        }

        #region Private

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="metaData"></param>
        /// <param name="validateValue"></param>
        /// <returns></returns>
        private static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression, 
            PropertyMetadata metaData,
            Func<TProperty, Boolean> validateValue)
        {
            return DependencyProperty.Register(
                propertyExpression.GetPropertyPath(),
                typeof(TProperty),
                typeof(TOwner),
                metaData,
                (value) => validateValue((TProperty)value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        private static DependencyProperty Register<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            PropertyMetadata metaData)
        {
            return DependencyProperty.Register(
                propertyExpression.GetPropertyPath(),
                typeof(TProperty),
                typeof(TOwner),
                metaData);
        }

        #endregion

        #endregion

        #region Register Attached

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="validateValue"></param>
        /// <returns></returns>
        public static DependencyProperty RegisterAttached<TOwner, TTarget, TProperty>(
            Expression<Func<TOwner, TTarget, TProperty>> getExpression,
            TProperty defaultValue,
            Action<TTarget, TProperty, TProperty> propertyChanged,
            Func<TTarget, TProperty, TProperty> coerceValue,
            Func<TProperty, Boolean> validateValue)
            where TTarget : DependencyObject
        {
            MethodCallExpression methodExpression = getExpression.Body as MethodCallExpression;
            if (methodExpression == null)
                throw new ArgumentException();

            return DependencyProperty.RegisterAttached(
                methodExpression.Method.Name.Remove(0, 3),
                typeof(TProperty),
                typeof(TOwner),
                PropertyMetadataFactory.Create(
                    defaultValue,
                    null,
                    coerceValue));
        }

        #endregion
    }
}
