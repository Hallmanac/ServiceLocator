using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Hallmanac.ServiceLocator.Core;
using Hallmanac.ServiceLocator.Tests.ClassesToRegister;


namespace Hallmanac.ServiceLocator.Tests
{
    public class StartupConfiguration
    {
        public static void RunBootstrapper(IServiceLocator serviceLocator)
        {
            
        }

        public static ITypeCatalog GetTypeCatalog(IServiceLocator serviceLocator)
        {
            var typeCatalog = serviceLocator.Resolve<ITypeCatalog>();
            if (typeCatalog != null)
            {
                return typeCatalog;
            }
            var assemblies = serviceLocator.Resolve<List<Assembly>>();
            if (assemblies == null || assemblies.Count < 1)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            }
            serviceLocator.RegisterInstance(assemblies);
            typeCatalog = new AppDomainTypeCatalog(assemblies);
            serviceLocator.Register(() => typeCatalog);
            return typeCatalog;
        }
    }
}