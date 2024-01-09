//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using System.Linq;
//using System.Web;
//using Hangfire;
//using Hangfire.SqlServer;
//using Owin;

//namespace MVC.SkillsForce.App_Start
//{
//    public class Startup
//    {
//        public string databaseConnectionstring = ConfigurationManager.ConnectionStrings["HangFireConnectionString"].ConnectionString;
//        private IEnumerable<IDisposable> GetHangfireServers()
//        {
//            GlobalConfiguration.Configuration
//                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//                .UseSimpleAssemblyNameTypeSerializer()
//                .UseRecommendedSerializerSettings()
//                .UseSqlServerStorage(databaseConnectionstring)
//                 .UseActivator(new ContainerJobActivator(UnityConfig.Container));

//            yield return new BackgroundJobServer();
//        }

//        public void Configuration(IAppBuilder app)
//        {
//            app.UseHangfireAspNet(GetHangfireServers);
//            app.UseHangfireDashboard();

//            // Let's also create a sample background job
//            BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));

//            // ...other configuration logic
//        }
//    }
//}