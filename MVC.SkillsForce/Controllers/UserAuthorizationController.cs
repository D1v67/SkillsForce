using BusinessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class UserAuthorizationController : Controller
    {
        private readonly IUserAuthorizationService _userAuthorizationService;
        public UserAuthorizationController(IUserAuthorizationService userAuthorizationService) 
        { 
            _userAuthorizationService = userAuthorizationService;
        }

        [HttpPost]
        public async Task<JsonResult> CheckPermissions(int userId, List<string> permissions)
        {
            try
            {
                // Call your DAL method to check permissions
                Dictionary<string, bool> permissionsResult = new Dictionary<string, bool>();

                foreach (var permission in permissions)
                {
                    bool hasPermission = await _userAuthorizationService.IsUserHavePermissionAsync(userId, permission);
                    permissionsResult.Add(permission, hasPermission);
                }

                return Json(permissionsResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return Json(new { error = ex.Message });
            }
        }
    }
}