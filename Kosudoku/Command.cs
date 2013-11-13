using System;
using System.Windows.Input;

namespace Kosudoku
{
    internal class Command : ICommand
    {
        private readonly Action<object> _executeDelegate;

        public Command(Action<object> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return _executeDelegate != null;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }
    }
}