using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class HomeVM : BaseViewModel
    {
        private string _fullname = string.Empty;

        public HomeVM(string name, string surname) => FullName = name + ' ' + surname;

        public string FullName
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged();
            }
        }
    }
}
