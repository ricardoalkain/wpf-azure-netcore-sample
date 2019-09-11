using System;
using System.Windows.Input;

namespace TTMS.UI.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action action;

        public event EventHandler CanExecuteChanged = delegate { };

        public RelayCommand(Action executeMethod)
        {
            action = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            action = executeMethod;
            canExecute = canExecuteMethod;
        }

        //public void RaiseCanExecuteChanged()
        //{
        //    CanExecuteChanged(this, EventArgs.Empty);
        //}

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute();
            }

            return action != null;
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> canExecute;
        private readonly Action<T> action;

        public event EventHandler CanExecuteChanged = delegate { };

        public RelayCommand(Action<T> executeMethod)
        {
            action = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            action = executeMethod;
            canExecute = canExecuteMethod;
        }

        //public void RaiseCanExecuteChanged()
        //{
        //    CanExecuteChanged(this, EventArgs.Empty);
        //}

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                T tparm = (T)parameter;
                return canExecute(tparm);
            }

            return action != null;
        }

        public void Execute(object parameter)
        {
            action?.Invoke((T)parameter);
        }
    }
}
