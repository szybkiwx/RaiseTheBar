using Microsoft.Practices.Unity;
using RaiseTheBar.Services;
using RiseTheBar.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace RiseTheBar
{
    public static class UnityHelpers
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<Startup>();
            
            container.RegisterType<IPlaceHttpClient, PlaceHttpClient>();
            container.RegisterType<IDefaultSearchSettings, DefaultSearchSettings>();
            container.RegisterType<HttpClient>(
                new InjectionFactory(x =>
                    new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["placesApiBase"]) }));

            container.RegisterType<IQueryStringBuilder, QueryStringBuilder>();
            container.RegisterType<IPlaceRetriever>(
                new InjectionFactory(x =>
                    new PlaceRetriever(
                        x.Resolve<IPlaceHttpClient>(),
                        x.Resolve<IQueryStringBuilder>(),
                        ConfigurationManager.AppSettings["apiKey"])));

        }

    }
}