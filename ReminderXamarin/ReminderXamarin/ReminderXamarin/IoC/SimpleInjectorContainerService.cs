using System;
using System.Collections.Generic;
using ReminderXamarin.Enums;
using SimpleInjector;

namespace ReminderXamarin.IoC
{
    public class SimpleInjectorContainerService : IContainerService
    {
        private readonly Container _container;

        public SimpleInjectorContainerService()
        {
            _container = new Container();
        }

        private Lifestyle Convert(LifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case LifeStyle.Singleton:
                    return Lifestyle.Singleton;
                case LifeStyle.Scoped:
                    return Lifestyle.Scoped;
                case LifeStyle.Transient:
                    return Lifestyle.Transient;
                default:
                    return Lifestyle.Singleton;
            }
        }

        public T Resolve<T>() where T : class
        {
            return _container.GetInstance<T>();
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return _container.GetAllInstances<T>();
        }

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return _container.GetAllInstances(type);
        }

        public void Register<T>(LifeStyle lifeStyle = LifeStyle.Singleton) where T : class
        {
            _container.Register<T>(Convert(lifeStyle));
        }

        public void Register<T, TN>(LifeStyle lifeStyle = LifeStyle.Singleton) where T : class where TN : class, T
        {
            _container.Register<T, TN>(Convert(lifeStyle));
        }

        public void Register<T>(T instance, LifeStyle lifeStyle = LifeStyle.Singleton) where T : class
        {
            _container.Register(() => instance, Convert(lifeStyle));
        }

        public void RegisterCollection<T>(Type[] implementations) where T : class
        {
            _container.Collection.Register<T>(implementations);
        }
    }
}