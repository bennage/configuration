namespace BetterConfig
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using Functional.Option;
    using ImpromptuInterface;

    public static class Configuration
    {
        public static IList<Func<string, string>> ValueStrategies = new List<Func<string, string>>()
        {
            key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process),
            key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User),
            key => ConfigurationManager.AppSettings.Get(key)
        };

        internal static Option<string> GetValueFor(string key)
        {
            foreach (var strategy in ValueStrategies)
            {
                var value = strategy(key);
                if (value != null) return Option.Some(value);
            }

            return Option.None;
        }

        public static T For<T>()
        {
            return For<T>(_ => { });
        }

        public static T For<T>(Action<T> setupDefaults)
        {
            var type = typeof(T);
            var rootName = type.Name;
            var properties = type.GetProperties();

            var expando = InitializeConfigurationObject(properties);

            T projected = expando.ActLike();
            setupDefaults(projected);

            LookupAndSetValuesFor(properties, rootName, expando);

            return projected;
        }

        private static void LookupAndSetValuesFor(PropertyInfo[] properties, string rootName, IDictionary<string, object> expando)
        {
            foreach (var property in properties)
            {
                var key = rootName + "." + property.Name;

                var value = GetValueFor(key);

                if (value.HasValue)
                    expando[property.Name] = value.Value;
            }
        }

        private static IDictionary<string, object> InitializeConfigurationObject(PropertyInfo[] properties)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var defaultForType = GetDefaultValueForType(property.PropertyType);
                expando.Add(propertyName, defaultForType);
            }

            return expando;
        }

        internal static object GetDefaultValueForType(Type type)
        {
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }
    }
}