using System.Net;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;
using WarehouseSystem.Views;

namespace WarehouseSystem.ViewModels;

internal class LoginVM : BaseViewModel
{
    public delegate void CloseWindowDelegate();

    private readonly string _baseUrl;
    private string _errorTextBlockValue = string.Empty;
    private string _password = string.Empty;

    public LoginVM(string baseUrl)
    {
        AuthService = new Auth(baseUrl);
        LoggingButtonPressed = new RelayCommand(ExecuteLoginRequest);
        _baseUrl = baseUrl;
        Username = "";
        Password = "";
    }

    public string? Username { get; set; }
    public ICommand? LoggingButtonPressed { get; set; }
    public Auth AuthService { get; }

    public string ErrorTextBlockValue
    {
        get => _errorTextBlockValue;
        set
        {
            _errorTextBlockValue = value;
            OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
    }

    public event CloseWindowDelegate RequestClose;

    public async void ExecuteLoginRequest()
    {
        if (Password == "" || Username == "")
            return;

        ErrorTextBlockValue = "Ожидайте";
        var response = await AuthService.VerifyUserRequest(Username, Password);
        ErrorTextBlockValue = response.ErrorMessage;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var newViewModel = new MainViewModel(_baseUrl, response.Data);
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