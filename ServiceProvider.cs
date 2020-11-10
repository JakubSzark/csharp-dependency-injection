using System;
using System.Collections.Generic;

namespace Szark.DI
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly List<Service> _services;
        private readonly List<object> _singletons;

        internal ServiceProvider(List<Service> services)
        {
            _services = services;
            _singletons = new List<object>();

            // We need to create all the singleton services first
            foreach (var service in services)
            {
                if (service.ServiceType == ServiceType.Singleton)
                {
                    var singleton = Activator.CreateInstance(service.ConcreteType);
                    if (singleton != null) _singletons.Add(singleton);
                }
            }

            // Then we need to assign the properties of those singletons
            foreach (var singleton in _singletons)
                AssignServiceProperties(singleton);
        }

        /// <summary>
        /// Fills in the public properties of a service
        /// </summary>
        private void AssignServiceProperties(object serviceObj)
        {
            var properties = serviceObj.GetType().GetProperties();
            if (properties == null || properties.Length == 0) return;

            // We want to go through all this service's properties
            foreach (var property in properties)
            {
                // Then we find the concrete type for the property
                foreach (var service in _services)
                {
                    if (service.AbstractType == property.PropertyType)
                    {
                        // We need to check is that concrete type is a singleton
                        var propVal = _singletons.Find(o => o.GetType() == service.ConcreteType);
                        if (propVal != null) property.SetValue(serviceObj, propVal);
                        else property.SetValue(serviceObj, Get(service.ConcreteType));

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an instance of a service
        /// </summary>
        internal object Get(Type concreteType)
        {
            object? result = _singletons.Find(o => o.GetType() == concreteType);

            if (result != null) return result;
            else
            {
                var service = _services.Find(s => s.ConcreteType == concreteType);

                if (service != null)
                {
                    result = Activator.CreateInstance(concreteType);
                    if (result != null) AssignServiceProperties(result);
                    return result ?? throw new InvalidOperationException("Failed " +
                        $"to create service of type {concreteType.FullName}");
                }
            }

            throw new InvalidOperationException($"Type {concreteType.FullName}"
                + " is not a registered service!");
        }

        /// <summary>
        /// Returns a instance of a service.
        /// If the service is a singleton then that is returned.
        /// Otherwise a new instance is created.
        /// </summary>
        public T Get<T>() where T : class => (T)Get(typeof(T));
    }
}
