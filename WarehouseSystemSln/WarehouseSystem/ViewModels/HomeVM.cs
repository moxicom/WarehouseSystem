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
        // fields
        private string _fullname;

        // constructor
        public HomeVM(string name, string surname)
        {
            FullName = name + ' ' + surname;
        }

        // Properties
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
