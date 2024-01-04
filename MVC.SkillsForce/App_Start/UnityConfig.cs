using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.AppLogger;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using System.Web.Mvc;
using Unity;
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

            Container.RegisterType<IUserAuthorizationDAL, UserAuthorizationDAL>();
            Container.RegisterType<IUserAuthorizationService, UserAuthorizationService>();
          

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


            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}