using System;
using System.Collections.Generic;
using System.Linq;

using Hallmanac.ServiceLocator.Core;

using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Parameters;


namespace Hallmanac.ServiceLocator.Ninject
{
    public class NinjectServiceLocator : IServiceLocator, TestMe
    {
        public NinjectServiceLocator(IKernel kernel = null)
        {
            Kernel = kernel ?? new StandardKernel();
        }


        public T Resolve<T>() where T : class
        {
            return Kernel.Get<T>();
        }


        public T Resolve<T>(string key) where T : class
        {
            return Kernel.Get<T>(key);
        }


        public object Resolve(Type type)
        {
            return Kernel.Get(type);
        }


        public IList<T> ResolveServices<T>() where T : class
        {
            var services = Kernel.GetAll<T>();
            return new List<T>(services);
        }


        public void Register<TInterface>(Type implType) where TInterface : class
        {
            Kernel.Bind<TInterface>().To(implType);
        }


        public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>();
        }


        public void RegisterNamedType<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>().Named(key);
        }


        public void Register(string key, Type type)
        {
            Kernel.Bind(type).To(type).Named(key);
        }


        public void Register(Type serviceType, Type implType)
        {
            Kernel.Bind(serviceType).To(implType);
        }


        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            Kernel.Bind<TInterface>().ToConstant(instance);
        }


        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>().InSingletonScope();
        }


        public void RegisterSingleton<TInterface>(Type implType) where TInterface : class
        {
            Kernel.Bind<TInterface>().To(implType).InSingletonScope();
        }


        public void RegisterSingleton(string key, Type type)
        {
            Kernel.Bind(type).ToSelf().InSingletonScope().Named(key);
        }


        public void Release(object instance)
        {
            if (instance == null)
            {
                return;
            }
            Kernel.Release(instance);
        }


        public void Reset()
        {
            Dispose();
        }


        public TService Inject<TService>(TService instance) where TService : class
        {
            return null;
        }


        public void TearDown<TService>(TService instance) where TService : class
        {
            if (instance == null)
            {
                return;
            }
            Kernel.Release(instance);
        }


        public void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class
        {
            var kernel = Kernel;

            kernel.Bind<TInterface>().ToMethod(context => factoryMethod());
        }


        public T ResolveWithParameters<T>(params object[] argumentParameters) where T : class
        {
            return argumentParameters.Length > 0
                ? Kernel.Get<T>(argumentParameters.OfType<ConstructorArgument>().Cast<IParameter>().ToArray())
                : Kernel.Get<T>();
        }


        public void RegisterTypesAutomatically(Action<object> registrationFunction = null)
        {
            if (registrationFunction == null)
            {
                Kernel.Bind(x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface());
            }
            else
            {
                registrationFunction(Kernel);
            }
        }


        public IKernel Kernel { get; }


        #region --- Dispose implementation ---

        private bool _disposed;


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Pattern for handling multiple calls to Dispose and allowing sub-classes to implment their own dispose pattern that
        ///     could
        ///     also include handling any calls to finalize from the GC.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (!disposing)
            {
                return;
            }

            // Run your cleanup logic
            Kernel.Dispose();

            // Set disposed equals to true
            _disposed = true;
        }

        #endregion
    }


    public interface TestMe
    {
        void RegisterTypesAutomatically(Action<object> registrationFunction = null);
    }
}