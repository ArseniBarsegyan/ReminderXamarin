using System;
using System.Collections.Generic;

namespace ReminderXamarin.DependencyResolver
{
    public static class ComponentFactory
    {
        public static T Resolve<T>() where T : class
        {
            return ComponentRegistry.Container.Resolve<T>();
        }

        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return ComponentRegistry.Container.ResolveAll<T>();
        }

        public static object Resolve(Type type)
        {
            return ComponentRegistry.Container.Resolve(type);
        }

        public static IEnumerable<object> ResolveAll(Type type)
        {
            return ComponentRegistry.Container.ResolveAll(type);
        }
    }
}