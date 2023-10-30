using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;
using WarehouseSystem.Views;

namespace WarehouseSystem.ViewModels
{
    internal class LoginVM : BaseViewModel
    {
        private string _errorTextBlockValue = string.Empty;
        private string _password = string.Empty;
        private readonly string _baseUrl;

        public string? Username { get; set; }
        public ICommand? LoggingButtonPressed { get; set; }
        public Auth AuthService { get; private set; }

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate RequestClose;

        public LoginVM(string baseUrl)
        {
            AuthService =  new Auth(baseUrl);
            LoggingButtonPressed = new RelayCommand(ExecuteLoginRequest);
            _baseUrl = baseUrl;
            Username = "";
            Password = "";
        }

        public string ErrorTextBlockValue
        {
            get => _errorTextBlockValue;
            set
            {
                _errorTextBlockValue = value;
                OnPropertyChanged(nameof(ErrorTextBlockValue));
            }
        }

        public string Password
        {
            get => _password;
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
            if (Password == "" || Username == "")
                return;

            ErrorTextBlockValue = "Ожидайте";
            ApiResponse<User> response = await AuthService.VerifyUserRequest(Username, Password);
            ErrorTextBlockValue = response.ErrorMessage;
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var newViewModel = new MainViewModel(_baseUrl);
                var newWindow = new AppMainWindow();
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
