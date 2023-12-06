using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace MVC.SkillsForce
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<Test_IDataAccessLayer, Test_DataAccessLayer>();
            //container.RegisterType<Test_IDataAccessLayer, Test_DataAccessLayer>();

            //container.RegisterType<IDataAccessLayer, DataAccessLayer>();
            container.RegisterType<IDBCommand, DBCommand>();

            container.RegisterType<ITrainingDAL, TrainingDAL>();
            container.RegisterType<ITrainingService, TrainingService>();
            //container.RegisterType<IDataAccessLayer, DataAccessLayer>();
            container.RegisterType<IUserDAL, UserDAL>();
            container.RegisterType<IUserService, UserService>();

            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IAccountDAL, AccountDAL>();

            container.RegisterType<IEnrollmentService, EnrollmentService>();
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>();

            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IDepartmentDAL, DepartmentDAL>();

            container.RegisterType<IPrerequisiteService, PrerequisiteService>();
            container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}