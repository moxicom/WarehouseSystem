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
using WarehouseSystem.Models;

namespace WarehouseSystem.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _errorTextBlockValue = "";
        private string _password;
        private APIService _service;

        public string Username { get; set; }
        public ICommand LoggingButtonPressed {  get; set; }
        
        public LoginViewModel()
        {
            LoggingButtonPressed = new RelayCommand(ExecuteLoginRequest);
            _service = new APIService("http://localhost:8080");
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

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password)); // Уведомляем View об изменении свойства
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public async void ExecuteLoginRequest()
        {
            //ErrorTextBlockValue = Password;
            string result = await _service.VerifyUserRequest(Username, Password);
            ErrorTextBlockValue = result;
        }
    }
}
