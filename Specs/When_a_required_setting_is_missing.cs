namespace Specs
{
    using System;
    using System.Linq;
    using BetterConfig;
    using Xunit;

    public class When_a_required_setting_is_missing
    {
        private readonly SettingNotFoundException _exception;

        public When_a_required_setting_is_missing()
        {
            var agg = Assert.Throws<AggregateException>(() =>
            {
                Configuration.For<IHasRequiredSetting>();
            });

            _exception = (SettingNotFoundException)agg.Flatten().InnerExceptions.First();
        }

        [Fact]
        public void throw_an_exception_if_the_value_is_not_found()
        {
            Assert.Equal("IHasRequiredSetting.SomethingRequired", _exception.SettingKey);
        }

        [Fact]
        public void contain_an_explanation_of_where_it_looked_for_the_setting()
        {
            Configuration.ValueStrategies.Keys.ToList().ForEach(
                strategy =>
                {
                    Assert.True(_exception.Explanation.Contains(strategy));
                });

            Assert.True(Configuration.ValueStrategies.Count > 0);
        }

        [Fact]
        public void contain_an_explanation_of_which_setting_it_was_looking_for()
        {
            Assert.True(_exception.Explanation.Contains("IHasRequiredSetting.SomethingRequired"));
        }


        public interface IHasRequiredSetting
        {
            string SomethingRequired { get; }
        }
    }
}