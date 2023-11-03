using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Models
{
    internal class GetCategoryResponseData
    {
        public string Title { get; set; } = "";
        public List<Item>? Items { get; set; }
    }
}
