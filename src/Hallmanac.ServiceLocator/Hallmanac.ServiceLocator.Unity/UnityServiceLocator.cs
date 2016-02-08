using System;
using System.Collections.Generic;
using System.Linq;

using Hallmanac.ServiceLocator.Core;

using Microsoft.Practices.Unity;


namespace Hallmanac.ServiceLocator.Unity
{
    public class UnityServiceLocator : IServiceLocator
    {
        public UnityServiceLocator()
            : this(new UnityContainer())
        {
        }


        public UnityServiceLocator(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container), "The specified Unity container cannot be null.");
            }

            Container = container;
        }

        public IUnityContainer Container { get; }


        public T Resolve<T>() where T : class
        {
            try
            {
                return Container.Resolve<T>();
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(typeof (T), ex);
            }
        }


        public T Resolve<T>(string key) where T : class
        {
            try
            {
                return Container.Resolve<T>(key);
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(typeof (T), ex);
            }
        }


        public object Resolve(Type type)
        {
            try
            {
                return Container.Resolve(type);
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(type, ex);
            }
        }


        public IList<T> ResolveServices<T>() where T : class
        {
            var services = Container.ResolveAll<T>();
            return new List<T>(services);
        }


        public T ResolveWithParameters<T>(params object[] argumentParameters) where T : class
        {
            return argumentParameters.Length > 0
                ? Container.Resolve<T>(argumentParameters.OfType<ParameterOverride>().Cast<ResolverOverride>().ToArray())
                : Container.Resolve<T>();
        }


        public void Register<TInterface>(Type implType) where TInterface : class
        {
            var key = $"{typeof (TInterface).Name}-{implType.FullName}";
            Container.RegisterType(typeof (TInterface), implType, key);

            // Work-around, also register this implementation to service mapping
            // without the generated key above.
            Container.RegisterType(typeof (TInterface), implType);
        }


        public void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface
        {
            Container.RegisterType<TInterface, TImplementation>();
        }


        public void RegisterNamedType<TInterface, TImplementation>(string key)
            where TImplementation : class, TInterface
        {
            Container.RegisterType<TInterface, TImplementation>(key);
        }


        public void Register(string key, Type type)
        {
            Container.RegisterType(type, key);
        }


        public void Register(Type serviceType, Type implType)
        {
            Container.RegisterType(serviceType, implType);
        }


        public void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class
        {
            var container = Container;
            Func<IUnityContainer, object> factoryFunc = c => factoryMethod.Invoke();
            container.RegisterType<TInterface>(new InjectionFactory(factoryFunc));
        }


        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            Container.RegisterInstance(instance);
        }


        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Container.RegisterType<TInterface, TImplementation>(new ContainerControlledLifetimeManager());
        }


        public void RegisterSingleton<TInterface>(Type implType) where TInterface : class
        {
            var key = $"{typeof (TInterface).Name}-{implType.FullName}";
            Container.RegisterType(typeof (TInterface), implType, key, new ContainerControlledLifetimeManager());

            // Work-around, also register this implementation to service mapping
            // without the generated key above.
            Container.RegisterType(typeof (TInterface), implType, new ContainerControlledLifetimeManager());
        }


        public void RegisterSingleton(string key, Type type)
        {
            Container.RegisterType(type, key, new ContainerControlledLifetimeManager());
        }


        public void Release(object instance)
        {
            if (instance == null)
            {
                return;
            }

            Container.Teardown(instance);
        }


        public void Reset()
        {
            Dispose();
        }


        public TService Inject<TService>(TService instance) where TService : class
        {
            return instance == null ? null : (TService) Container.BuildUp(instance.GetType(), instance);
        }


        public void TearDown<TService>(TService instance) where TService : class
        {
            if (instance == null)
            {
                return;
            }
            Container.Teardown(instance);
        }


        public T Resolve<T>(Type type) where T : class
        {
            try
            {
                return Container.Resolve(type) as T;
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(type, ex);
            }
        }


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
            Container.Dispose();

            // Set disposed equals to true
            _disposed = true;
        }

        #endregion
    }
}