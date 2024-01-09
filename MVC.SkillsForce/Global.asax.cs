using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BusinessLayer.SkillsForce.Interface;
using Hangfire;
using Hangfire.SqlServer;
using MVC.SkillsForce.App_Start;

namespace MVC.SkillsForce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public string databaseConnectionstring = ConfigurationManager.ConnectionStrings["HangFireConnectionString"].ConnectionString;

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(databaseConnectionstring)
                .UseActivator(new ContainerJobActivator(UnityConfig.Container));


            yield return new BackgroundJobServer();
        }


        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           HangfireAspNet.Use(GetHangfireServers);

            // Let's also Schedule a sample background job
             BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));

            //BackgroundJob.Schedule<IEnrollmentService>(x => x.RunAutomaticSelectionOfApprovedEnrollmentsAsync(true), TimeSpan.FromMinutes(1));
        }



    }
}
