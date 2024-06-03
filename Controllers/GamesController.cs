using Game_Library_2._0.Data;
using Game_Library_2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game_Library_2._0.Controllers
{
    public class GamesController : Controller
    {
        private GameDAO gameDAO = new GameDAO();

        public ActionResult Index()
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            return View("Index", gameDAO.FetchAll());
        }

        public ActionResult Details(int id)
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            GameModel gameModel = gameDAO.Fetch(id);
            return View("Details", gameModel);
        }

        public ActionResult Create()
        {
            return View("GameForm");
        }

        [HttpPost]
        public ActionResult ProcessCreate(GameModel gameModel)
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            var folderPath = Server.MapPath("~/App_Data/Pictures");
            if (gameModel.Picture != null)
            {
                var path = Path.Combine(folderPath, Path.GetFileName(gameModel.Picture.FileName));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                gameModel.Picture.SaveAs(path);
                gameModel.PicturePath = path;
            }
            else if (gameModel.Id <= 0)
            {
                gameModel.PicturePath = Path.Combine(folderPath, Path.GetFileName("defaultImge.jpg"));
            }
            gameDAO.CreateOrUpdate(gameModel);
            return View("Details", gameModel);
        }

        public ActionResult Edit(int id)
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            GameModel gameModel = gameDAO.Fetch(id);
            return View("GameForm", gameModel);
        }

        public ActionResult Delete(int id)
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            gameDAO.Delete(id);
            return View("Index", gameDAO.FetchAll());
        }

        public ActionResult DisplayImage(string picturePath)
        {
            return File(picturePath, "image/jpg");
        }

        public ActionResult SearchForm()
        {
            return View("SearchForm");
        }

        public ActionResult SearchForName(string searchPhrase)
        {
            gameDAO.LoggedUsername = (string)Session["Username"];
            return View("Index", gameDAO.SearchForName(searchPhrase));
        }

    }
}