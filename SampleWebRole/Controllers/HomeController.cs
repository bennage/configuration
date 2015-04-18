namespace SampleWebRole.Controllers
{
    using System;
    using System.Web.Mvc;
    using BetterConfig;
    using Microsoft.Azure;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Configuration.ValueStrategies.Add(CloudConfigurationManager.GetSetting);

            var config = Configuration.For<IMyConfiguration>();
            return View(config);
        }

        public ActionResult SetEnvVar()
        {
            Environment.SetEnvironmentVariable(
                variable: "IMyConfiguration.MyConnectionString",
                value: "set in process",
                target: EnvironmentVariableTarget.Process);

            return RedirectToAction("Index");
        }
    }

    public interface IMyConfiguration
    {
        string MyConnectionString { get; }
    }
}