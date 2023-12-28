using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Unity;

namespace MVC.SkillsForce.App_Start
{
    public class ContainerJobActivator : JobActivator
    {
        private IUnityContainer _container;

        public ContainerJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }
}