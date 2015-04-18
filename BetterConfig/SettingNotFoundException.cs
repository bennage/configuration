namespace BetterConfig
{
    using System;

    public class SettingNotFoundException : Exception
    {
        public string SettingKey { get; private set; }

        public SettingNotFoundException(string settingKey)
        {
            SettingKey = settingKey;
        }
    }
}