using ReminderXamarin.Enums;
using System;
using System.Collections.Generic;

namespace ReminderXamarin.IoC
{
    public interface IContainerService
    {
        T Resolve<T>() where T : class;
        IEnumerable<T> ResolveAll<T>() where T : class;
        object Resolve(Type type);
        IEnumerable<object> ResolveAll(Type type);

        void Register<T>(LifeStyle lifeStyle = LifeStyle.Singleton) where T : class;
        void Register<T, TN>(LifeStyle lifeStyle = LifeStyle.Singleton) where T : class where TN : class, T;
        void Register<T>(T instance, LifeStyle lifeStyle = LifeStyle.Singleton) where T : class;
        void RegisterCollection<T>(Type[] implementations) where T : class;
    }
}