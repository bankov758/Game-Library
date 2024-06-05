using Game_Library_2._0.Data;
using Game_Library_2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game_Library_2._0.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Friends()
        {
            UserDAO usersDAO = new UserDAO();
            usersDAO.LoggedUsername = (string)Session["Username"];
            return View("Friends", usersDAO.FetchFriends());
        }

        public ActionResult SearchForm()
        {
            return View("FriendSearchForm");
        }

        public ActionResult SearchUsername(string searchUsername)
        {
            UserDAO usersDAO = new UserDAO();
            usersDAO.LoggedUsername = (string)Session["Username"];
            return View("NotFriends", usersDAO.SearchUsername(searchUsername));
        }

        public ActionResult AddFriend(int id)
        {
            UserDAO usersDAO = new UserDAO();
            usersDAO.LoggedUsername = (string)Session["Username"];
            usersDAO.addFriend(id);
            return View("FriendSearchForm");
        }

        public ActionResult RemoveFriend(int id)
        {
            UserDAO usersDAO = new UserDAO();
            usersDAO.LoggedUsername = (string)Session["Username"];
            usersDAO.RemoveFriend(id);
            return View("Friends", usersDAO.FetchFriends());
        }
    }
}