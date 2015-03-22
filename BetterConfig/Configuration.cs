namespace Bennage.Configuration.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using System.Linq;
    using ImpromptuInterface;

    public static class Configuration
    {

        public static IList<Func<string, string>> ValueStrategies = new List<Func<string, string>>()
        {
            key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process),
            key => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User),
            key => ConfigurationManager.AppSettings.Get(key)
        }; 

        private static string GetValueFor(string key)
        {
            foreach (var strategy in ValueStrategies)
            {
                var value = strategy(key);
                if (value != null) return value;
            }

            return string.Empty;
        }

        public static T For<T>()
        {
            var type = typeof (T);
            var rootName = type.Name;
            var properties = type.GetProperties();

            var expando = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in properties)
            {
                var key = rootName + "." + property.Name;
                var value = GetValueFor(key);
                expando.Add(property.Name, value);
            }

            return expando.ActLike();
        }
    }
}