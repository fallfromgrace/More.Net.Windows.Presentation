using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace More.Net.Windows.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class SingleElementContainer : CollectionContainer
    {
        #region Static Members

        static SingleElementContainer()
        {
            EmptyContent = new Object();
            ContentProperty = DependencyPropertyFactory.Register(
                (SingleElementContainer d) => d.Content,
                EmptyContent,
                OnContentChanged); 
            CollectionProperty.OverrideMetadata(
                typeof(SingleElementContainer), 
                new FrameworkPropertyMetadata { CoerceValueCallback = CoerceCollection });
        }

        public static readonly Object EmptyContent;

        public static readonly DependencyProperty ContentProperty;

        private static object CoerceCollection(DependencyObject d, object baseValue)
        {
            return ((SingleElementContainer)d).content;
        }

        private static void OnContentChanged(
            SingleElementContainer d, 
            Object oldValue, 
            Object newValue)
        {
            var content = d.content;

            if (oldValue == EmptyContent && newValue != EmptyContent)
                content.Add(newValue);
            else if (oldValue != EmptyContent && newValue == EmptyContent)
                content.RemoveAt(0);
            else // (e.OldValue != EmptyContent && e.NewValue != EmptyContent)
                content[0] = newValue;
        }

        #endregion

        #region Instance Members

        public SingleElementContainer()
        {
            content = new ObservableCollection<Object>();
            CoerceValue(CollectionProperty);
        }


        public Object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private readonly ObservableCollection<Object> content;

        #endregion
    }
}
