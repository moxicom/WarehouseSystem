using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            window.DataContext = new MainViewModel("http://localhost:8080");
            Run(window);
            //Run(new MainWindow());
        }
    }
}
