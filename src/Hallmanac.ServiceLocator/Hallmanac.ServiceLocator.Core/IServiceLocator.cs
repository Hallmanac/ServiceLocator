#region License

// --- Modifications have been made to original code done by Javier ---\\

// Author: Javier Lozano <javier@lozanotek.com>
// Copyright (c) 2009-2010, lozanotek, inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion


using System;
using System.Collections.Generic;


namespace Hallmanac.ServiceLocator.Core
{
    public interface IServiceLocator : IDisposable
    {
        /// <summary>
        /// Resolves an interface or class to it's concrete implementation.
        /// </summary>
        T Resolve<T>() where T : class;


        /// <summary>
        /// Gets a pre-defined instance from the container with the name of the specified key.
        /// </summary>
        T Resolve<T>(string key) where T : class;

        /// <summary>
        /// Gets an instance of the type specified.
        /// </summary>
        object Resolve(Type type);


        /// <summary>
        /// Gets all available concrete instances of the type.
        /// </summary>
        IList<T> ResolveServices<T>() where T : class;


        /// <summary>
        /// Registers a binding for the specified interface or type
        /// </summary>
        void Register<TInterface>(Type implType) where TInterface : class;


        /// <summary>
        /// Registers a binding for the specified interface or type
        /// </summary>
        void Register<TInterface, TImplementation>() where TImplementation : class, TInterface;


        /// <summary>
        /// Registers a binding for the specified interface or type with a specified name.
        /// </summary>
        void RegisterNamedType<TInterface, TImplementation>(string key) where TImplementation : class, TInterface;


        /// <summary>
        /// Registers a binding for the specified interface or type with a specified name.
        /// </summary>
        void Register(string key, Type type);


        /// <summary>
        /// Registers a binding for the specified interface or type
        /// </summary>
        void Register(Type serviceType, Type implType);


        /// <summary>
        /// Registers a binding for the specified interface or type to a pre-defined instance.
        /// </summary>
        void RegisterInstance<TInterface>(TInterface instance) where TInterface : class;


        /// <summary>
        /// Registers a binding for the specified interface or type to a singleton
        /// </summary>
        void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface;


        /// <summary>
        /// Registers a binding for the specified interface or type to a singleton
        /// </summary>
        void RegisterSingleton<TInterface>(Type implType) where TInterface : class;


        /// <summary>
        /// Registers a binding for the specified interface or type to a singleton
        /// </summary>
        void RegisterSingleton(string key, Type type);


        /// <summary>
        /// Deactivates and releases the current given instance if it is being managed by the IOC container.
        /// </summary>
        void Release(object instance);


        /// <summary>
        /// Disposes the IOC instance
        /// </summary>
        void Reset();


        /// <summary>
        /// Not too sure what this should do.
        /// </summary>
        TService Inject<TService>(TService instance) where TService : class;


        /// <summary>
        /// Releases the instance out of the IOC container.
        /// </summary>
        void TearDown<TService>(TService instance) where TService : class;


        /// <summary>
        /// Registers a binding for the specified interface or type by running a function 
        /// for any kind of implementation logic that needs to be run when registering it to
        /// the container.
        /// </summary>
        void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class;


        /// <summary>
        /// Resolves an instance out of the container by using the given argument Parameters to pass into
        /// the constructor. Arguments must match up with what the constructor expects.
        /// </summary>
        T ResolveWithParameters<T>(params object[] argumentParameters) where T : class;
    }
}