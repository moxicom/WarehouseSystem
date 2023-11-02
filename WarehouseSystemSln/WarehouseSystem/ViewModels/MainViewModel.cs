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
        private readonly CategoriesVM _categoriesVM;
        private readonly string _baseUrl;

        public ICommand HomeBtnClick { get; }
        public ICommand CategoriesBtnClick { get; }
                         
        public MainViewModel(string baseUrl)
        {
            CurrentViewModel = new HomeVM();
            _baseUrl = baseUrl;
            _categoriesVM = new CategoriesVM(baseUrl, this);
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

        public void OpenCategoriesView() => CurrentViewModel = _categoriesVM;
        public void OpenHomeView() => CurrentViewModel = new HomeVM();
        public void OpenCategoryView(int ID) => CurrentViewModel = new CategoryVM(ID, _baseUrl, this);
        
    }
}