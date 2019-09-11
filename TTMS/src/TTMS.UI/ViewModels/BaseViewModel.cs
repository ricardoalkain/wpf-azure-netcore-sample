using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TTMS.UI.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, value)) return;

            member = value;
            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (NullReferenceException)
            {
            }
            catch
            {
                throw;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
