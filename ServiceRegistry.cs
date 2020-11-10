using System;
using System.Collections.Generic;
using System.Reflection;

namespace Szark.DI
{
    public class ServiceRegistry
    {
        private readonly List<Service> _services;
        private readonly string[]? _args;

        public ServiceRegistry(string[]? args = null)
        {
            _args = args;
            _services = new List<Service>() {
                new Service(typeof(Args), typeof(IArgs), ServiceType.Singleton)
            };
        }

        /// <summary>
        /// Builds a registery of all services in the provided assembly.
        /// </summary>
        public ServiceRegistry RegisterAssembly(Assembly assembly)
        {
            foreach (var concrete in assembly.GetTypes())
            {
                var attr = concrete.GetCustomAttribute<ServiceAttribute>();
                if (attr != null && attr.Interface.IsInterface)
                {
                    var ctors = concrete.GetConstructors();
                    if (ctors[0].GetParameters().Length > 0)
                    {
                        throw new InvalidOperationException("Service " +
                            "cannot contain parameters in constructor!");
                    }

                    _services.Add(new Service(concrete,
                        attr.Interface, attr.ServiceType));
                }
            }

            return this;
        }

        /// <summary>
        /// Registers a service manually
        /// </summary>
        /// <typeparam name="T">The concrete type</typeparam>
        /// <typeparam name="K">The interface</typeparam>
        public ServiceRegistry Register<T, K>(ServiceType type = ServiceType.Temporary)
        {
            var service = new Service(typeof(T), typeof(K), type);
            if (!_services.Contains(service)) _services.Add(service);
            return this;
        }

        /// <summary>
        /// Runs the IBuildRunner
        /// </summary>
        public ServiceRegistry Run<T>() where T : class, IServiceRunner
        {
            var provider = new ServiceProvider(_services);

            var args = provider.Get<Args>();
            args.Arguments = _args;

            var runner = Activator.CreateInstance<T>();
            runner.Run(provider);

            return this;
        }
    }
}
