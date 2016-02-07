using System;
using System.Runtime.Serialization;


namespace Hallmanac.ServiceLocator.Core
{
    [Serializable]
    public class ServiceResolutionException : Exception
    {
        public ServiceResolutionException(Type service)
            :
                base($"Could not resolve serviceType '{service}'")
        {
            ServiceType = service;
        }


        public ServiceResolutionException(Type service, Exception innerException)
            : base($"Could not resolve serviceType '{service}'", innerException)
        {
            ServiceType = service;
        }


        public ServiceResolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public Type ServiceType { get; set; }
    }
}