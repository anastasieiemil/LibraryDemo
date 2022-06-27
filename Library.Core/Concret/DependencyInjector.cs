using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Concret
{
    public static class DependencyInjector
    {
        /// <summary>
        /// Adds singleTone service.
        /// </summary>
        /// <typeparam name="IService"></typeparam>
        /// <typeparam name="ServiceModel"></typeparam>
        public static void AddSingletone<IService, ServiceModel>() where ServiceModel : class, new()
        {
            var service = CreateService<IService, ServiceModel>(ClassType.SINGLETONE);
            service.Obj = new ServiceModel();

            dependencies.Add(typeof(IService), service);
        }

        /// <summary>
        /// Adds transient service.
        /// </summary>
        /// <typeparam name="IService"></typeparam>
        /// <typeparam name="ServiceModel"></typeparam>
        public static void AddTransient<IService, ServiceModel>() where ServiceModel : class, new()
        {
            var service = CreateService<IService, ServiceModel>(ClassType.TRANSIENT);
            service.Obj = new ServiceModel();

            dependencies.Add(typeof(IService), service);
        }

        /// <summary>
        /// Gets the required service.
        /// </summary>
        /// <typeparam name="IService"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IService Get<IService>()
        {
            var type = typeof(IService);
            if (!dependencies.TryGetValue(type, out Service? service))
            {
                throw new Exception($"The service {type.AssemblyQualifiedName} was not registered.");
            }

            switch (service?.Type)
            {
                case ClassType.SINGLETONE:
                    {
                        return (IService)service?.Obj;
                    }
                case ClassType.TRANSIENT:
                    {
                        return (IService)Activator.CreateInstance(service.ConcretType);
                    }
                default:
                    throw new Exception($"The service {type.AssemblyQualifiedName} couldn't be resolved!");

            }
        }

        #region private

        /// <summary>
        /// Creates Service model based on the given params.
        /// </summary>
        /// <typeparam name="IService"></typeparam>
        /// <typeparam name="ServiceModel"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Service CreateService<IService, ServiceModel>(ClassType type) where ServiceModel : class
        {
            if (dependencies.ContainsKey(typeof(IService)))
            {
                throw new Exception("Type already registered");
            }
            else
            {
                return new Service
                {
                    ConcretType = typeof(ServiceModel),
                    Type = type
                };
            }
        }

        private static Dictionary<Type, Service> dependencies = new Dictionary<Type, Service>();

        internal class Service
        {
            public object? Obj { get; set; }
            public Type ConcretType { get; set; } = null!;

            public ClassType Type { get; set; }
        }


        #endregion 

        public enum ClassType : int
        {
            SINGLETONE = 0,
            TRANSIENT = 1,
        }

    }
}
