using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;
using FinalTask.Domain.Entities;
using FinalTask.Domain.Infrastructure;
using FinalTask.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Logger;

namespace FinalTask.WebUI.Controllers
{
    /// <summary>
    /// Handles all requests relating to user
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {

        private ITaskRepository _repository;
        private readonly Loger _loger = new Loger(AppDomain.CurrentDomain.BaseDirectory, LogLevel.All, LogFormat.Xml);

        public AccountController(ITaskRepository repo)
        {
            _repository = repo;
        }

        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

        private AppUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        /// <summary>
        /// Diplay view  to login
        /// </summary>
        /// <param name="returnUrl">Url name to return</param>
        /// <returns></returns>
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// LOgin specific user
        /// </summary>
        /// <param name="model">User to login</param>
        /// <param name="returnUrl">Url name to return</param>
        /// <returns></returns>
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await UserManager.FindAsync(model.Name, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Некоректний логін чи пароль");
            }
            else
            {
                try
                {
                    var ident = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);

                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, ident);
                }
                catch(Exception ex)
                {
                    _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());

                }

                _loger.Log("User " + model.Name + " is loginning", LogLevel.Info, DateTime.Now, GetType().ToString());
                if (user.UserName == "Admin")
                    return RedirectToAction("Index", "Admin", "Admin");
                return RedirectToAction("Index", "Visit");
            }

            return View(model);
        }

        /// <summary>
        /// Diplays view to register
        /// </summary>
        /// <returns></returns>
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model">User data to register</param>
        /// <returns></returns>
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return View(model);
            var user = new AppUser { Name = model.Name, UserName = model.Email, Surname = model.Surname, Email = model.Email, Telephone = model.Telephone };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                _loger.Log("User " + model.Email + " is loginning", LogLevel.Info, DateTime.Now, GetType().ToString());
                return RedirectToAction("Index", "Visit");
            }
            AddErrorsFromResult(result);
            _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
            return View(model);
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Diplay user account details
        /// </summary>
        /// <returns></returns>
        // GET: /Account/SeeDetails
        [Authorize]
        public ActionResult SeeDetails()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        /// <summary>
        /// Diplay yser account details to edit
        /// </summary>
        /// <returns></returns>
        // GET: /Account/SeeDetails
        [Authorize]
        public ActionResult Edit()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                var edituser = new EditViewModel
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Telephone = user.Telephone
                };
                return View(edituser);
            }
            else
            {
                return RedirectToAction("SeeDetails");
            }
        }

        /// <summary>
        /// Edit cpecific account data
        /// </summary>
        /// <param name="model">Data to edit</param>
        /// <returns></returns>
        // POST: /Account/Edit
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View(model);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Telephone = model.Telephone;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("SeeDetails");
            }
            _loger.Log(result.Errors.ToString(), LogLevel.Error, DateTime.Now, GetType().ToString());
            AddErrorsFromResult(result);
            return View(model);
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