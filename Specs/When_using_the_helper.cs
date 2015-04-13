namespace Specs
{
    using System;
    using BetterConfig;
    using Xunit;

    public class When_using_the_helper
    {
        private static readonly Action<string, string> SetEnvVar =
            (variable, value) => EnvVarHelper.SetEnvVar(value, EnvironmentVariableTarget.Process, variable);

        public When_using_the_helper()
        {
            SetEnvVar("lucky", null);
        }

        [Fact]
        public void Can_retrieve_an_int()
        {
            SetEnvVar("lucky", "7");
            var lucky = ConfigurationHelper.Get<int>("lucky");

            Assert.Equal(7, lucky);
        }

        [Fact]
        public void Can_retrieve_a_string()
        {
            SetEnvVar("lucky", "7");
            var lucky = ConfigurationHelper.Get<string>("lucky");

            Assert.Equal("7", lucky);
        }

        [Fact]
        public void Can_retrieve_a_GUID()
        {
            var original = Guid.NewGuid();
            SetEnvVar("lucky", original.ToString());
            var lucky = ConfigurationHelper.Get<Guid>("lucky");

            Assert.Equal(original, lucky);
        }

        [Fact]
        public void Can_retrieve_a_TimeSpan()
        {
            var original = new TimeSpan(0, 1, 2, 3);
            SetEnvVar("lucky", original.ToString());
            var lucky = ConfigurationHelper.Get<TimeSpan>("lucky");

            Assert.Equal(original, lucky);
        }

        [Fact]
        public void Throws_appropriate_exception_on_conversion_error_for_timespan()
        {
            SetEnvVar("lucky", "not a timespan");

            Assert.Throws<FormatException>(() => { ConfigurationHelper.Get<TimeSpan>("lucky"); });
        }

        [Fact]
        public void Throws_appropriate_exception_on_conversion_error_for_int()
        {
            SetEnvVar("lucky", "not an int");

            Assert.Throws<FormatException>(() => { ConfigurationHelper.Get<int>("lucky"); });
        }
    }
}