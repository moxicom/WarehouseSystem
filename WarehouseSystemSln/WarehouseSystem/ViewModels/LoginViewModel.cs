using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;
using WarehouseSystem.Views;

namespace WarehouseSystem.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        private string? _errorTextBlockValue;
        private string? _password;
        public string? Username { get; set; }
        public ICommand? LoggingButtonPressed { get; set; }
        public Auth AuthService { get; private set; }

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate RequestClose;

        public LoginViewModel(string baseURL)
        {
            AuthService =  new Auth(baseURL);
            LoggingButtonPressed = new RelayCommand(ExecuteLoginRequest);
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
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public async void ExecuteLoginRequest()
        {
            ErrorTextBlockValue = "Ожидайте";
            string result = await AuthService.VerifyUserRequest(Username, Password);
            ErrorTextBlockValue = result;
            //
            // Created this to test open new window function with parameters
            //
            if (true)
            {
                LoginViewModel newViewModel = new LoginViewModel(AuthService.BaseURL);
                TestNewWindow newWindow = new TestNewWindow();
                newWindow.DataContext = newViewModel;
                newWindow.Show();
                CloseWindow();
            }
        }

        public void CloseWindow()
        {
            RequestClose.Invoke();
        }
    }
}
