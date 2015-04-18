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
            return (T)To(rawVaule, typeof(T));
        }

        public static object To(string rawVaule, Type targetType)
        {
            try
            {
                if (!Map.ContainsKey(targetType))
                {
                    //TODO: this could be extensible as well
                    return targetType.IsEnum
                        ? Enum.Parse(targetType, rawVaule)
                        : Convert.ChangeType(rawVaule, targetType);
                }

                return Map[targetType](rawVaule);
            }
            catch (Exception e)
            {
                var msg = (String.Format(
                    "Could not convert string '{0}' to type {1}: {2}",
                    rawVaule,
                    targetType.Name,
                    e));

                throw new FormatException(msg, e);
            }
        }
    }
}