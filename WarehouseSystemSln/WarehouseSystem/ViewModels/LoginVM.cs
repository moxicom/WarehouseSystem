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
        private string _errorTextBlockValue;
        private string _password;
        private string _baseURL;

        public string? Username { get; set; }
        public ICommand? LoggingButtonPressed { get; set; }
        public Auth AuthService { get; private set; }

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate RequestClose;

        public LoginVM(string baseURL)
        {
            AuthService =  new Auth(baseURL);
            LoggingButtonPressed = new RelayCommand(ExecuteLoginRequest);
            _baseURL = baseURL;
            Username = "";
            Password = "";
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
            if (Password == "" || Username == "")
                return;

            ErrorTextBlockValue = "Ожидайте";
            ApiResponse<User> response = await AuthService.VerifyUserRequest(Username, Password);
            ErrorTextBlockValue = response.ErrorMessage;
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MainViewModel newViewModel = new MainViewModel(_baseURL);
                AppMainWindow newWindow = new AppMainWindow();
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
