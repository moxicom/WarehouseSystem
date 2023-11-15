using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Enums;

namespace WarehouseSystem.Models
{
    internal class UserDialogData
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoles Role { get; set; }
    }
}
