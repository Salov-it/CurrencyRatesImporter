using System.Windows.Input;

namespace CurrencyRatesImporter.Wpf
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object?, Task> _executeAsync;

        public RelayCommand(Func<object?, Task> executeAsync)
        {
            _executeAsync = executeAsync;
        }

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            await _executeAsync(parameter);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
