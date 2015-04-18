namespace BetterConfig
{
    using System;
    using System.Collections.Generic;

    public class SettingNotFoundException : Exception
    {
        public string SettingKey { get; private set; }
        public string Explanation { get; private set; }

        public SettingNotFoundException(string settingKey, ICollection<string> strategies)
        {
            SettingKey = settingKey;
            Explanation = String.Format(
                "The setting, {0}, was not found.{1}Attempted to locate the setting using:{1}{2}",
                settingKey,
                Environment.NewLine,
                String.Join(Environment.NewLine, strategies));
        }
    }
}