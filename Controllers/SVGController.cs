using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyeMusic45;
using eyemusic45.DAL;
using System.Threading;
using eyemusic45.Business;
using System.Drawing;
using System.IO;
using Ionic.Zip;


namespace eyemusic45.Controllers
{
    public class SVGController : HomeController
    {

        int lessonnumber = 0; 

        //get the canvas in ajax
        //proccess the image and create sound
        public ActionResult canvasUpload(string imageData)
        {
            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }


            byte[] sendEM = Convert.FromBase64String(imageData);

            int next = Id_number_drag++;
            _eyeMusicModel.realpath = _eyeMusicModel.path + "\\EM\\Images\\" + _eyeMusicModel.SvgGlobalName + next + ".bmp";
            vh.createMp3Save(_eyeMusicModel.SvgGlobalName + next, new Bitmap(Image.FromStream(new MemoryStream(sendEM), true, true)), true);
            _eyeMusicModel.currImagePath = "/EM/Images/" + _eyeMusicModel.SvgGlobalName + next + ".bmp";
            return Json(new { voice = _eyeMusicModel.theUri, image = _eyeMusicModel.currImagePath });
        
        }


        public ActionResult canvasUploadLesson(string imageData)
        {
            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }


            byte[] sendEM = Convert.FromBase64String(imageData);

            //int next = Id_number_drag++;
            _eyeMusicModel.num_question++;

            _eyeMusicModel.realpath = _eyeMusicModel.path + "\\EM\\Images\\" + _eyeMusicModel.SvgGlobalName + _eyeMusicModel.num_question + ".bmp";
 
            if (!System.IO.File.Exists(_eyeMusicModel.realpath))
            {
                vh.createMp3Save(_eyeMusicModel.SvgGlobalName + _eyeMusicModel.num_question, new Bitmap(Image.FromStream(new MemoryStream(sendEM), true, true)), true);
            }

            _eyeMusicModel.currImagePath = "/EM/Images/" + _eyeMusicModel.SvgGlobalName + _eyeMusicModel.num_question + ".bmp";
            return Json(new { voice = _eyeMusicModel.theUri, image = _eyeMusicModel.currImagePath });

        }


        [HttpPost]
        //ajax
        //click on image on SVG view 
        // create sound and image 
        public ActionResult ajaxregSVG(string fileName)
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

            string fileNameshort = "";
            if (fileName != null)
            {

                string[] alldb = fileName.Split('/');

                string DBnumber = alldb[alldb.Length - 1];
                string DBstage = alldb[alldb.Length - 2];
                string DBclass = alldb[alldb.Length - 3];

                fileNameshort =  _eyeMusicModel.filter + DBnumber.Replace(".bmp", "");

                string fileNameWithSeesion = fileName;


                string path = Server.MapPath("~");
                _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameshort + ".bmp";
                _eyeMusicModel.theUri = "/EM/Out/" + fileNameshort + ".mp3";

                _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameWithSeesion;

                vh.createMp3(fileNameshort, path + fileName.Replace("/", "\\"));

            }

            return Json(new { image = "/EM/Images/" + _eyeMusicModel.bmpName + ".bmp", voice = _eyeMusicModel.theUri, voiceSvg = "uploads/" + fileNameshort + ".wav" });
        }

        public ActionResult svgcreator()
        {
            Session.Add("username", "Simon");
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
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

            Thread workerThread = new Thread(deleteZip);
            _eyeMusicModel.dragid = id_fordrag_before++;

            workerThread.Start(Session.SessionID);

            return View(_eyeMusicModel);
        }

        public ActionResult svgd()
        {
            Session.Add("username", "Simon");
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
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

            Thread workerThread = new Thread(deleteZip);
            workerThread.Start(Session.SessionID);

            _eyeMusicModel.dragid = id_fordrag_before++;

            return View(_eyeMusicModel);
        }

        public ActionResult svg()
        {
            Session.Add("username", "Simon");
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
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

            Thread workerThread = new Thread(deleteZip);
            workerThread.Start(Session.SessionID);

            _eyeMusicModel.dragid = id_fordrag_before++;

            return View(_eyeMusicModel);
        }

        [HttpPost]
        //ajax
        //save the file that uplad in drag and drop in svg view
        public ActionResult saveSvgfile(IEnumerable<HttpPostedFileBase> files)
        {
            //creates session object by restoring current session parameters
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

            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the fielname
                        var fileName = Path.GetFileName(file.FileName);

                        if (fileName.EndsWith(".svg"))
                        {
                            _eyeMusicModel.file = file;
                        }
                    }
                }
            }

            return Json(new { });
        }

        [HttpPost]
        //ajax
        //save the file that uplad in drag and drop in dragdrop view
        public ActionResult saveBmpfile(IEnumerable<HttpPostedFileBase> files)
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

            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the fielname
                        var fileName = Path.GetFileName(file.FileName);

                        if (fileName.EndsWith(".bmp") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".jpeg"))
                        {
                            _eyeMusicModel.file = file;
                        }
                    }
                }
            }

            return Json(new { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed">the scan speed of EyeMusic in mulisecound per colmun </param>
        /// <param name="Time">The approximate time for sound the speech</param>
        /// <param name="len">the language of speech</param>
        /// <param name="couple">Arbitrary coupling for change the synchronize for speech with eyemusic</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFilesSVG(string speed, string Time, string len,string couple)
        {
            //Formatting all parameters
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


            _eyeMusicModel.ScanSpeed = Int32.Parse(speed);
            _eyeMusicModel.SvgNumbers = "";
            _eyeMusicModel.currImagePathupload = "";
            _eyeMusicModel.theUri = "";

            string path = Server.MapPath("~");

            var fileName = Path.GetFileName(_eyeMusicModel.file.FileName);
            //the name of the file 
            _eyeMusicModel.SvgGlobalName = Session.SessionID + _eyeMusicModel.dragid + Id_number_drag++ + fileName.Replace("%20", "_").Replace(".svg", "");
            // where to save the file
            _eyeMusicModel.realpath = path + "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".svg";
            _eyeMusicModel.svgHtmlPath = "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".wav";
            _eyeMusicModel.NumberPath = path + _eyeMusicModel.svgHtmlPath;
            //save the file in the real path
            _eyeMusicModel.file.SaveAs(_eyeMusicModel.realpath);
           

           
            if (Time != null && len != null)
            {
                //calls the class speech with all parameteres and returns the text and its location in the file for mespeak to read later
                speech addNumber = new speech(_eyeMusicModel.ScanSpeed, _eyeMusicModel.NumberPath, Int32.Parse(Time),Int32.Parse(couple), len);
                _eyeMusicModel.SvgNumbers = addNumber.add(_eyeMusicModel.file);
            }

            _eyeMusicModel.file.InputStream.Position = 0;
            //read the tr - the svg file that was saved in the server
            TextReader tr = new StreamReader(_eyeMusicModel.file.InputStream);

            string allSvgText = tr.ReadToEnd();

            _eyeMusicModel.currImagePathupload = "/uploads/" + _eyeMusicModel.SvgGlobalName + ".svg";
            //Return json with "numbers" - which have the text and all the speech parameters, and "text" - which has the real text of the svg file
            return Json(new { image = _eyeMusicModel.currImagePathupload, voice = _eyeMusicModel.svgHtmlPath, numbers = _eyeMusicModel.SvgNumbers, text = allSvgText});
        }

        public ActionResult playNext(string speed, string Time, string len, string couple, string nameLesson)
        {
            //Formatting all parameters
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

            _eyeMusicModel.ScanSpeed = Int32.Parse(speed);
            _eyeMusicModel.SvgNumbers = "";
            _eyeMusicModel.currImagePathupload = "";
            _eyeMusicModel.theUri = "";

            string path = Server.MapPath("~");

            //var fileName = Path.GetFileName(_eyeMusicModel.file.FileName);
            //the name of the file 
            //_eyeMusicModel.SvgGlobalName = Session.SessionID + _eyeMusicModel.dragid + Id_number_drag++ + fileName.Replace("%20", "_").Replace(".svg", "");
            // where to save the file
            _eyeMusicModel.num_question =0;
            _eyeMusicModel.SvgGlobalName = "LESSONSVG" + nameLesson;
            _eyeMusicModel.realpath = path + "\\SvgLesson\\" + nameLesson + ".svg";
            _eyeMusicModel.svgHtmlPath = "\\SvgLesson\\" + nameLesson + ".wav";
            _eyeMusicModel.NumberPath = path + _eyeMusicModel.svgHtmlPath;
            //save the file in the real path

            string[] allLines = System.IO.File.ReadAllLines(path + "\\SvgLesson\\lessonText.txt");

            if (Time != null && len != null)
            {
                //calls the class speech with all parameteres and returns the text and its location in the file for mespeak to read later
                speech addNumber = new speech(_eyeMusicModel.ScanSpeed, _eyeMusicModel.NumberPath, Int32.Parse(Time), Int32.Parse(couple), len);
                _eyeMusicModel.SvgNumbers = addNumber.addFileName(_eyeMusicModel.realpath);
            }

            //_eyeMusicModel.file.InputStream.Position = 0;
            //read the tr - the svg file that was saved in the server
            TextReader tr = new StreamReader(_eyeMusicModel.realpath);

            string allSvgText = tr.ReadToEnd();

            _eyeMusicModel.currImagePathupload = "/uploads/" + _eyeMusicModel.SvgGlobalName + ".svg";
            //Return json with "numbers" - which have the text and all the speech parameters, and "text" - which has the real text of the svg file
            return Json(new { image = _eyeMusicModel.currImagePathupload, voice = _eyeMusicModel.svgHtmlPath, numbers = _eyeMusicModel.SvgNumbers, text = allSvgText, svgLesson = allLines[int.Parse(nameLesson) - 1]});
        }



        public ActionResult dragdrop()
        {
            Session.Add("username", "Simon");
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
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

            Thread workerThread = new Thread(deleteZip);
            workerThread.Start(Session.SessionID + _eyeMusicModel.dragid.ToString());

            _eyeMusicModel.dragid = id_fordrag_before++;

            return View(_eyeMusicModel);
        }

        //after exit delte not used file for save space
        public void deleteZip(object dragid)
        {
            try
            {

                string path = Server.MapPath("~");

                String[] filenames = System.IO.Directory.GetFiles(path + "\\EM\\Out");

                // This is just a sample, provided to illustrate the DotNetZip interface.  
                // This logic does not recurse through sub-directories.
                // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                // which operates recursively. 
                foreach (String filename in filenames)
                {
                    if (filename.Contains((string)dragid))
                    {
                        System.IO.File.Delete(filename);
                    }

                }

                filenames = System.IO.Directory.GetFiles(path + "\\uploads");

                // This is just a sample, provided to illustrate the DotNetZip interface.  
                // This logic does not recurse through sub-directories.
                // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                // which operates recursively. 
                foreach (String filename in filenames)
                {
                    if (filename.Contains((string)dragid))
                    {
                        System.IO.File.Delete(filename);
                    }

                }

            }

            catch (Exception e)
            {

            }
        }

        //zip file create
        [HttpPost]
        public ActionResult forzipfile()
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

            string zipfile = "";

            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    // note: this does not recurse directories! 
                    string path = Server.MapPath("~");
                    String[] filenames = System.IO.Directory.GetFiles(path + "\\EM\\Out");

                    // This is just a sample, provided to illustrate the DotNetZip interface.  
                    // This logic does not recurse through sub-directories.
                    // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                    // which operates recursively. 
                    foreach (String filename in filenames)
                    {
                        if (filename.Contains(Session.SessionID + _eyeMusicModel.dragid))
                        {
                            zip.AddFile(filename).FileName = filename.Replace(Session.SessionID, "").Replace(".bmp", "").Replace(path + "\\EM\\Out", "mp3File");
                            zip.AddFile(filename.Replace("\\EM\\Out", "\\uploadReal").Replace(".mp3", "")).FileName = filename.Replace(Session.SessionID, "").Replace(".mp3", "").Replace(path + "\\EM\\Out", "images_after");
                        }
                    }


                    zipfile = path + "\\uploads\\" + Session.SessionID + _eyeMusicModel.dragid + ".zip";
                    zip.Save(zipfile);
                }
            }
            catch (System.Exception ex1)
            {
                System.Console.Error.WriteLine("exception: " + ex1);
            }

            return Json(new { image = "uploads/" + Session.SessionID + _eyeMusicModel.dragid + ".zip" });
        }

        //in drag drop proccess the image and return image and sound
        [HttpPost]
        public ActionResult UploadFiles()
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


            // extract only the fielname
            var fileName = Path.GetFileName(_eyeMusicModel.file.FileName);

            if (fileName.EndsWith(".bmp") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".jpeg"))
            {
                string path = Server.MapPath("~");

                string fileNameSeesion = Session.SessionID + _eyeMusicModel.dragid + fileName.Replace(".bmp", "").Replace(".jpg", "").Replace(".png", "").Replace(".jpeg", "").Replace("%20", "_") + Id_number_drag++ + ".bmp";
                _eyeMusicModel.realpath = path + "\\uploads\\" + fileNameSeesion;

                _eyeMusicModel.currImagePathupload = "/uploads/" + fileNameSeesion;

                vh.createMp3Save(fileNameSeesion, new Bitmap(Image.FromStream(_eyeMusicModel.file.InputStream, true, true)), true);
            }


            return Json(new { image = _eyeMusicModel.currImagePathupload, voice = _eyeMusicModel.theUri });
        }

        //Upload file with a button (in dragdrop view)
        public JsonResult UploadBlined()
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


            for (int i = 0; i < Request.Files.Count; i++)
            {
                _eyeMusicModel.currImagePathupload = "";
                _eyeMusicModel.theUri = "";

                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);

                    if (fileName.EndsWith(".bmp") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".jpeg"))
                    {
                        string path = Server.MapPath("~");

                        string fileNameSeesion = Session.SessionID + _eyeMusicModel.dragid + fileName.Replace(".bmp", "").Replace(".jpg", "").Replace(".png", "").Replace(".jpeg", "").Replace("%20", "_") + Id_number_drag++ + ".bmp";
                        _eyeMusicModel.realpath = path + "\\uploads\\" + fileNameSeesion;

                        _eyeMusicModel.currImagePathupload = "/uploads/" + fileNameSeesion;

                        vh.createMp3Save(fileNameSeesion, new Bitmap(Image.FromStream(file.InputStream, true, true)), true);
                    }
                }
            }
            return Json(_eyeMusicModel.currImagePathupload + "?" + _eyeMusicModel.theUri);
        }

        /// <summary>
        /// For create svg from urmola 
        /// create the speech and init file names
        /// </summary>
        /// <param name="svg">the svg text</param>
        /// <returns></returns>
        public JsonResult speechFromSvgString(string svg, string speed, string Time, string len, string couple)
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


            _eyeMusicModel.ScanSpeed = Int32.Parse(speed);
            _eyeMusicModel.SvgNumbers = "";
            _eyeMusicModel.currImagePathupload = "";
            _eyeMusicModel.theUri = "";

            string path = Server.MapPath("~");

            _eyeMusicModel.SvgGlobalName = Session.SessionID + _eyeMusicModel.dragid + Id_number_drag++;
            _eyeMusicModel.realpath = path + "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".svg";
            _eyeMusicModel.svgHtmlPath = "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".wav";
            _eyeMusicModel.NumberPath = path + _eyeMusicModel.svgHtmlPath;

            if (Time != null && len != null)
            {
                //found the text for mespeak (at javascript)
                speech addNumber = new speech(_eyeMusicModel.ScanSpeed, _eyeMusicModel.NumberPath, Int32.Parse(Time), Int32.Parse(couple), len);
                _eyeMusicModel.SvgNumbers = addNumber.add(svg);
            }

            _eyeMusicModel.currImagePathupload = "/uploads/" + _eyeMusicModel.SvgGlobalName + ".svg";

            return Json(new {voice = _eyeMusicModel.svgHtmlPath, numbers = _eyeMusicModel.SvgNumbers, text = svg });        
        }

        //Upload file with a button (in svg view)
        public JsonResult UploadBlinedSVG()
        {
            string allSvgText = "";

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


            for (int i = 0; i < Request.Files.Count; i++)
            {
                _eyeMusicModel.currImagePathupload = "";
                _eyeMusicModel.theUri = "";

                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);

                    if (fileName.EndsWith(".svg"))
                    {
                        string path = Server.MapPath("~");

                        _eyeMusicModel.SvgGlobalName = Session.SessionID + _eyeMusicModel.dragid + Id_number_drag++ + fileName.Replace("%20", "_").Replace(".svg", "");
                        _eyeMusicModel.realpath = path + "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".svg";
                        _eyeMusicModel.svgHtmlPath = "\\uploads\\" + _eyeMusicModel.SvgGlobalName + ".wav";
                        _eyeMusicModel.NumberPath = path + _eyeMusicModel.svgHtmlPath;

                        file.SaveAs(_eyeMusicModel.realpath);

                        speech addNumber = new speech(_eyeMusicModel.ScanSpeed, _eyeMusicModel.NumberPath, 350,30, "e");
                        _eyeMusicModel.SvgNumbers = addNumber.add(file);

                        _eyeMusicModel.file.InputStream.Position = 0;
                        TextReader tr = new StreamReader(_eyeMusicModel.file.InputStream);

                        allSvgText = tr.ReadToEnd();

                        _eyeMusicModel.currImagePathupload = "/uploads/" + _eyeMusicModel.SvgGlobalName + ".svg";
                    }
                }
            }

            return Json(_eyeMusicModel.currImagePathupload + "?" + allSvgText + "?" + _eyeMusicModel.SvgNumbers);
        }

    }
}
