using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BodeAbp.Scaffolding.UI
{
    internal class FocusBehavior
    {
        public static readonly DependencyProperty FocusOnFirstElementProperty =
            DependencyProperty.RegisterAttached(
                "FocusOnFirstElement",
                typeof(bool),
                typeof(FocusBehavior),
                new PropertyMetadata(false, OnFocusOnFirstElementPropertyChanged));

        public static bool GetFocusOnFirstElement(Control control)
        {
            return (bool)control.GetValue(FocusOnFirstElementProperty);
        }

        public static void SetFocusOnFirstElement(Control control, bool value)
        {
            control.SetValue(FocusOnFirstElementProperty, value);
        }

        static void OnFocusOnFirstElementPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as Control;
            if (control == null || !(args.NewValue is bool))
            {
                return;
            }

            if ((bool)args.NewValue)
            {
                control.Loaded += (sender, e) => control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
