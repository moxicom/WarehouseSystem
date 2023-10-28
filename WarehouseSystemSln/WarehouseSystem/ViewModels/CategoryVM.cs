using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class CategoryVM : BaseItemListVM<object>
    {
        // По сути я должен отсюда иметь возможность возвращаться обратно к категориям,
        // а для этого необходимо иметь доступ к методам класса `MainViewModel`
        
        // fields

        // constructor
        public CategoryVM(int ID, string baseUrl, MainViewModel mainVM) : base(baseUrl, mainVM, "Товары отсутствуют",
            "Загрузка...")
        {
            StatusTextValue = ID.ToString();
            IsStatusTextVisible = true;
        }

        // methods
    }
}
