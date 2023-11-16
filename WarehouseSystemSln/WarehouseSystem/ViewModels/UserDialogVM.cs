using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class UserDialogVM : BaseViewModel
    {
        // Fields
        private string _name;
        private string _surname;
        private string _username;
        private string _password;
        private bool _isUpdating;
        private UserRoles _selectedRole;

        // Constructor
        public UserDialogVM(bool isUpdating)
        {
            _isUpdating = isUpdating;
            _name = string.Empty;
            _surname = string.Empty;
            _username = string.Empty;
            _password = string.Empty;

        }

        // Properties
        public event EventHandler<UserDialogData>? DialogClosing;
        public ICommand OkButtonCommand => new RelayCommand(OkButtonClickHandler);
        public ICommand CancelButtonCommand => new RelayCommand(CancelButtonClickHandler);

        public ObservableCollection<UserRoles> Roles { get; set; } = new ObservableCollection<UserRoles>((UserRoles[])Enum.GetValues(typeof(UserRoles)));

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value; 
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public UserRoles SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                }
            }
        }

        // Methods
        private bool CheckFields()
        {
            if (Name == string.Empty) return false;
            if (Surname == string.Empty) return false;
            if (Username == string.Empty) return false;
            if (Password == string.Empty && !_isUpdating) return false;
            return true;
        }

        private void OkButtonClickHandler()
        {
            if (!CheckFields())
            {
                MessageBox.Show("Необходимо заполнить все необходимые поля!");
                return;
            }
            var data = new UserDialogData
            { 
                Name = _name,
                Surname = _surname,
                Username = _username,
                Password = _password,
                Role = _selectedRole
            };
            DialogClosing?.Invoke(this, data);
        }

        private void CancelButtonClickHandler()
        {
            DialogClosing?.Invoke(this, null);
        }
    }
}
