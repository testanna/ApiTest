using System.Collections.Generic;

namespace ApiTest.View
{
    class PostmanView
    {
        public Dictionary<string, string> Args { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Url { get; set; }
    }
}
