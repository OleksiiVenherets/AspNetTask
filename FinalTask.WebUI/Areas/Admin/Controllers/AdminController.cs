using System;
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
    /// Handles all requests relating toadministrating user
    /// </summary>
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private readonly Loger _loger = new Loger(AppDomain.CurrentDomain.BaseDirectory, LogLevel.All, LogFormat.Xml);
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        /// <summary>
        /// Disolays view to create user
        /// </summary>
        /// <returns></returns>
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model">User to create</param>
        /// <returns></returns>
        // POST: Admin/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result =
                    await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                    _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">User Id to delete</param>
        /// <returns></returns>
        //POST: Admin/Delete
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
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
                return View("Error", new[] { "Користувача не знайдено" });
            }
        }

        /// <summary>
        /// Display user data  to edit
        /// </summary>
        /// <param name="id">User Id to edit</param>
        /// <returns></returns>
        //GET: Admin/Edit
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="id">User ID to edit</param>
        /// <param name="email">user login</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        // POST: Admin/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                var validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        _loger.Log(validPass.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                        AddErrorsFromResult(validPass);
                    }
                }

                if (validPass != null && ((!validEmail.Succeeded || password == string.Empty || !validPass.Succeeded)))
                    return View(user);
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Користувача не знйдено");
            }
            return View(user);
        }
        
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
    }
}