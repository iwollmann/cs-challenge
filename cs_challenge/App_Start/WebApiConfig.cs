using AutoMapper;
using cs_challenge.Data;
using cs_challenge.Mappers;
using cs_challenge.Services;
using Microsoft.Data.Entity;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UnityDependencyResolver.Lib;

namespace cs_challenge
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );

            var container = new UnityContainer();

            RegisterDependencies(container);

            config.DependencyResolver = new UnityWebApiDependencyResolver(container);
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Filters.Add(new NotImplExceptionFilterAttribute());
        }

        private static void RegisterDependencies(UnityContainer container)
        {
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterInstance<EfDbContext>(new EfDbContext(new InMemoryContext()));
            container.RegisterInstance<IMapper>(AutoMapperConfig.RegisterMappings());
        }
    }
}
