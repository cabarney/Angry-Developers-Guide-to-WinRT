using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HelloSilverlight
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public bool SetProperty<T>(ref T backingStorage, T value, string propertyName)
        {
            if (Equals(backingStorage, value)) return false;

            backingStorage = value;

            return true;
        }

        public bool SetProperty<T>(Func<T> getter, Action<T> setter, T value, string propertyName)
        {
            T backingStorage = getter();
            if (!SetProperty(ref backingStorage, value, propertyName))
            {
                return false;
            }

            setter(backingStorage);
            return true;
        }


        protected bool SetPropertyAndValidate<T>(Func<T> getter, Action<T> setter, T value, string propertyName)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = CreateValidationContext(propertyName);
            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                SetErrors(propertyName, validationResults);
                return false;
            }
            ClearErrors(propertyName);
            if (SetProperty(getter, setter, value, propertyName))
            {
                NotifyOfPropertyChange(propertyName);
                return true;
            }
            return false;
        }


        private readonly IDictionary<string, IList<string>> _errors = new Dictionary<string, IList<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            IList<string> list;
            return _errors.TryGetValue(propertyName, out list) ? list : Enumerable.Empty<string>();
        }

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        private ValidationContext CreateValidationContext(string propertyName)
        {
            var validationContext = new ValidationContext(this)
                {
                    MemberName = propertyName
                };
            return validationContext;
        }

        private void ClearErrors(string propertyName)
        {
            _errors.Remove(propertyName);
            RaiseErrorsChanged(propertyName);
        }

        private void SetErrors(string propertyName, IEnumerable<ValidationResult> validationResults)
        {
            var propertyErrors = validationResults
                .Select(x => x.ErrorMessage)
                .ToList();

            _errors[propertyName] = propertyErrors;
            RaiseErrorsChanged(propertyName);
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }

            NotifyOfPropertyChange("HasErrors");
        }

        protected void ValidateObject()
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults);
            if (isValid)
            {
                ClearErrors(string.Empty);
                return;
            }

            var allResults = validationResults
                .SelectMany(result => result.MemberNames.Select(member => new KeyValuePair<string, ValidationResult>(member, result)))
                .GroupBy(x => x.Key, pair => pair.Value);

            foreach (var memberResults in allResults)
            {
                SetErrors(memberResults.Key, memberResults);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyOfPropertyChange(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}