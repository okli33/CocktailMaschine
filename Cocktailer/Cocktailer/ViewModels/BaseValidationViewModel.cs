using Cocktailer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cocktailer.ViewModels
{
    public class BaseValidationViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        readonly IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>(); 
        public BaseValidationViewModel(INavService navService) : base(navService)
        {

        }

        protected void Validate(Func<bool> rule, string error, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;
            if (errors.ContainsKey(propertyName))
                errors.Remove(propertyName);
            if (rule() == false)
                errors.Add(propertyName, new List<string> { error });
            OnPropertyChanged(nameof(HasErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors => errors?.Any(x => x.Value?.Any() == true) == true;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return errors.SelectMany(x => x.Value);
            if (errors.ContainsKey(propertyName) && errors[propertyName].Any())
                return errors[propertyName];
            return new List<string>();
        }
    }
}
