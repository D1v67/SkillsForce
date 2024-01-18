using Common.SkillsForce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MVC.SkillsForce.Custom
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }


        public List<RolesEnum> AuthorizedRoles { get; set; }


        public CustomAuthorizationAttribute(params RolesEnum[] roles)
        {
            this.AuthorizedRoles = roles.ToList();

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dfController = filterContext.Controller as Controller;
            if (dfController != null && dfController.Session["CurrentRole"] != null)
            {
                var currentRole = (RolesEnum)Enum.Parse(typeof(RolesEnum), dfController.Session["CurrentRole"].ToString());
                if (!AuthorizedRoles.Contains(currentRole))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "AccessDenied" }));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "AccessDenied" }));
            }

        }
    }
}