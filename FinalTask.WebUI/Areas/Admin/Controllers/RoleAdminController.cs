using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinalTask.Domain.Entities;
using FinalTask.Domain.Infrastructure;
using FinalTask.WebUI.Areas.Admin.Models;
using Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FinalTask.WebUI.Areas.Admin.Controllers
{
    /// <summary>
    /// Handles all requests relating to userroles
    /// </summary>
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : Controller
    {
        private AppUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        private AppRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();

        private readonly Loger _loger = new Loger(AppDomain.CurrentDomain.BaseDirectory, LogLevel.All, LogFormat.Xml);

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        /// <summary>
        /// Displays view to create new role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="name">Role name</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (!ModelState.IsValid)
                return View(name);
            var result
                = await RoleManager.CreateAsync(new AppRole(name));

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AddErrorsFromResult(result);
                _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
            }
            return View(name);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">Role ID to delete</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new[] { "Роль не знайдена" });
            }
        }

        /// <summary>
        /// Edit Role
        /// </summary>
        /// <param name="id">Role Id to edit</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            var memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<AppUser> members
                = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        /// <summary>
        /// Edit role
        /// </summary>
        /// <param name="model">Edited role data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            if (!ModelState.IsValid)
                return View("Error", new[] {"Роль не знайдена"});
            IdentityResult result;
            foreach (var userId in model.IdsToAdd ?? new string[] { })
            {
                result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                if (!result.Succeeded)
                {
                    _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("Error", result.Errors);
                }
            }
            foreach (var userId in model.IdsToDelete ?? new string[] { })
            {
                result = await UserManager.RemoveFromRoleAsync(userId,
                    model.RoleName);

                if (!result.Succeeded)
                {
                    _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("Error", result.Errors);
                }
            }
            return RedirectToAction("Index");
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}