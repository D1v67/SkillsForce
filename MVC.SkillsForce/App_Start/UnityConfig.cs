using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.AppLogger;
using Common.SkillsForce.BackgoundJobLogger;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using System.ComponentModel;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace MVC.SkillsForce
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; internal set; }
        public static void RegisterComponents()
        {
		    Container = new UnityContainer();

            Container.RegisterType<ILogger, Logger>();

            Container.RegisterType<IDBCommand, DBCommand>();

            Container.RegisterType<ISessionService, SessionService>();

            Container.RegisterType<IFileExtensionValidation, FileExtensionValidation>();


            Container.RegisterType<IJobLogger, JobLogger>();

            Container.RegisterType<IExportSelectedEmployeeService, ExportSelectedEmployeeService>();
            Container.RegisterType<IExportSelectedEmployeeDAL, ExportSelectedEmployeeDAL>();

            Container.RegisterType<IUserAuthorizationDAL, UserAuthorizationDAL>();
            Container.RegisterType<IUserAuthorizationService, UserAuthorizationService>();

            Container.RegisterType<IUserActivityService, UserActivityService>();
            Container.RegisterType<IUserActivityDAL, UserActivityDAL>();


            Container.RegisterType<ITrainingDAL, TrainingDAL>();
            Container.RegisterType<ITrainingService, TrainingService>();
            //container.RegisterType<IDataAccessLayer, DataAccessLayer>();
            Container.RegisterType<IUserDAL, UserDAL>();
            Container.RegisterType<IUserService, UserService>();

            Container.RegisterType<IAccountService, AccountService>();
            Container.RegisterType<IAccountDAL, AccountDAL>();

            Container.RegisterType<IEnrollmentService, EnrollmentService>();
            Container.RegisterType<IEnrollmentDAL, EnrollmentDAL>();

            Container.RegisterType<IDepartmentService, DepartmentService>();
            Container.RegisterType<IDepartmentDAL, DepartmentDAL>();

            Container.RegisterType<IPrerequisiteService, PrerequisiteService>();
            Container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>();

            Container.RegisterType<INotificationService, NotificationService>();

            Container.RegisterType<IAttachmentService, AttachmentService>();
            Container.RegisterType<IAttachmentDAL, AttachmentDAL>();

            Container.RegisterType<IAppNotificationService, AppNotificationService>();
            Container.RegisterType<IAppNotificationDAL, AppNotificationDAL>();

            Container.RegisterInstance<INotificationService>( "Email Notification", new NotificationService());
            Container.RegisterInstance<INotificationService>("In App Notification", new AppNotificationService(Container.Resolve<IAppNotificationDAL>()));
            //Container.RegisterType(typeof(INotificationHandler), typeof(NotificationHandler),
            //    new InjectionConstructor(new ResolveArray)

            //    );

            Container.RegisterType(typeof(INotificationHandler), typeof(NotificationHandler),
            new InjectionConstructor(new ResolvedArrayParameter<INotificationService>(
                new ResolvedParameter<INotificationService>("Email Notification"),
                new ResolvedParameter<INotificationService>("In App Notification")
            )));

            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}