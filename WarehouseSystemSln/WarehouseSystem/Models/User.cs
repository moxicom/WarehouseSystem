using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname {  get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        User = 0,
        Admin = 1,
    }
}
