﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{ 
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public ICommand HomeBtnClick { get; }
        public ICommand CategoriesBtnClick { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            CurrentViewModel = new HomeVM();
            HomeBtnClick = new RelayCommand(OpenHomeView);
            CategoriesBtnClick = new RelayCommand(OpenCategoriesView);
        }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        private void OpenHomeView() => CurrentViewModel = new HomeVM();
        private void OpenCategoriesView() => CurrentViewModel = new CategoriesVM();
    }
}