using System;
using System.Windows.Input;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    public class RelayCommand : ICommand
    {
        private readonly Action _invokedAction;

        public RelayCommand(Action invokedAction)
        {
            _invokedAction = invokedAction;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _invokedAction?.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}