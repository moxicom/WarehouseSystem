using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class AdminPanelVM : BaseViewModel
    {
        // Fields
        private User _selectedUser;
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

    // Constructor
    public AdminPanelVM(MainViewModel mainVM) 
        {
            MainVM = mainVM;
            AddCommand = new RelayCommand(AddEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee, CanDeleteEmployee);
            EditCommand = new RelayCommand(EditEmployee, CanEditEmployee);
            LoadInitialData();
        }

        // Properties
        MainViewModel MainVM { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

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

        // Methods
        private void LoadInitialData()
        {
            // Здесь вы можете загрузить начальные данные или примеры работников
            // Employees.Add(new Employee("Имя1", "Должность1"));
            // Employees.Add(new Employee("Имя2", "Должность2"));
            // ...
        }

        private void AddEmployee()
        {
            // Логика добавления нового работника
            // Возможно, открывается диалоговое окно для ввода данных
            // Затем добавляем нового работника в коллекцию
            Users.Add(new User() 
            {
                Id = 1,
                Name = "name",
                Surname = "surname",
                Role = Enums.UserRoles.BasicEmployee,
            });
        }

        private void DeleteEmployee()
        {
            // Логика удаления выбранного работника
            if (false)
            {
                return;
            }
            Users.Remove(SelectedUser);
            SelectedUser= null;
        }

        private bool CanDeleteEmployee()
        {
            // Включаем кнопку удаления только при наличии выбранного работника
            return SelectedUser!= null;
        }

        private void EditEmployee()
        {
            // Логика изменения выбранного работника
            // Возможно, открывается диалоговое окно для редактирования данных
            // Затем обновляем свойства выбранного работника
            // Например: SelectedEmployee.Name = "Новое имя";
            //          SelectedEmployee.Position = "Новая должность";
        }

        private bool CanEditEmployee()
        {
            // Включаем кнопку изменения только при наличии выбранного работника
            return SelectedUser!= null;
        }

        private void UpdateCommands()
        {
            // Вызываем это метод после изменения выбранного работника
            // для обновления состояния команд
            ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
        }
    }
}
