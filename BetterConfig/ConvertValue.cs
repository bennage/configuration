namespace BetterConfig
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class ConvertValue
    {
        public static readonly IDictionary<Type, Func<string, object>> Map = new ConcurrentDictionary<Type, Func<string, object>>();

        static ConvertValue()
        {
            Map.Add(
                typeof(string), x => x);

            Map.Add(
                typeof(TimeSpan), x => TimeSpan.Parse(x));

            Map.Add(
                typeof(Guid), x => Guid.Parse(x));

            Map.Add(
                typeof(DateTimeOffset),
                x => DateTimeOffset.Parse(x, null, System.Globalization.DateTimeStyles.AssumeUniversal));
        }

        public static T To<T>(string rawVaule)
        {
            var targetType = typeof(T);

            if (!Map.ContainsKey(targetType))
            {
                //TODO: this could be extensible as well
                return targetType.IsEnum
                    ? (T)Enum.Parse(targetType, rawVaule)
                    : (T)Convert.ChangeType(rawVaule, targetType);
            }

            try
            {
                return (T)Map[targetType](rawVaule);
            }
            catch (Exception e)
            {
                // TODO: something other than trace?
                Trace.WriteLine(String.Format(
                    "Could not convert string '{0}' to type {1}: {2}",
                    rawVaule,
                    targetType.Name,
                    e));

                throw;
            }
        }
    }
}