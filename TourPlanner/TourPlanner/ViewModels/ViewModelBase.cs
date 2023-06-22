using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged //if a property changes -> UI gets notified and passes along changes 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "")
        {
            ValidatePropertyName(propertyName);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ValidatePropertyName(string propertyName) //validates if property exists
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("Invalid propery name: " + propertyName);
            }
        }

        private string _alertMessage = null!;
        private string _successMessage = null!;
        private bool _hasException;
        private bool _isSuccess;

        public string AlertMessage
        {
            get => _alertMessage;
            set
            {
                _alertMessage = value;
                _hasException = !string.IsNullOrEmpty(_alertMessage);
                RaisePropertyChangedEvent(nameof(HasException));
                RaisePropertyChangedEvent(nameof(AlertMessage));
            }
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                _successMessage = value;
                _isSuccess = !string.IsNullOrEmpty(_successMessage);
                RaisePropertyChangedEvent(nameof(IsSuccess));
                RaisePropertyChangedEvent(nameof(SuccessMessage));
            }
        }

        public bool HasException
        {
            get { return _hasException; }
            set
            {
                _hasException = value;
                RaisePropertyChangedEvent(nameof(HasException));
            }
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set
            {
                _isSuccess = value;
                RaisePropertyChangedEvent(nameof(IsSuccess));
            }
        }

        protected void HandleException(Action action)
        {
            AlertMessage = null;
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                SuccessMessage = null;
                AlertMessage = ex.Message;
                Trace.WriteLine("Exceptiontestalert:");
                Trace.WriteLine(AlertMessage);
            }
        }

        protected async Task HandleExceptionAsync(Func<Task> action)
        {
            AlertMessage = null;
            try
            {
                await action.Invoke();
            }
            catch (Exception ex)
            {
                SuccessMessage = null;
                AlertMessage = ex.Message;
                Trace.WriteLine("Exceptiontestalert:");
                Trace.WriteLine(AlertMessage);
            }
        }
    }

}
