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
        private string _baseURL;

        public ICommand HomeBtnClick { get; }
        public ICommand CategoriesBtnClick { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(string baseURL)
        {
            HomeBtnClick = new RelayCommand(OpenHomeView);
            CategoriesBtnClick = new RelayCommand(OpenCategoriesView);

            OpenHomeView();

            _baseURL = baseURL;
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
        protected void OpenCategoriesView() => CurrentViewModel = new CategoriesVM(_baseURL);
        protected void OpenCategoryView() => CurrentViewModel = new CategoryVM();
        
    }
}