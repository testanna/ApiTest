using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using System.Collections.Generic;
using ApiTest.ApiHelper;
using ApiTest.View;

namespace ApiTest.Test
{
    [TestClass]
    public class GetTest
    {
        PostmanApi _api;
        PostmanView _view;

        [TestInitialize]
        public void Initialize()
        {
            _view = new PostmanView
            {
                Url = "https://postman-echo.com/get?&foo1=bar1&foo2=bar2",
                Args = new Dictionary<string, string> { }
            };
            _view.Args.Add("foo1", "bar1");
            _view.Args.Add("foo2", "bar2");
            //Здесь должно быть создание объекта PostmanView через PostmanAPI
        }

        [TestMethod]
        public void SuccessGet()
        {
            _api = new PostmanApi("postman", "password");
            IRestResponse<PostmanView> response = _api.GetPostman("get?", Method.GET, 
                new Dictionary<string, string> { { "foo1", "bar1" }, { "foo2", "bar2" } });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse("".Equals(response.Content));
            
            PostmanView responseView = response.Data;

            Assert.AreEqual(_view.Url, responseView.Url, 
                "Метод возвращает некорректное значение Url");
            Assert.AreEqual(_view.Args.Count, responseView.Args.Count, 
                "Метода возвращает некорректное количество аргументов Args");
            //Еще проверки объекта
        }

        [TestCleanup]
        public void CleanUp()
        {
            //Здесь должно быть удаление объекта PostmanView через PostmanAPI 
        }
    }
}
