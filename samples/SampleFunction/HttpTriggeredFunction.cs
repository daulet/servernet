using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SampleFunction.Model;
using Servernet;

namespace SampleFunction
{
    public class HttpTriggeredFunction : IFunction<HttpResponseMessage>
    {
        private readonly HttpRequestMessage _request;

        public HttpTriggeredFunction([HttpTrigger] HttpRequestMessage request)
        {
            _request = request;
        }

        public HttpResponseMessage Run()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
