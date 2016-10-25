using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BodeAbp.Scaffolding.UI
{
    internal class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private static readonly IDictionary<string, IList<string>> _noErrors = new Dictionary<string, IList<string>>();

        private readonly Dictionary<string, bool> _changeFlags = new Dictionary<string, bool>();
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        protected virtual void Validate([CallerMemberName]string propertyName = "")
        {

        }

        protected IDictionary<string, bool> ChangeFlags
        {
            get { return _changeFlags; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                return;
            }

            _changeFlags[propertyName] = true;

            Validate(propertyName);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
                OnPropertyChanged("HasErrors");
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                return _errors.Values;
            }

            List<string> errors;
            if (!_errors.TryGetValue(propertyName, out errors))
            {
                errors = new List<string>();
            }
            return errors;
        }

        public bool HasErrors
        {
            get { return _errors.SelectMany(e => e.Value).Any(); }
        }

        protected void ClearError(string propertyName)
        {
            List<string> errors;
            if (_errors.TryGetValue(propertyName, out errors))
            {
                errors.Clear();
            }
            OnErrorsChanged(propertyName);
        }

        protected void AddError(string propertyName, string error)
        {
            List<string> errors;
            if (!_errors.TryGetValue(propertyName, out errors))
            {
                errors = new List<string>();
                _errors[propertyName] = errors;
            }
            errors.Add(error);
            OnErrorsChanged(propertyName);
        }
    }
}
