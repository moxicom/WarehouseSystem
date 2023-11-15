using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.ViewModels;
using WarehouseSystem.Views;

namespace WarehouseSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            var window = new AppMainWindow();
            User user = new User()
            {
                Id = 1,
                Name = "TestName",
                Surname = "TestSurname",
                Role = Enums.UserRoles.Admin,
            };
            window.DataContext = new MainViewModel("http://localhost:8080", user);
            Run(window);

            //Run(new MainWindow());
        }
    }
}
