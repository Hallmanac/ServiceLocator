using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;


namespace Hallmanac.ServiceLocator.Tests.ClassesToRegister
{
    public class AppDomainTypeCatalog : ITypeCatalog
    {
        private readonly List<Type> _loadedTypes;


        public AppDomainTypeCatalog(IEnumerable<Assembly> assemblies)
        {
            _loadedTypes = LoadTypes(assemblies);
        }


        public Type[] LoadedTypes => _loadedTypes.ToArray();


        public Type[] GetDerivedTypes(Type type)
        {
            return (
                from derivedType in _loadedTypes
                where type != derivedType
                where type.IsAssignableFrom(derivedType)
                select derivedType).ToArray();
        }


        public Type[] GetDerivedTypes<T>()
        {
            return GetDerivedTypes(typeof(T));
        }


        public Type[] GetGenericInterfaceImplementations(Type type)
        {
            return (from derivedType in _loadedTypes
                    from interfaceType in derivedType.GetInterfaces()
                    where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == type
                    select derivedType).Distinct().ToArray();
        }


        public Type[] GetInterfaceImplementations(Type type)
        {
            return (from derivedType in _loadedTypes
                    where !derivedType.IsInterface
                    from interfaceType in derivedType.GetInterfaces()
                    where interfaceType == type
                    select derivedType).Distinct().ToArray();
        }


        public Type[] GetInterfaceImplementations<T>()
        {
            return GetInterfaceImplementations(typeof(T));
        }


        private static List<Type> LoadTypes(IEnumerable<Assembly> assemblies)
        {
            var loadedTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    loadedTypes.AddRange(types);
                }
                catch (ReflectionTypeLoadException exception)
                {
                    exception.LoaderExceptions
                        .Select(e => e.Message)
                        .Distinct().ToList()
                        .ForEach(message => Debug.WriteLine(message));
                }
            }

            return loadedTypes;
        }
    }
}