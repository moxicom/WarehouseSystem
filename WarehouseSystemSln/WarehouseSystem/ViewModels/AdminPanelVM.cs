using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class AdminPanelVM : BaseViewModel
    {
        // Fields
        private const string _loadingStatus = "Загрузка...";
        private User _selectedUser;
        private bool _canReloadItems;
        private string _statusTextValue;
        private bool _isStatusTextVisible;
        private bool _isTableVisible;
        private ObservableCollection<User> _users;

    // Constructor
    public AdminPanelVM(string baseUrl, MainViewModel mainVM)
        {
            MainVM = mainVM;
            BaseUrl = baseUrl;
            Users = new ObservableCollection<User>();
            AddCommand = new RelayCommand(AddEmployee);
            RemoveCommand = new RelayCommand(RemoveEmployee, CanRemoveEmployee);
            EditCommand = new RelayCommand(EditEmployee, CanEditEmployee);
            ReloadItemsCommand = new RelayCommand(ReloadData, CanReloadItems);
            ShowTable();
            ReloadData();
        }

        // Properties
        public string BaseUrl { get; }
        public MainViewModel MainVM { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ReloadItemsCommand { get; }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser= value;
                OnPropertyChanged(nameof(SelectedUser));
                UpdateCommands();
            }
        }

        public bool CanReloadItems
        {
            get => _canReloadItems;
            set
            {
                _canReloadItems = value;
                OnPropertyChanged();
            }
        }

        public string StatusTextValue
        {
            get => _statusTextValue;
            set
            {
                _statusTextValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsStatusTextVisible
        {
            get => _isStatusTextVisible;
            set
            {
                _isStatusTextVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsTableVisible
        {
            get => _isTableVisible;
            set
            {
                _isTableVisible = value;
                OnPropertyChanged();
            }
        }

        // Methods
        private async void ReloadData()
        {
            ShowStatus(_loadingStatus);
            CanReloadItems = false;
            await LoadData();
        }

        private async Task LoadData()
        {
            Users = new ObservableCollection<User>();
            var response = await GetUsersRequest();
            ProcessLoadingData(response);
        }

        private async Task<ApiResponse<List<User>>> GetUsersRequest()
        {
            var userService = new UsersService(BaseUrl);
            var response = await userService.GetUsers(MainVM.User.Id);
            return response;
        }

        private void ProcessLoadingData(ApiResponse<List<User>> response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null)
                {
                    Users = new ObservableCollection<User>(response.Data);
                    ShowTable();
                }
                else
                {
                    Users = new ObservableCollection<User>();
                    ShowTable();
                }
            }
            else
            {
                ShowStatus(response.ErrorMessage);
            }
            CanReloadItems = true;
        }

        private void AddEmployee()
        {
            // Логика добавления нового работника
            // Возможно, открывается диалоговое окно для ввода данных
            // Затем добавляем нового работника в коллекцию
            Users.Add(new User() 
            {
                Id = 228,
                Name = "name",
                Surname = "surname",
                Role = Enums.UserRoles.BasicEmployee,
            });
        }

        private async void RemoveEmployee()
        {
            var confirmationDialog = new ConfirmationDialog();
            var message = "Вы уверены, что хотите удалить этого пользователя?";
            if (await confirmationDialog.ShowConfirmationDialog(message) == false)
                return;
            var response = await RemoveUserRequest();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show(response.ErrorMessage);
            }
            ReloadData();
        }

        private async Task<ApiResponse<object>> RemoveUserRequest()
        {
            var userService = new UsersService(BaseUrl);
            var response = await userService.RemoveUser(senderID: MainVM.User.Id, userIDtoDelete: SelectedUser.Id);
            return response;
        }

        private void EditEmployee()
        {
            // Логика изменения выбранного работника
            // Возможно, открывается диалоговое окно для редактирования данных
            // Затем обновляем свойства выбранного работника
            // Например: SelectedEmployee.Name = "Новое имя";
            //          SelectedEmployee.Position = "Новая должность";
        }

        private void ShowStatus(string statusText)
        {
            StatusTextValue = statusText;
            IsStatusTextVisible = true;
            IsTableVisible = false;
        }

        private void ShowTable()
        {
            IsStatusTextVisible = false;
            IsTableVisible = true;
        }

        private bool CanEditEmployee()
        {
            if (SelectedUser == null)
            {
                return false;
            }
            else if (SelectedUser.Id == MainVM.User.Id)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanRemoveEmployee()
        {
            if (SelectedUser == null)
            {
                return false;
            }
            else if (SelectedUser.Id == MainVM.User.Id)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateCommands()
        {
            // Вызываем это метод после смены выбранного работника
            // для обновления состояния команд
            ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
            ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
        }
    }
}
