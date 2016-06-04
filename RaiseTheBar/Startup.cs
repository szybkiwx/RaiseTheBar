using Microsoft.Owin;
using Owin;
using RaiseTheBar;
using System.Web.Http;
using Unity.WebApi;

[assembly: OwinStartup(typeof(RiseTheBar.Startup))]
namespace RiseTheBar
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);



            httpConfiguration.DependencyResolver = new UnityDependencyResolver(UnityHelpers.GetConfiguredContainer());

            appBuilder.UseWebApi(httpConfiguration);
           
        }
    }
}