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
        public static IDictionary<string, Func<string, string>> ValueStrategies = new Dictionary<string, Func<string, string>>()
        {
            { 
                "environmental variable (process)",
                key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process)
             },
             {
                 "environmental variable (user)",
                 key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User)
            },
            {   "ConfigurationManager.AppSettings",
                key => ConfigurationManager.AppSettings.Get(key)
            }
        };

        internal static Option<string> GetValueFor(string key)
        {
            foreach (var strategy in ValueStrategies.Values)
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

            var problems = LookupAndSetValuesFor(properties, rootName, expando);

            if (problems.Any()) throw new AggregateException(problems);

            return projected;
        }

        private static IList<Exception> LookupAndSetValuesFor(PropertyInfo[] properties, string rootName, IDictionary<string, object> expando)
        {
            var problems = new List<Exception>();
            foreach (var property in properties)
            {
                var key = rootName + "." + property.Name;

                var unparsedValue = GetValueFor(key);

                if (unparsedValue.HasValue)
                {
                    try
                    {
                        var value = ConvertValue.To(unparsedValue.Value, property.PropertyType);
                        expando[property.Name] = value;
                    }
                    catch (Exception e)
                    {
                        problems.Add(new SettingConversionException(key, unparsedValue.Value, property.PropertyType, e));
                        expando[property.Name] = unparsedValue.Value;
                    }
                }

                if (expando[property.Name] == null)
                    problems.Add(new SettingNotFoundException(key, ValueStrategies.Keys));
            }

            return problems;
        }

        private static IDictionary<string, object> InitializeConfigurationObject(PropertyInfo[] properties)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                //var defaultForType = GetDefaultValueForType(property.PropertyType);
                expando.Add(propertyName, null);
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