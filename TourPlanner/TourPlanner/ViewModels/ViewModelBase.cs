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
        private bool _hasException;

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

        public bool HasException
        {
            get { return _hasException; }
            set
            {
                _hasException = value;
                RaisePropertyChangedEvent(nameof(HasException));
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
                AlertMessage = ex.Message;
                Trace.WriteLine("Exceptiontestalert:");
                Trace.WriteLine(AlertMessage);
            }
        }
    }

}
