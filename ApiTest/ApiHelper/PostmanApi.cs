using ApiTest.View;
using RestSharp;
using System;
using System.Collections.Generic;

namespace ApiTest.ApiHelper
{
    class PostmanApi
    {
        const string BaseUrl = "https://postman-echo.com";

        readonly IRestClient _client;

        public PostmanApi(string accountSid, string secretKey)
        {
            _client = new RestClient(BaseUrl);
            //Здесь была бы аутентификация
        }

        public IRestResponse<PostmanView> GetPostman(string resource, Method method, Dictionary<string, string> parameters)
        {
            var request = new RestRequest(resource, method);

            foreach (var p in parameters)
            {
                request.AddParameter(p.Key, p.Value);
            }

            return Execute<PostmanView>(request);
        }

        private IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            //Здесь была бы подстановка данных аутентификации
            var response = _client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var postmanException = new Exception(message, response.ErrorException);
                throw postmanException;
            }
            return response;
        }
    }
}
