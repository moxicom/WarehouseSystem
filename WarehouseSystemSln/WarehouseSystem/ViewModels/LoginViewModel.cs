using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WarehouseSystem.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _errorTextBlockValue = "";
        public ICommand LoggingButtonPressed {  get; set; }
        
        public LoginViewModel()
        {
            LoggingButtonPressed = new RelayCommand(ExecuteLogin);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void ExecuteLogin()
        {
            ErrorTextBlockValue = "Кнопка нажата";
        }

        public string ErrorTextBlockValue
        {
            get { return _errorTextBlockValue; }
            set
            {
                _errorTextBlockValue = value;
                OnPropertyChanged(nameof(ErrorTextBlockValue));
            }
        }
    }
}
