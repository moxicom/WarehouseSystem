using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
