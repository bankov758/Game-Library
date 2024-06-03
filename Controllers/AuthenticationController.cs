using Game_Library_2._0.Data;
using Game_Library_2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Game_Library_2._0.Controllers
{
    public class AuthenticationController : Controller
    {

        private UserDAO userDAO = new UserDAO();

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                UserModel userToBeLogged = userDAO.Fetch(user.Username);
                if (userToBeLogged != null && IsValidUser(user, userToBeLogged))
                {
                    Session["Username"] = user.Username;
                    userDAO.LoggedUsername = user.Username;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View("Login", user);

        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = new UserModel();
                userModel.Username = user.Username;
                userModel.Password = user.Password;
                userModel.Email = user.Email;
                if (userDAO.Fetch(user.Username) != null)
                {
                    ModelState.AddModelError("", "That username is already taken.");
                }
                else
                {
                    userDAO.CreateOrUpdate(userModel);
                    Session["Username"] = user.Username;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValidUser(LoginModel userTryingToLogin, UserModel userFromDb)
        {
            return Crypto.VerifyHashedPassword(userFromDb.Password, userTryingToLogin.Password);
        }

    }
}
