namespace Specs
{
    using System;
    using System.Configuration;
    using BetterConfig;
    using Xunit;
    using Configuration = BetterConfig.Configuration;

    public class TheLocalHelper
    {
        const string PropertyName = "IMyConfiguration.SqlConnectionString";

        [Fact]
        public void xyz()
        {
            LocalHelper.Test("myTest");
        }

       
    }
}