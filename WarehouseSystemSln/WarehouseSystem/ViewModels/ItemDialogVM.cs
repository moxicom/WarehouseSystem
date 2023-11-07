using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class ItemDialogVM: BaseViewModel
    {
        public event EventHandler<string> DialogClosing;
        public string DataToSend { get; set; }

        public ItemDialogVM() 
        {

        }

        public ICommand? OkButtonCommand => new RelayCommand(OkButtonClickHandler);
        public ICommand? CancelButtonCommand => new RelayCommand(CancelButtonClickHandler);

        private void OkButtonClickHandler()
        {
            DataToSend = "Hello from dialog window";
            DialogClosing?.Invoke(this, DataToSend);
        }

        private void CancelButtonClickHandler()
        {
            DialogClosing?.Invoke(this, null);
        }
    }
}
