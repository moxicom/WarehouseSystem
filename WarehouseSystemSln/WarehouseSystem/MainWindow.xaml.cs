﻿using System;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
<<<<<<< Updated upstream:WarehouseSystemSln/WarehouseSystem/MainWindow.xaml.cs
    public partial class MainWindow : Window
    {
        public MainWindow()
=======
    public partial class AppMainWindow : Window
    {
        public AppMainWindow()
>>>>>>> Stashed changes:WarehouseSystemSln/WarehouseSystem/Views/MainWindow.xaml.cs
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
