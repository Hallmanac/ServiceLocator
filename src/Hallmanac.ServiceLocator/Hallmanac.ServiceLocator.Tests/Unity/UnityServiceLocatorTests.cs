using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Hallmanac.ServiceLocator.Core;
using Hallmanac.ServiceLocator.Tests.ClassesToRegister;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnityServiceLocator = Hallmanac.ServiceLocator.Unity.UnityServiceLocator;


namespace Hallmanac.ServiceLocator.Tests.Unity
{
    [TestClass]
    public class UnityServiceLocatorTests
    {
        private readonly IServiceLocator _serviceLocator;
        //private readonly ITypeCatalog _typeCatalog;


        public UnityServiceLocatorTests()
        {
            _serviceLocator = new UnityServiceLocator(new UnityContainer());
            StartupConfiguration.RunBootstrapper(_serviceLocator);

            //_typeCatalog = StartupConfiguration.GetTypeCatalog(_serviceLocator);
        }


        [TestMethod]
        public void StartupTest()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            _serviceLocator.RegisterInstance(assemblies);
            var typeCatalog = new AppDomainTypeCatalog(assemblies);
            _serviceLocator.Register<ITypeCatalog>(() => typeCatalog);
            var localTypeCatalog = _serviceLocator.Resolve<ITypeCatalog>();
            var localAssembliesInstance = _serviceLocator.Resolve<List<Assembly>>();

            Assert.IsInstanceOfType(localTypeCatalog, typeof (AppDomainTypeCatalog));
            Assert.AreEqual(typeCatalog, localTypeCatalog);
            Assert.IsTrue(typeCatalog.Equals(localTypeCatalog));
            Assert.IsTrue(ReferenceEquals(typeCatalog, localTypeCatalog));
        }
    }
}