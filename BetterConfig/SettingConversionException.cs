namespace BetterConfig
{
    using System;

    public class SettingConversionException : Exception
    {
        public string SettingKey { get; private set; }
        public string UnparsedValue { get; set; }
        public Type TargetType { get; set; }

        public SettingConversionException(string settingKey, string unparsedValue, Type targetType, Exception exception)
            : base(
            String.Format(
                "The setting, {0}, was found but the value could not be converted to {2}.{1} The value found was:{1}'{3}'",
                settingKey,
                Environment.NewLine,
                targetType.Name,
                unparsedValue),
            exception)
        {
            SettingKey = settingKey;
            UnparsedValue = unparsedValue;
            TargetType = targetType;
        }
    }
}