using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace BodeAbp.Scaffolding.UI
{
    internal abstract class ViewModel<T> : ViewModel where T : ViewModel
    {
        protected void OnPropertyChanged<TProp>(Expression<Func<T, TProp>> propertyFunc)
        {
            var propertyName = PropertyName(propertyFunc);

            Debug.Assert(!String.IsNullOrEmpty(propertyName));

            base.OnPropertyChanged(propertyName);
        }

        protected static string PropertyName<TProp>(Expression<Func<T, TProp>> expression)
        {
            var name = String.Empty;
            var propExpression = expression.Body as MemberExpression;

            if (propExpression != null)
            {
                name = propExpression.Member.Name;
            }

            return name;
        }

        protected bool ShouldValidate(string propretyName, string currentPropertyName)
        {
            if (propretyName == null)
            {
                return true;
            }

            return String.Equals(propretyName, currentPropertyName, StringComparison.Ordinal);
        }
    }
}
