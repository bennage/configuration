namespace SampleWebRole
{
    using System.Web;
    using System.Web.Routing;
    using App_Start;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}