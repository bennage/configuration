namespace Specs
{
    using BetterConfig;
    using Xunit;
    using Configuration = BetterConfig.Configuration;

    public class When_using_local_dev_config
    {
        [Fact]
        public void load_values_from_well_known_yml_file()
        {
            Configuration.ValueStrategies.Add(LocalDebugging.Strategy());

            var config = Configuration.For<IExampleForLocal>();

            Assert.Equal(42, config.SomeNumber);
        }

        public interface IExampleForLocal
        {
            int SomeNumber { get; }
        }
    }
}