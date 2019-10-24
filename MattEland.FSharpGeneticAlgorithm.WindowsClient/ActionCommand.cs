using System;
using System.Windows.Input;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _invokedAction;

        public ActionCommand(Action invokedAction)
        {
            _invokedAction = _ => invokedAction?.Invoke();
        }

        public ActionCommand(Action<object> invokedAction)
        {
            _invokedAction = invokedAction;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _invokedAction?.Invoke(parameter);

        public event EventHandler CanExecuteChanged;
    }
}