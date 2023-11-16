using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;
using static WarehouseSystem.ViewModels.LoginVM;

namespace WarehouseSystem.ViewModels;

public class MainViewModel : BaseViewModel
{
    // fields
    private readonly string _baseUrl;
    private bool _isAdminPanelVisible;
    private readonly CategoriesVM _categoriesVM;
    private readonly AdminPanelVM _adminPanelVM;
    private BaseViewModel _currentViewModel;

    public delegate void CloseWindowDelegate();
    public event CloseWindowDelegate RequestClose;


    // constructor
    public MainViewModel(string baseUrl, User user)
    {
        User = user;
        IsAdminPanelVisible = User.Role == Enums.UserRoles.Admin ? true : false;
        _baseUrl = baseUrl;
        _categoriesVM = new CategoriesVM(baseUrl, this);
        _adminPanelVM = new AdminPanelVM(baseUrl, this);
        OpenHomeCommand = new RelayCommand(OpenHomeView);
        OpenCategoriesCommand = new RelayCommand(OpenCategoriesView);
        OpenAdminPanelCommand = new RelayCommand(OpenAdminPanel);
        LogOutCommand = new RelayCommand(LogOut);
        OpenHomeView();
    }

    // properties
    public ICommand OpenHomeCommand { get; }
    public ICommand OpenCategoriesCommand { get; }
    public ICommand OpenAdminPanelCommand { get; }
    public ICommand LogOutCommand { get; }
    public User User { get; }

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel != value)
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsAdminPanelVisible
    {
        get => _isAdminPanelVisible;
        set 
        {
            _isAdminPanelVisible = value;
            OnPropertyChanged(nameof(IsAdminPanelVisible));
        }
    }

    // methods
    public void OpenCategoriesView() => CurrentViewModel = _categoriesVM;
    public void OpenHomeView() => CurrentViewModel = new HomeVM(User.Name, User.Surname);
    public void OpenCategoryView(int ID) => CurrentViewModel = new CategoryVM(ID, _baseUrl, this);
    public void OpenAdminPanel() => CurrentViewModel = _adminPanelVM;

    public void LogOut()
    {
        var currentWindow = Application.Current.MainWindow;
        var authWindow = new MainWindow(_baseUrl);
        authWindow.Show();
        currentWindow?.Close();
    }
}