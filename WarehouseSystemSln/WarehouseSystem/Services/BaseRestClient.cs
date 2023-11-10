using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Services
{
    internal abstract class BaseRestClient
    {
        public RestClient Client { get; private set; }
        public string BaseUrl { get; private set; }

        public BaseRestClient(string baseUrl)
        {
            ChangeClientUrl(baseUrl);  
            BaseUrl = baseUrl;
        }

        public void ChangeClientUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            Client = new RestClient(new RestClientOptions { BaseUrl = new Uri(baseUrl), MaxTimeout = 5000 });
        }

        protected string ProcessRequestStatus(HttpStatusCode statusCode) 
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return "";
                case HttpStatusCode.NotFound:
                    return "Ресурс не существует на сервере";
                case HttpStatusCode.Unauthorized:
                    return "Отказано в доступе";
                case HttpStatusCode.InternalServerError:
                    return "Внутрисерверная ошибка";
                case 0:
                    return "Ответ от сервера не получен. Проверьте подключение";
                default:
                    return "";
            }
        }
    }
}
