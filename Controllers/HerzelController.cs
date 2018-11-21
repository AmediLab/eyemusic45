using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyemusic45.Models.ViewModels;
using System.IO;
using eyeMusic45;

namespace eyemusic45.Controllers
{
    public class HerzelController : HomeController
    {
        //
        // GET: /HerzelAmar/

        AddLessonsModel _theModel;
        
        //enter to input new lesson screen
        public ActionResult input()
        {
            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                if (System.Web.HttpContext.Current.User.Identity.Name != "")
                {
                    session_dead(System.Web.HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    not_session_dead();
                }
            }

            //only addLesson user pass (1234) can add lessons
            //if (_eyeMusicModel.userDAL.rule == "A" || _eyeMusicModel.userDAL.rule == "B")
            {
                refreshmodel();

                _theModel.allDirectory = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");
                System.Web.HttpContext.Current.Session["herzel"] = _theModel;

                return View("Index", _theModel);
            }
            /*else
            {
                ViewBag.len = _eyeMusicModel.len;
                ViewBag.ReturnUrl = "/launch/herzel/input";
                return View("../Home/Login");
            }*/
        }

        /// <summary>
        /// add new lesson (new directory)
        /// </summary>
        /// <param name="DirName">name of directory</param>
        /// <param name="DirHebrew">hebrew translate of directory name</param>
        /// <returns></returns>
        public ActionResult add(string DirName, string DirHebrew)
        {
             _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                if (System.Web.HttpContext.Current.User.Identity.Name != "")
                {
                    session_dead(System.Web.HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    not_session_dead();
                }
            }

            //only addLesson user pass (1234) can add directory

            /*if (_eyeMusicModel.userDAL.rule == "B")
            {*/
                refreshmodel();

                if (DirName != null && DirHebrew != null)
                {                
                    bool ifExesit = false;
                    string[] alldirEng = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");

                    for (int i = 0; i < alldirEng.Length; i++)
                    {
                        if (alldirEng[i] == DirName)
                        {
                            ifExesit = true;
                        }
                    }

                    if (!ifExesit)
                    {
                        System.IO.Directory.CreateDirectory(_theModel.map + "Hrezel\\" + DirName);
                        System.IO.File.Create(_theModel.map + "Hrezel\\" + DirName + "\\ImageName.txt").Close();
                        System.IO.File.AppendAllText(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt", DirName + Environment.NewLine);
                        System.IO.File.AppendAllText(_theModel.map + "\\Texts\\HEB\\HerzelDir.txt", DirHebrew + Environment.NewLine);
                    }

                    _theModel.allDirectory = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");
                    System.Web.HttpContext.Current.Session["herzel"] = _theModel;
                }
                return View("Index", _theModel);
            /*}
            else
            {
                ViewBag.len = _eyeMusicModel.len;
                return View("../Home/Login");
            }*/
        }

        /// <summary>
        /// Enter to specipic directory for add pictures 
        /// </summary>
        /// <param name="DirName">name of directory to enter</param>
        /// <returns></returns>
        public ActionResult enterLesson(string DirName)
        {
            refreshmodel();

            //_theModel.allImages =  Directory.GetFiles(_theModel.map + "Hrezel\\" + DirName);
            _theModel.allImages = Directory.GetFiles(_theModel.map + "Hrezel\\" + DirName, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".png")).ToArray();
            
            if(DirName != null)
                _theModel.DirName = DirName;
            
            System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            
            return View(_theModel);
        }

        //remove directory from directory list
        public ActionResult remove(string DirName)
        {
            
            refreshmodel();

            if (DirName != null)
            {
                int indexToDel = 0;

                if (System.IO.Directory.Exists(_theModel.map + "Hrezel\\" + DirName))
                    System.IO.Directory.Delete(_theModel.map + "Hrezel\\" + DirName, true);

                //Delete the old list of directory and build new list
                string[] alldirEng = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");
                string[] alldirHeb = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\HEB\\HerzelDir.txt");

                for (int i = 0; i < alldirEng.Length; i++)
                {
                    if (alldirEng[i] == DirName)
                    {
                        indexToDel = i;
                        break;
                    }
                }

                //Write new list of directory
                System.IO.File.WriteAllText(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt", "");
                System.IO.File.WriteAllText(_theModel.map + "\\Texts\\HEB\\HerzelDir.txt", "");

                for (int i = 0; i < alldirEng.Length; i++)
                {
                    if (i != indexToDel)
                    {
                        System.IO.File.AppendAllText(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt", alldirEng[i] + Environment.NewLine);
                        System.IO.File.AppendAllText(_theModel.map + "\\Texts\\HEB\\HerzelDir.txt", alldirHeb[i] + Environment.NewLine);
                    }
                }

                _theModel.allDirectory = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");
                System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            }
            return View("Index", _theModel);
        }

        //Upload files with button
        public JsonResult UploadBlined()
        {
            refreshmodel();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                string Hebrowname = Request.Params["Hebrowname"];
                System.IO.File.AppendAllText(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt", Hebrowname + Environment.NewLine);
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);

                    if (fileName.EndsWith(".bmp") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".jpeg"))
                    {
                        _theModel.imgShowPath = "/Hrezel/" + _theModel.DirName + "/" + fileName.Replace("%20", "_");
                        _theModel.picFullPath = _theModel.map + "Hrezel\\" + _theModel.DirName +"\\" + fileName.Replace("%20", "_");
                        file.SaveAs(_theModel.picFullPath); 
                    }
                }
            }
            System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            
            //return Json(_eyeMusicModel.currImagePathupload + "?" + _eyeMusicModel.theUri);
            return Json(_theModel.picFullPath + "?" + _theModel.imgShowPath);

        }

        //Training on the lesson
        public ActionResult learnLesson(string DirName)
        {
            refreshmodel();

            //If select lesson and not refresh
            if (DirName != null)
                _theModel.DirName = DirName;

            _theModel.imageHebrew = System.IO.File.ReadAllLines(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt");
            _theModel.allImages = Directory.GetFiles(_theModel.map + "Hrezel\\" + _theModel.DirName, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".png")).ToArray();

            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                if (System.Web.HttpContext.Current.User.Identity.Name != "")
                {
                    session_dead(System.Web.HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    not_session_dead();
                }
            }

            _theModel.WebPath = new string[_theModel.allImages.Length];
            for (int i = 0; i < _theModel.allImages.Length;i++ )
            {
                _theModel.WebPath[i] = "/Hrezel/" + _theModel.DirName + "/" +_theModel.allImages[i].Split('\\').Last();
            }

            string shortName = _theModel.allImages[0].Replace(".bmp", "").Split('\\').Last();
            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.beep_noise.Replace(".mp3", "") + shortName + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _theModel.currImagePath = "/EM/Images/" + fileNameSeesion;
                
            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _theModel.allImages[0]);

            _theModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            _theModel.ScanSpeed = _eyeMusicModel.ScanSpeed;
            System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            return View(_theModel);
        }

        //select lesson (directory) from list to learn
        public ActionResult selectLesson()
        {
            refreshmodel();

            _theModel.allDirectory = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\ENG\\HerzelDir.txt");
            _theModel.allHebrewDir = System.IO.File.ReadAllLines(_theModel.map + "\\Texts\\HEB\\HerzelDir.txt");
            System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            return View(_theModel);
        }

        //reomve specipic image from directory (by click on it)
        public void remImage(string imagPath)
        {
            refreshmodel();
            int indexDelete = 0;

            for(int i=0;i<_theModel.allImages.Length;i++)
            {
                if (imagPath == _theModel.allImages[i])
                    indexDelete = i;
            }

                System.IO.File.Delete(imagPath);
                _theModel.imageHebrew = System.IO.File.ReadAllLines(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt");
                System.IO.File.WriteAllText(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt", "");

                for (int i = 0; i < _theModel.imageHebrew.Length; i++)
                {
                    if (i != indexDelete)
                    {
                        System.IO.File.AppendAllText(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt", _theModel.imageHebrew[i] + Environment.NewLine);
                    }
                }

                _theModel.allImages = Directory.GetFiles(_theModel.map + "Hrezel\\" + _theModel.DirName, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".png")).ToArray();
                _theModel.imageHebrew = System.IO.File.ReadAllLines(_theModel.map + "Hrezel\\" + _theModel.DirName + "\\ImageName.txt");
                System.Web.HttpContext.Current.Session["herzel"] = _theModel;
        }


        public void refreshmodel()
        {
            _theModel = (AddLessonsModel)System.Web.HttpContext.Current.Session["herzel"];
            if (_theModel == null)
            {
                _theModel = new AddLessonsModel();
                _theModel.map = Server.MapPath("~");
                System.Web.HttpContext.Current.Session["herzel"] = _theModel;
            }
        }

    }
}
