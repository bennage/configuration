namespace BetterConfig
{
    using System;

    public static class ConfigurationHelper
    {
        public static T Get<T>(string key)
        {
            var rawValue = Configuration.GetValueFor(key);
            try
            {
                if (rawValue.HasValue)
                    return ConvertValue.To<T>(rawValue.Value);
                throw new Exception("?");
            }
            catch (Exception e)
            {
                //TODO: add info about key
                throw;
            }
        }
    }
}