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
                return ConvertValue.To<T>(rawValue);
            }
            catch (Exception e)
            {
                //TODO: add info about key
                throw;
            }
        }
    }
}