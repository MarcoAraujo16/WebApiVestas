using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebApiVestas.BD_persistent;
using WebApiVestas.Models;

namespace WebApiVestas
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            container.RegisterType<ISection, SectionRepository>(new HierarchicalLifetimeManager());
        }
    }
}