namespace Specs
{
    using System;
    using BetterConfig;
    using Xunit;

    public class When_using_ConfigurationHelper
    {
        [Fact]
        public void be_able_to_retrieve_an_int()
        {
            using (EnvironmentVariable.ProcessLevel("lucky", "7"))
            {
                var lucky = ConfigurationHelper.Get<int>("lucky");
                Assert.Equal(7, lucky);
            }
        }

        [Fact]
        public void be_able_to_retrieve_a_string()
        {
            using (EnvironmentVariable.ProcessLevel("lucky", "7"))
            {
                var lucky = ConfigurationHelper.Get<string>("lucky");
                Assert.Equal("7", lucky);
            }
        }

        [Fact]
        public void be_able_to_retrieve_a_GUID()
        {
            var original = Guid.NewGuid();

            using (EnvironmentVariable.ProcessLevel("lucky", original.ToString()))
            {
                var lucky = ConfigurationHelper.Get<Guid>("lucky");
                Assert.Equal(original, lucky);
            }
        }

        [Fact]
        public void be_able_to_retrieve_a_TimeSpan()
        {
            var original = new TimeSpan(0, 1, 2, 3);
            using (EnvironmentVariable.ProcessLevel("lucky", original.ToString()))
            {
                var lucky = ConfigurationHelper.Get<TimeSpan>("lucky");
                Assert.Equal(original, lucky);
            }
        }

        [Fact]
        public void throws_appropriate_exception_on_conversion_error_for_timespan()
        {
            using (EnvironmentVariable.ProcessLevel("lucky", "not a timespan"))
            {
                Assert.Throws<FormatException>(() => { ConfigurationHelper.Get<TimeSpan>("lucky"); });
            }
        }

        [Fact]
        public void throws_appropriate_exception_on_conversion_error_for_int()
        {
            using (EnvironmentVariable.ProcessLevel("lucky", "not a int"))
            {
                Assert.Throws<FormatException>(() => { ConfigurationHelper.Get<int>("lucky"); });
            }
        }
    }
}