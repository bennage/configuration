namespace Specs
{
    using System;
    using System.Configuration;
    using Xunit;
    using Configuration = BetterConfig.Configuration;

    public class When_loading_a_configuration
    {
        const string PropertyName = "IMyConfiguration.SqlConnectionString";

        [Fact]
        public void pick_up_process_level_environmental_variables()
        {
            var randomValue = Guid.NewGuid().ToString();

            using (EnvironmentVariable.ProcessLevel(PropertyName, randomValue))
            {
                var config = Configuration.For<IMyConfiguration>();
                Assert.Equal(randomValue, config.SqlConnectionString);
            }
        }

        [Fact]
        public void pick_up_user_level_environmental_variables()
        {
            var randomValue = Guid.NewGuid().ToString();

            using (EnvironmentVariable.UserLevel(PropertyName, randomValue))
            {
                var config = Configuration.For<IMyConfiguration>();
                Assert.Equal(randomValue, config.SqlConnectionString);
            }
        }

        [Fact]
        public void give_precedence_to_process_level_over_user_level()
        {
            var randomValue = Guid.NewGuid().ToString();

            using (EnvironmentVariable.ProcessLevel(PropertyName, randomValue))
            using (EnvironmentVariable.UserLevel(PropertyName, "not this value"))
            {
                var config = Configuration.For<IMyConfiguration>();
                Assert.Equal(randomValue, config.SqlConnectionString);
            }
        }

        [Fact]
        public void read_settings_from_config_file()
        {
            var expected = ConfigurationManager.AppSettings.Get(PropertyName);
            var config = Configuration.For<IMyConfiguration>();
            Assert.Equal(expected, config.SqlConnectionString);
        }

        public interface IMyConfiguration
        {
            string SqlConnectionString { get; }
        }
    }
}