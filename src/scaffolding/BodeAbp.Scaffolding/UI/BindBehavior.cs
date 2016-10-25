using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BodeAbp.Scaffolding.UI
{
    internal class BindBehavior
    {
        #region ForceBindOnLostFocusProperty

        public static readonly DependencyProperty ForceBindOnLostFocusProperty =
            DependencyProperty.RegisterAttached(
                "ForceBindOnLostFocus",
                typeof(DependencyProperty),
                typeof(BindBehavior),
                new UIPropertyMetadata(null, ForceBindOnLostFocusOnChanged));

        public static DependencyProperty GetForceBindOnLostFocus(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(ForceBindOnLostFocusProperty);
        }

        public static void SetForceBindOnLostFocus(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(ForceBindOnLostFocusProperty, value);
        }

        private static void ForceBindOnLostFocusOnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = obj as FrameworkElement;
            if (targetElement != null && e.NewValue != null)
            {
                targetElement.LostKeyboardFocus += ForceBindOnLostFocus_OnLostKeyboardFocus;
            }
            else
            {
                targetElement.LostKeyboardFocus -= ForceBindOnLostFocus_OnLostKeyboardFocus;
            }
        }

        private static void ForceBindOnLostFocus_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OldFocus.IsKeyboardFocusWithin)
            {
                // Just bail out if we're still ultimately in the focus of the old element.
                // This is the case when clicking in a ComboBox that is IsEditable=true
                // as the new focus is the TextBox that the ComboBox creates as a child of itself.
                return;
            }

            var targetElement = (FrameworkElement)e.Source;
            var binding = targetElement.GetBindingExpression(GetForceBindOnLostFocus(targetElement));

            if (targetElement != null && binding != null)
            {
                binding.UpdateSource();
            }
        }

        #endregion

        #region ForceBindOnEnterProperty

        public static readonly DependencyProperty ForceBindOnEnterProperty =
            DependencyProperty.RegisterAttached(
                "ForceBindOnEnter",
                typeof(DependencyProperty),
                typeof(BindBehavior),
                new UIPropertyMetadata(null, ForceBindOnEnterPropertyChanged));

        public static DependencyProperty GetForceBindOnEnter(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(ForceBindOnEnterProperty);
        }

        public static void SetForceBindOnEnter(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(ForceBindOnEnterProperty, value);
        }

        private static void ForceBindOnEnterPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = obj as FrameworkElement;
            if (obj != null && e.NewValue != null)
            {
                targetElement.PreviewKeyDown += ForceBindOnEnterProperty_OnPreviewKeyDown;
            }
            else
            {
                targetElement.PreviewKeyDown -= ForceBindOnEnterProperty_OnPreviewKeyDown;
            }
        }

        private static void ForceBindOnEnterProperty_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var targetElement = (FrameworkElement)e.Source;
                var binding = targetElement.GetBindingExpression(GetForceBindOnEnter(targetElement));

                if (targetElement != null && binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }

        #endregion
    }
}
