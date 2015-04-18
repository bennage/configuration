namespace Specs
{
    using BetterConfig;
    using System;
    using System.Linq;
    using Xunit;

    public class When_a_value_cannot_be_converted
    {
        const string unparsedValue = "something not parsable to int";
        const string settingKey = "IMyConfig.SomeNumber";

        [Fact]
        public void capture_the_target_type()
        {
            using (EnvironmentVariable.ProcessLevel(settingKey, unparsedValue))
            {
                var agg = Assert.Throws<AggregateException>(() =>
                {
                    Configuration.For<IMyConfig>();
                });

                var e = (SettingConversionException)agg.Flatten().InnerExceptions.First();

                Assert.Equal(typeof (int), e.TargetType);
            }
        }

        [Fact]
        public void capture_the_unparsed_value()
        {
            using (EnvironmentVariable.ProcessLevel(settingKey, unparsedValue))
            {
                var agg = Assert.Throws<AggregateException>(() =>
                {
                    Configuration.For<IMyConfig>();
                });

                var e = (SettingConversionException)agg.Flatten().InnerExceptions.First();

                Assert.Equal(unparsedValue, e.UnparsedValue);
            }
        }

        [Fact]
        public void capture_the_setting_key()
        {
            using (EnvironmentVariable.ProcessLevel(settingKey, unparsedValue))
            {
                var agg = Assert.Throws<AggregateException>(() =>
                {
                    Configuration.For<IMyConfig>();
                });

                var e = (SettingConversionException)agg.Flatten().InnerExceptions.First();

                Assert.Equal(settingKey, e.SettingKey);
            }
        }

        [Fact]
        public void contain_relevant_data_in_the_message()
        {
            using (EnvironmentVariable.ProcessLevel(settingKey, unparsedValue))
            {
                var agg = Assert.Throws<AggregateException>(() =>
                {
                    Configuration.For<IMyConfig>();
                });

                var e = (SettingConversionException)agg.Flatten().InnerExceptions.First();

                Assert.True(e.Message.Contains(settingKey), "It should contain the setting key.");
                Assert.True(e.Message.Contains(unparsedValue), "It should contain the unparsed value.");
                Assert.True(e.Message.Contains(typeof(int).Name), "It should contain the target type's name.");
            }
        }

        public interface IMyConfig
        {
            int SomeNumber { get; }
        }
    }
}