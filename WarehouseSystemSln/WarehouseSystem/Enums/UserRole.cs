using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Enums
{
    public enum UserRoles
    {
        Guest = 0,      // Can only see all categories and items
        BasicEmployee,  // + Can update categories and items
        Moderator,      // + Can add and delete categories and items
        Admin,          // + Can add new users (and also can see admin panel to do it)
    }
}
