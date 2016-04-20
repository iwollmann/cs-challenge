using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace cs_challenge.Tests
{
    public class HttpApplication : HttpServer
    {
        public HttpApplication(string baseUrl) : base(GetConfiguration(baseUrl))
        {
        }

        private static HttpConfiguration GetConfiguration(string baseUrl)
        {
            var routeCollection = new HttpRouteCollection(baseUrl);
            var configuration = new HttpConfiguration(routeCollection);
            WebApiConfig.Register(configuration);

            return configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);  
        }
    }
}
