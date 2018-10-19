using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using MassTransit;
using ServiceBus;

namespace WebPublisher
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IBusControl _bus;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FeatureFoldersRazorViewEngine());

            _bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                configurator.Host(new Uri("rabbitmq://192.168.56.101"), hostConfigurator =>
                {
                    hostConfigurator.Username("test");
                    hostConfigurator.Password("test");

                    hostConfigurator.PublisherConfirmation = true;
                });
            });
            _bus.Start();
            
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(c => _bus).AsImplementedInterfaces().SingleInstance();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_End()
        {
            _bus.Stop();
        }
    }

    public class FeatureFoldersRazorViewEngine : RazorViewEngine
    {
        public FeatureFoldersRazorViewEngine()
        {
            var featureFolderViewLocationFormats = new[]
            {
                "~/Features/{1}/{0}.cshtml",
                "~/Features/{1}/{0}.vbhtml",
                "~/Features/Shared/{0}.cshtml",
                "~/Features/Shared/{0}.vbhtml",
            };

            ViewLocationFormats = featureFolderViewLocationFormats;
            MasterLocationFormats = featureFolderViewLocationFormats;
            PartialViewLocationFormats = featureFolderViewLocationFormats;
        }
    }
}
