using System;
using System.Windows.Input;
using JetBrains.Annotations;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _invokedAction;

        [PublicAPI]
        public ActionCommand(Action invokedAction)
        {
            _invokedAction = _ => invokedAction?.Invoke();
        }

        [PublicAPI]
        public ActionCommand(Action<object> invokedAction)
        {
            _invokedAction = invokedAction;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _invokedAction?.Invoke(parameter);

        public event EventHandler CanExecuteChanged;
    }
}