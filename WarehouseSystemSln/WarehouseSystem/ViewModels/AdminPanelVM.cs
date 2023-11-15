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
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;
using WarehouseSystem.Views;

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
            AddCommand = new RelayCommand(AddUser);
            RemoveCommand = new RelayCommand(RemoveUser, CanRemoveEmployee);
            UpdateCommand = new RelayCommand(UpdateUser, CanUpdateUser);
            ReloadItemsCommand = new RelayCommand(ReloadData, CanReloadItems);
            ShowTable();
            ReloadData();
        }

        // Properties
        public string BaseUrl { get; }
        public MainViewModel MainVM { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand UpdateCommand { get; }
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

        private async void AddUser()
        {
            var dialogData = ShowUserDialog(isUpdating: false);
            if (dialogData == null)
                return;
            var user = new User()
            {
                Name = dialogData.Name,
                Surname = dialogData.Surname,
                Username = dialogData.Username,
                Password = Auth.Sha256Hash(dialogData.Password),
                Role = dialogData.Role,
            };
            var response = await AddUserRequest(MainVM.User.Id, user);
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show(response.ErrorMessage);
            ReloadData();
        }

        private async Task<ApiResponse<object>> AddUserRequest(int senderID, User user)
        {
            var userService = new UsersService(BaseUrl);
            var response = await userService.InsertUser(senderID, user);
            return response;
        }

        private async void RemoveUser()
        {
            var confirmationDialog = new ConfirmationDialog();
            var message = "Вы уверены, что хотите удалить этого пользователя?";
            if (await confirmationDialog.ShowConfirmationDialog(message) == false)
                return;
            var response = await RemoveUserRequest();
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show(response.ErrorMessage);
            ReloadData();
        }

        private async Task<ApiResponse<object>> RemoveUserRequest()
        {
            var userService = new UsersService(BaseUrl);
            var response = await userService.RemoveUser(senderID: MainVM.User.Id, userIDtoDelete: SelectedUser.Id);
            return response;
        }

        private async void UpdateUser()
        {
            var userData = MakeDialogData(SelectedUser);
            var dialogData = ShowUserDialog(isUpdating: true, userData);
            if (dialogData == null)
                return;
            string password;
            if (dialogData.Password == string.Empty)
            {
                password = string.Empty;
            } else
            {                
                password = Auth.Sha256Hash(dialogData.Password);
                MessageBox.Show(dialogData.Password + "\n" + password);
            }
            var user = new User()
            {
                Id = SelectedUser.Id,
                Name = dialogData.Name,
                Surname = dialogData.Surname,
                Username = dialogData.Username,
                Password = password,
                Role = dialogData.Role,
            };
            var response = await UpdateRequest(MainVM.User.Id, user);
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show(response.ErrorMessage);
            ReloadData();
        }

        private async Task<ApiResponse<object>> UpdateRequest(int senderID, User user)
        {
            var userService = new UsersService(BaseUrl);
            var response = await userService.UpdateUser(senderID: MainVM.User.Id, user);
            return response;
        }

        private UserDialogData MakeDialogData(User user)
        {
            return new UserDialogData
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                Role = user.Role,
            };
        }

        // shows dialog with user data
        private UserDialogData? ShowUserDialog(bool isUpdating, UserDialogData? userData = null)
        {
            var userDialogView = new UserDialogView();
            var userDialogVM = new UserDialogVM(isUpdating);
            userDialogView.DataContext = userDialogVM;

            if (userData != null)
            {
                userDialogVM.Name = userData.Name;
                userDialogVM.Surname = userData.Surname;
                userDialogVM.Username = userData.Username;
                userDialogVM.SelectedRole = userData.Role;
            }

            var dialogData = new UserDialogData();
            string errorText = string.Empty;
            bool hasData = false;

            userDialogVM.DialogClosing += (sender, data) =>
            {
                if (data != null)
                {
                    hasData = true;
                    dialogData = data;
                }
                userDialogView.Close();
            };

            userDialogView.ShowDialog();

            if (!hasData)
                return null;
            return dialogData;
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

        private bool CanUpdateUser()
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
            ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        }
    }
}
