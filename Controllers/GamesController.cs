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

        public ActionResult Index()
        {
            GameDAO gameDAO = new GameDAO();
            return View("Index", gameDAO.FetchAll());
        }

        public ActionResult Details(int id)
        {
            GameDAO gameDAO = new GameDAO();
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
            if (gameModel.Picture != null)
            {
                var folderPath = Server.MapPath("~/App_Data/Pictures");
                var path = Path.Combine(folderPath, Path.GetFileName(gameModel.Picture.FileName));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                gameModel.Picture.SaveAs(path);
                gameModel.PicturePath = path;
            }

            GameDAO gameDAO = new GameDAO();
            gameDAO.CreateOrUpdate(gameModel);
            return View("Details", gameModel);
        }

        public ActionResult Edit(int id)
        {
            GameDAO gameDAO = new GameDAO();
            GameModel gameModel = gameDAO.Fetch(id);
            return View("GameForm", gameModel);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            GameDAO gameDAO = new GameDAO();
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
            GameDAO gameDAO = new GameDAO();
            return View("Index", gameDAO.SearchForName(searchPhrase));
        }

    }
}