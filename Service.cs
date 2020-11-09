using System;
using System.Diagnostics.CodeAnalysis;

namespace DependencyInjection
{
    public enum ServiceType
    {
        Temporary,
        Singleton
    }

    public class Service : IEquatable<Service>
    {
        public ServiceType ServiceType;
        public Type ConcreteType, AbstractType;

        public Service(Type concreteType, Type abstractType, ServiceType serviceType = 0) =>
            (ConcreteType, AbstractType, ServiceType) = (concreteType, abstractType, serviceType);

        public bool Equals(Service? other) =>
            other != null && ConcreteType == other.ConcreteType;
    }
}
