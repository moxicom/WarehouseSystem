using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseSystem.ViewModels;

namespace WarehouseSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow(string baseUrl)
        {
            InitializeComponent();
            LoginVM loginViewModel = new LoginVM(baseUrl);
            loginViewModel.RequestClose += CloseWindow;
            DataContext = loginViewModel;
                
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}
