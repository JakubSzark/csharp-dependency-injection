using System;

namespace Szark.DI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public readonly Type Interface;
        public readonly ServiceType ServiceType;

        public ServiceAttribute(Type serviceInterface, 
            ServiceType type = ServiceType.Temporary)
        {
            Interface = serviceInterface;
            ServiceType = type;
        }
    }
}
