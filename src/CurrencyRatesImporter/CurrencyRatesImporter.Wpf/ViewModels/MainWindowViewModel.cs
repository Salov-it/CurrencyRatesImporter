using CurrencyRatesImporter.Application.CQRS.Commands.ImportRates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CurrencyRatesImporter.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ImportRatesCommandHandler _handler;

        public MainWindowViewModel(ImportRatesCommandHandler handler)
        {
            _handler = handler;

            ImportCommand = new RelayCommand(async _ =>
            {
                try
                {
                    Status = "Импорт выполняется...";

                    var cmd = new ImportRatesCommand(SelectedDate);

                    await _handler.Handle(cmd);

                    Status = "Импорт успешно завершён.";
                    
                }
                catch (Exception ex)
                {
                    Status = "Произошла ошибка.";

                    // логируем, чтобы сохранить стек
                    File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");

                    MessageBox.Show("Произошла критическая ошибка. Подробности см. в error.log.",
                        "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private string _status = "";
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _selectedDate = null;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand ImportCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}