using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Hallmanac.ServiceLocator.Core;
using Hallmanac.ServiceLocator.Ninject;
using Hallmanac.ServiceLocator.Tests.ClassesToRegister;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.ServiceLocator.Tests.Ninject
{
    /// <summary>
    /// Summary description for NinjectServiceLocatorTests
    /// </summary>
    [TestClass]
    public class NinjectServiceLocatorTests
    {
        private readonly IServiceLocator _serviceLocator;

        private TestContext testContextInstance;


        public NinjectServiceLocatorTests()
        {
            _serviceLocator = new NinjectServiceLocator();
            StartupConfiguration.RunBootstrapper(_serviceLocator);
        }


        [TestMethod]
        public void StartupTest()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            _serviceLocator.RegisterInstance(assemblies);
            ITypeCatalog typeCatalog = new AppDomainTypeCatalog(assemblies);
            _serviceLocator.Register(() => typeCatalog);
            var localTypeCatalog = _serviceLocator.Resolve<ITypeCatalog>();
            var localAssembliesInstance = _serviceLocator.Resolve<List<Assembly>>();

            Assert.IsInstanceOfType(localTypeCatalog, typeof (AppDomainTypeCatalog));
            Assert.AreEqual(typeCatalog, localTypeCatalog);
            Assert.IsTrue(typeCatalog.Equals(localTypeCatalog));
            Assert.IsTrue(ReferenceEquals(typeCatalog, localTypeCatalog));
        }


        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion
    }
}