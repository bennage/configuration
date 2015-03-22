namespace Specs
{
    using System;
    using System.Configuration;
    using Xunit;
    using Configuration = BetterConfig.Configuration;

    public class When_loading_a_configuration
    {
        const string PropertyName = "IMyConfiguration.SqlConnectionString";

        public When_loading_a_configuration()
        {
            SetEnvVar(null, EnvironmentVariableTarget.Process);
            SetEnvVar(null, EnvironmentVariableTarget.User);
            //SetEnvVar(null, EnvironmentVariableTarget.Machine); // requires elevated privileges
        }

        [Fact]
        public void process_level_environmental_variables_are_picked_up()
        {
            var randomValue = Guid.NewGuid().ToString();

            SetEnvVar(randomValue, EnvironmentVariableTarget.Process);

            var config = Configuration.For<IMyConfiguration>();
            Assert.Equal(randomValue, config.SqlConnectionString);
        }

        [Fact]
        public void user_level_environmental_variables_are_picked_up()
        {
            var randomValue = Guid.NewGuid().ToString();

            SetEnvVar(randomValue, EnvironmentVariableTarget.User);

            var config = Configuration.For<IMyConfiguration>();
            Assert.Equal(randomValue, config.SqlConnectionString);
        }

        [Fact]
        public void process_level_environmental_take_precedence_over_user_level()
        {
            var randomValue = Guid.NewGuid().ToString();

            SetEnvVar(randomValue, EnvironmentVariableTarget.Process);
            SetEnvVar("not this value", EnvironmentVariableTarget.User);

            var config = Configuration.For<IMyConfiguration>();
            Assert.Equal(randomValue, config.SqlConnectionString);
        }

        [Fact]
        public void can_load_from_config_file()
        { 
            var expected = ConfigurationManager.AppSettings.Get(PropertyName);
            var config = Configuration.For<IMyConfiguration>();
            Assert.Equal(expected, config.SqlConnectionString);
        }

        private static void SetEnvVar(string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(
                variable: PropertyName,
                value: value,
                target: target);
        }

        public interface IMyConfiguration
        {
            string SqlConnectionString { get; }
        }
    }
}