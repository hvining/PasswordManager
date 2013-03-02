using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace PasswordManager.Behaviors
{
    public class KeyDownToAction
    {
        // Using a DependencyProperty as the backing store for KeyDownActionProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyDownActionProperty =
            DependencyProperty.Register("KeyDownAction", typeof(Action<KeyRoutedEventArgs>), typeof(KeyDownToAction), new PropertyMetadata(null, ActionChangd));

        public static Action<KeyRoutedEventArgs> GetKeyDownAction(DependencyObject obj)
        {
            if (obj == null)
                return null;

            return (Action<KeyRoutedEventArgs>)obj.GetValue(KeyDownActionProperty);
        }

        public static void SetKeyDownAction(DependencyObject obj, Action<KeyRoutedEventArgs> action)
        {
            if (obj == null)
                return;

            obj.SetValue(KeyDownActionProperty, action);
        }

        private static void ActionChangd(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;

            if(element == null)
                return;

            Action<KeyRoutedEventArgs> keyDownAction = (Action<KeyRoutedEventArgs>)element.GetValue(KeyDownActionProperty);

            element.KeyDown += element_KeyDown;
            element.Unloaded += Unloaded;  
        }

        static void element_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)sender;
            Action<KeyRoutedEventArgs> action = (Action<KeyRoutedEventArgs>)obj.GetValue(KeyDownActionProperty);
            action(e);
        }

        private static void Unloaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.KeyDown -= element_KeyDown;
            element.Unloaded -= Unloaded;
        }


    }
}
