namespace SampleWebRole
{
    using System;
    using System.EnterpriseServices.Internal;
    using BetterConfig;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public static class AzureConfigurationExtensions
    {
        static AzureConfigurationExtensions()
        {
            // TODO: can we ensure `ValueStrategies` is initialized?
            // review the order of execution
            Configuration.ValueStrategies.Insert(0, CloudConfigurationManager.GetSetting);

            // TODO: Hmm... when running in Azure, should we check the cloud config first?
        }

        public static string GetStorageDirectory(string name)
        {
            if (RoleEnvironment.IsAvailable)
            {
                var res = RoleEnvironment.GetLocalResource(name);
                return res.RootPath;
            }   
            else
            {
                var path = System.IO.Path.GetTempPath();

                try
                {
                    var fullPath = System.IO.Path.Combine(path, name);
                    if (!System.IO.Directory.Exists(fullPath))
                    {
                        var dirInfo = System.IO.Directory.CreateDirectory(fullPath);
                        return dirInfo.FullName;
                    }
                }
                catch (Exception)
                { }
                return path;
            }
        }
    }
}