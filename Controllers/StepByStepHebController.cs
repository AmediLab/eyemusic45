using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyeMusic45;
using eyemusic45.Business;
using eyemusic45.DAL;
using System.Threading;



namespace eyemusic45.Controllers
{
    public class StepByStepHebController : HomeController
    {
        StepByStepClass StepFiles;
        MaxUserStepByStep currentStage;
        int THE_MAX = 879;
       
        //
        // GET: /UsersLogin/

        //reutrn on class if the user get <70% in the exam
        public ActionResult returnClass()
        {
            sessionOrNot();

            if (_eyeMusicModel.StepSession == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new StepByStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.StepSession = StepFiles;
            }

            _eyeMusicModel.StepSession.changeLan(_eyeMusicModel.len);

            _eyeMusicModel.StepSession.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.StepSession.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.StepSession.getLessonsInt();
            if (_eyeMusicModel.StepByStepListInts.Length > 0)
                _eyeMusicModel.StepSession.setindex(_eyeMusicModel.StepSession.getLessonsInt()[_eyeMusicModel.StepSession.getLessonsInt().Length - 1]);

            _eyeMusicModel.num_question_step = 1;
            return View("enter", _eyeMusicModel);
        }

        [Authorize]
        //the musnu of step by step
        public ActionResult menuStep()
        {
            sessionOrNot();

            /*if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/StepByStepHeb/menuStep";
                return View("../Home/Login");
            }
            else
            {*/
                return View();
            //}
        }

        [Authorize]
        //return to first lesson of step by step 
        public ActionResult seeFirst()
        {
             sessionOrNot();

             /*if (!_eyeMusicModel.complete_register)
             {
                 ViewBag.len = _eyeMusicModel.len;
                 System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                 ViewBag.ReturnUrl = "/StepByStepHeb/seeFirst";
                 return View("../Home/Login");
             }
             else
             {
             */
                 string map = Server.MapPath("~");
                 StepFiles = new StepByStepClass(map, _eyeMusicModel.len);
                 _eyeMusicModel.StepSession = StepFiles;

                 _eyeMusicModel.StepSession.changeLan(_eyeMusicModel.len);
                 _eyeMusicModel.StepSession.setindex(11);
                 _eyeMusicModel.blind = false;
                 _eyeMusicModel.SoundAfterPicture = false;

                return View("Enter", _eyeMusicModel);
            
             //}
        }

        public ActionResult EndExam()
        {
            sessionOrNot();

            if (_eyeMusicModel.StepSession == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new StepByStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.StepSession = StepFiles;
            }

            _eyeMusicModel.StepSession.changeLan(_eyeMusicModel.len);

            _eyeMusicModel.StepSession.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.StepSession.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.StepSession.getLessonsInt();
            _eyeMusicModel.StepSession.next();
            _eyeMusicModel.num_question_step = 1;
            return View("enter", _eyeMusicModel);
        }

        [Authorize]
        //Without see the image
        //only show description
        public ActionResult blind()
        {
            sessionOrNot();

            goToCurrent();

            /*else
            {
                 _eyeMusicModel.StepSession.changeToLast();
            }*/

            _eyeMusicModel.blind = true;
            _eyeMusicModel.SoundAfterPicture = false;

            if (_eyeMusicModel.StepSession.returnType() == 1)
                return View("Enter", _eyeMusicModel);
            else if (_eyeMusicModel.StepSession.returnType() == 2)
            {
                showImage();
                return View("Learn", _eyeMusicModel);
            }
            else if (_eyeMusicModel.StepSession.returnType() == 3)
                return View("Exam", _eyeMusicModel);
            else
                return View("../Home/firstAny", _eyeMusicModel);
        }

        [Authorize]
        //sound the mp3 file and after of it show the image
        public ActionResult SoundAfterPicture()
        {
            sessionOrNot();

            goToCurrent();

            _eyeMusicModel.SoundAfterPicture = true;
            if (_eyeMusicModel.StepSession.returnType() == 1)
                return View("Enter", _eyeMusicModel);
            else if (_eyeMusicModel.StepSession.returnType() == 2)
            {
                showImage();
                return View("Learn", _eyeMusicModel);
            }
            else if (_eyeMusicModel.StepSession.returnType() == 3)
                return View("Exam", _eyeMusicModel);
            else
                return View("../Home/firstAny", _eyeMusicModel);
        }

        [Authorize]
        //see image and description
        public ActionResult see()
        {
            sessionOrNot();

            /*if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/StepByStepHeb/see";
                return View("../Home/Login");
            }
            else
            {*/
                goToCurrent();
                _eyeMusicModel.blind = false;
                _eyeMusicModel.SoundAfterPicture = false;


                if (_eyeMusicModel.StepSession.returnType() == 1)
                    return View("Enter", _eyeMusicModel);
                else if (_eyeMusicModel.StepSession.returnType() == 2)
                {
                    showImage();
                    return View("Learn", _eyeMusicModel);
                }
                else if (_eyeMusicModel.StepSession.returnType() == 3)
                {
                    string path = Server.MapPath("~");
                    string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.StepSession.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

                    _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
                    _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

                    _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

                    vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.StepSession.imagesCurrent());

                    _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

                    _eyeMusicModel.StepSession.resetNumCorrect();
                    _eyeMusicModel.num_question_step = _eyeMusicModel.StepSession.foundNumber();
                    return View("Exam", _eyeMusicModel);
                }
                else
                    return View("../Home/firstAny", _eyeMusicModel);
            //}
        }

        //found the index of user in last session
        private void goToCurrent()
        {
            if (_eyeMusicModel.StepSession == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new StepByStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.StepSession = StepFiles;
            }

            _eyeMusicModel.StepSession.changeLan(_eyeMusicModel.len);

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    IQueryable<stepByStep_user> sdd = entities.stepByStep_user.Where(usr => usr.user_ID == _eyeMusicModel.userDAL.user_ID);

                    if (sdd.Any())
                    {
                        DateTime? TimeStep = sdd.Max(usr => usr.Time);
                        int? indexStep = sdd.Where(usr => usr.Time == (DateTime)TimeStep).FirstOrDefault().id;

                        if (indexStep != null && indexStep != THE_MAX)
                            _eyeMusicModel.StepSession.setindex((int)indexStep + 1);
                    }
                }
                catch (Exception e)
                {

                }

            }
        }

        //the introdaction of lessons and exams
        public ActionResult Enter(string nextOrPrev)
        {
            sessionOrNot();
            goToCurrent();
            _eyeMusicModel.StepSession.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.StepSession.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.StepSession.getLessonsInt();

            if (nextOrPrev != null)
            {
                if (nextOrPrev == "next")
                {
                }
                else
                    _eyeMusicModel.StepSession.setindex(Int32.Parse(nextOrPrev));
            }


            return View(_eyeMusicModel);
        }

        //check if have user id (maybe only IP)
        private void sessionOrNot()
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
        }

        //start exam
        public ActionResult Exam(string current, string funny, string know)
        {
            if (current == null)
                return RedirectToAction("see");

            sessionOrNot();

            Thread workerThread = new Thread(addtoDBFunny);
            StepByStepHappy addRow = new StepByStepHappy();

            if (funny != null && know != null)
            {
                addRow.happy = Int32.Parse(funny);
                addRow.know = Int32.Parse(know);
            }

            addRow.datetime = DateTime.Now;
            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.StepSession.getindex();

            workerThread.Start(addRow);

            _eyeMusicModel.num_question_step = 9;
            _eyeMusicModel.StepSession.setindex(Int32.Parse(current));
            _eyeMusicModel.StepSession.next();

            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.StepSession.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.StepSession.imagesCurrent());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            _eyeMusicModel.StepSession.resetNumCorrect();

            return View(_eyeMusicModel);
        }

        //start learn
        public ActionResult Learn(string nextOrPrev, string current)
        {
            if (current == null)
                return RedirectToAction("see");

            sessionOrNot();

            _eyeMusicModel.StepSession.setindex(Int32.Parse(current));

            if (nextOrPrev == "next")
                _eyeMusicModel.StepSession.next();
            else
                _eyeMusicModel.StepSession.prev();

            showImage();


            if (_eyeMusicModel.SoundAfterPicture)
            {
                return View("SoundAfterPicture", _eyeMusicModel);
            }
            else
            {
                return View(_eyeMusicModel);
            }
        }

        //prepare the image for show
        //_eyeMusicModel.currImagePath - the location of image
        //_eyeMusicModel.theUri - the loaction of sound 
        private void showImage()
        {
            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.StepSession.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;

            //the location of image
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.StepSession.imagesCurrent());

            //the loaction of sound
            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
        }

        [HttpPost]
        //in the last question only check the correct of answer 
        public ActionResult OnlyCheck(string answer, string iInIndex, string diffrent)
        {
            sessionOrNot();

            _eyeMusicModel.StepSession.setindex(Int32.Parse(iInIndex));

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;
            addRow.right = _eyeMusicModel.StepSession.ifcorrectLast(answer);
            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.StepSession.getindex() + 1;
            
            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";
            

            workerThread.Start(addRow);

            if (_eyeMusicModel.StepSession.ifcorrectLast(answer))
            {
                _eyeMusicModel.StepSession.addCorrect();
            }

            return Json(new
            {
                correct = _eyeMusicModel.StepSession.ifcorrectLast(answer),
                numCorrect = _eyeMusicModel.StepSession.getCorrect()
            });

        }

        [HttpPost]
        //check the correct of answer and move to next question
        public ActionResult Examnext(string answer, string iInIndex, string diffrent)
        {
            sessionOrNot();

            _eyeMusicModel.StepSession.setindex(Int32.Parse(iInIndex));

            _eyeMusicModel.StepSession.next();
            _eyeMusicModel.num_question_step++;

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;
            addRow.right = _eyeMusicModel.StepSession.ifcorrect(answer);
            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.StepSession.getindex();

            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";
           
            workerThread.Start(addRow);

            if (_eyeMusicModel.StepSession.returnType() == 3)
            {
                createimage();

                if (_eyeMusicModel.StepSession.ifcorrect(answer))
                {
                    _eyeMusicModel.StepSession.addCorrect();
                }

                return Json(new
                {
                    voice = _eyeMusicModel.theUri,
                    title = _eyeMusicModel.StepSession.TitlesCurrent(),
                    sendindex = _eyeMusicModel.StepSession.getindex(),
                    pagetype = _eyeMusicModel.StepSession.typesCurrent(),
                    correct = _eyeMusicModel.StepSession.ifcorrect(answer)
                });
            }
            else
            {
                return View("enter", _eyeMusicModel);
            }
        }

        [HttpPost]
        //for reduce time between stimulate load the next image and sound 
        //before the user click on next but
        public ActionResult next(string iInIndex, string nextOrPrev, string diffrent)
        {
            sessionOrNot();

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;
            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.StepSession.getindex();

            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";
            
            workerThread.Start(addRow);

            _eyeMusicModel.StepSession.setindex(Int32.Parse(iInIndex));

            if (nextOrPrev == "next")
                _eyeMusicModel.StepSession.next();
            else
                _eyeMusicModel.StepSession.prev();

            if (_eyeMusicModel.StepSession.typesCurrent() != "1")
                createimage();

            return Json(new
            {
                image = "/EM/Images/" + _eyeMusicModel.bmpName + ".bmp",
                voice = _eyeMusicModel.theUri,
                title = _eyeMusicModel.StepSession.TitlesCurrent(),
                sendindex = _eyeMusicModel.StepSession.getindex(),
                pagetype = _eyeMusicModel.StepSession.typesCurrent()
            });
        }

        //create the image and sound from the stimulate
        private void createimage()
        {
            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.StepSession.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.StepSession.imagesCurrent());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
        }

      
        public void addtoDBFunny(object obj)
        {
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    entities.StepByStepHappies.Add((StepByStepHappy)obj);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        public void addToDBStepByStep(object obj)
        {
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    entities.stepByStep_user.Add((stepByStep_user)obj);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

    }
}
