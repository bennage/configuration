namespace Specs
{
    using BetterConfig;
    using Xunit;

    public class When_setting_default_values
    {
        [Fact]
        public void return_the_value_that_was_set()
        {
            var config = Configuration.For<IHasSettableValues>(defaults =>
            {
                defaults.SomeValue = "default value";
            });
            Assert.Equal("default value", config.SomeValue);
        }

        [Fact]
        public void give_loaded_values_precedence_over_default_values()
        {
            const string expected = "from env var";

            using (EnvironmentVariable.ProcessLevel("IHasSettableValues.SomeValue", expected))
            {
                var config = Configuration.For<IHasSettableValues>(defaults =>
                {
                    defaults.SomeValue = "from default";
                });
                Assert.Equal(expected, config.SomeValue);
            }
        }

        public interface IHasSettableValues
        {
            string SomeValue { get; set; }
        }
    }
}