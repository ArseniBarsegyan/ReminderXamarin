using System;
using RmApp.Core.Enums;
using RmApp.Core.Services.IoC;

namespace RmApp.Core.DependencyResolver
{
    public static class ComponentRegistry
    {
        public static IContainerService Container { get; set; }

        public static void Register<T>(LifeStyle lifeStyle = LifeStyle.Singleton) where T : class
        {
            Container.Register<T>(lifeStyle);
        }

        public static void Register<T>(T instance, LifeStyle lifeStyle = LifeStyle.Singleton) where T : class
        {
            Container.Register(instance, lifeStyle);
        }

        public static void RegisterCollection<T>(Type[] implementations) where T : class
        {
            Container.RegisterCollection<T>(implementations);
        }

        public static void Register<T, TN>(LifeStyle lifeStyle = LifeStyle.Singleton)
            where T : class where TN : class, T
        {
            Container.Register<T, TN>(lifeStyle);
        }
    }
}