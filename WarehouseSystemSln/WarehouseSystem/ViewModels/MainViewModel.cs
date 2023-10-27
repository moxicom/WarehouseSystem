using GalaSoft.MvvmLight;
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
        private CategoriesVM _categoriesVM;

        private readonly string _baseUrl;

        public ICommand HomeBtnClick { get; }
        public ICommand CategoriesBtnClick { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
                         
        public MainViewModel(string baseUrl)
        {
            CurrentViewModel = new HomeVM();
            _baseUrl = baseUrl;
            _categoriesVM = new CategoriesVM(baseUrl);
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

        protected void OpenCategoriesView() => CurrentViewModel = _categoriesVM;
        private void OpenHomeView() => CurrentViewModel = new HomeVM();
        protected void OpenCategoryView() => CurrentViewModel = new CategoryVM();
        
    }
}