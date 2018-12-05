using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyeMusic45;
using eyemusic45.Business;
using eyemusic45.DAL;
using System.Threading;
using System.Drawing;


namespace eyemusic45.Controllers
{
    public class ExpStepController : HomeController
    {
        ExpStepClass StepFiles;
        int THE_MAX = 879;
        int FIRST_EXAM = 30;
        int FINAL_EXMA = 30;
        int NORMAL_EXAM = 10;
        int TIME_EXAM = 45;

        int FIRST_EXAM_ENTER = 1;
        int FINEL_EXAM_ENTER = 380;
        static int numsaveimage = 0;

        //When the user get grade <70% return on last lesson 
        public ActionResult returnClass()
        {
            sessionOrNot();

            if (_eyeMusicModel.ExpStep == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new ExpStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.ExpStep = StepFiles;
            }

            _eyeMusicModel.ExpStep.changeLan(_eyeMusicModel.len);

            _eyeMusicModel.ExpStep.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.ExpStep.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.ExpStep.getLessonsInt();
            if (_eyeMusicModel.StepByStepListInts.Length > 0)
                _eyeMusicModel.ExpStep.setindex(_eyeMusicModel.ExpStep.getLessonsInt()[_eyeMusicModel.ExpStep.getLessonsInt().Length - 1]);

            _eyeMusicModel.num_question_step = 1;
            return View("enterExp", _eyeMusicModel);
        }

        /*
        public void saveImage(string path,string name)
        {
           numsaveimage++;
           Bitmap dd = new Bitmap(path);
           dd.Save("C:\\Users\\menahemk\\Desktop\\vsnew\\eyemusicto\\eyemusic45\\uploads\\" + numsaveimage.ToString()  + ".bmp");
        }*/


        //the menu for select option for exam
        public ActionResult menustepExp()
        {
            sessionOrNot();

            if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/launch/ExpStep/menustepExp";
                return View("../Home/Login");
            }
            else
            {
                return View(_eyeMusicModel);
            }
        }

        public ActionResult menustepExp(eyemusic45.Models.ViewModels.userPass model)
        {
            sessionOrNot();



            return View(_eyeMusicModel);
            
        }

        //return to first lesson of step by step 
        public ActionResult seeFirst()
        {
            sessionOrNot();

            if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/ExpStep/seeFirst";
                return View("../Home/Login");
            }
            else
            {

                string map = Server.MapPath("~");
                StepFiles = new ExpStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.ExpStep = StepFiles;
                _eyeMusicModel.totalNumQuestion = FIRST_EXAM;
                _eyeMusicModel.ExpStep.changeLan(_eyeMusicModel.len);
                _eyeMusicModel.ExpStep.setindex(0);
                _eyeMusicModel.blind = false;
                _eyeMusicModel.SoundAfterPicture = false;
                _eyeMusicModel.withAnswer = false;
                _eyeMusicModel.TimeToExam = TIME_EXAM;
                
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.UtcNow - baseDate;
                _eyeMusicModel.TimeEnd = diff.TotalMilliseconds + _eyeMusicModel.TimeTotalExp;

                return View("EnterExp", _eyeMusicModel);

            }
        }

        public ActionResult TimeEnded()
        {
            sessionOrNot();

            if (_eyeMusicModel.ExpStep.getindex() < 30)
                _eyeMusicModel.ExpStep.setindex(31);
            else
                _eyeMusicModel.ExpStep.setindex(379);

            return View("EnterExp", _eyeMusicModel);

        }

        public ActionResult EndExam()
        {
            sessionOrNot();

            if (_eyeMusicModel.ExpStep == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new ExpStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.ExpStep = StepFiles;
            }

            _eyeMusicModel.ExpStep.changeLan(_eyeMusicModel.len);
            _eyeMusicModel.withAnswer = true;
            _eyeMusicModel.ExpStep.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.ExpStep.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.ExpStep.getLessonsInt();
            _eyeMusicModel.ExpStep.next();
            _eyeMusicModel.num_question_step = 1;
            return View("enterExp", _eyeMusicModel);
        }

        //Without see the image
        //only show description
        public ActionResult blind(string GoTo)
        {
            sessionOrNot();

            int togo = 0;
            if (Int32.TryParse(GoTo, out togo))
                _eyeMusicModel.ExpStep.setindex(togo);

            //goToCurrent();

            _eyeMusicModel.blind = true;
            _eyeMusicModel.SoundAfterPicture = false;


            if (_eyeMusicModel.TimeEnd == double.MaxValue)
            {
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.UtcNow - baseDate;
                _eyeMusicModel.TimeEnd = diff.TotalMilliseconds + _eyeMusicModel.TimeTotalExp;
            }


            if (_eyeMusicModel.ExpStep.returnType() == 1)
                return View("enterExp", _eyeMusicModel);
            else if (_eyeMusicModel.ExpStep.returnType() == 2)
            {
                showImage();
                _eyeMusicModel.totalNumImageInLesson = _eyeMusicModel.ExpStep.returnNumImage();
                _eyeMusicModel.imagePass = _eyeMusicModel.ExpStep.retPassImg();
                return View("learnExp", _eyeMusicModel);
            }
            else if (_eyeMusicModel.ExpStep.returnType() == 3)
                return View("ExamExp", _eyeMusicModel);
            else if (_eyeMusicModel.ExpStep.returnType() == 4)
            {
                _eyeMusicModel.TimeToExam = TIME_EXAM;
                _eyeMusicModel.withAnswer = false;
                return View("ExamExp", _eyeMusicModel);
            }
            else
                return View("../Home/firstAny", _eyeMusicModel);
        }

        public ActionResult hideNextButton()
        {
            sessionOrNot();

            //_eyeMusicModel.ExpStep.setindex(315);

            if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/ExpStep/see";
                return View("../Home/Login");
            }
            else
            {
                //goToCurrent();
                _eyeMusicModel.blind = false;
                _eyeMusicModel.SoundAfterPicture = false;
                _eyeMusicModel.showNextButton = false;

                if (_eyeMusicModel.TimeEnd == double.MaxValue)
                {
                    DateTime baseDate = new DateTime(1970, 1, 1);
                    TimeSpan diff = DateTime.UtcNow - baseDate;
                    _eyeMusicModel.TimeEnd = diff.TotalMilliseconds + _eyeMusicModel.TimeTotalExp;
                }


                if (_eyeMusicModel.ExpStep.returnType() == 1)
                    return View("enterExp", _eyeMusicModel);
                else if (_eyeMusicModel.ExpStep.returnType() == 2)
                {
                    showImage();
                    _eyeMusicModel.totalNumImageInLesson = _eyeMusicModel.ExpStep.returnNumImage();
                    _eyeMusicModel.imagePass = _eyeMusicModel.ExpStep.retPassImg();
                    return View("learnExp", _eyeMusicModel);
                }
                else //if (_eyeMusicModel.ExpStep.returnType() == 3)
                {
                    string path = Server.MapPath("~");
                    string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

                    _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
                    _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

                    _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

                    vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

                    _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
                    _eyeMusicModel.ExpStep.resetNumCorrect();
                    _eyeMusicModel.num_question_step = _eyeMusicModel.ExpStep.foundNumber();

                    if (_eyeMusicModel.ExpStep.returnType() == 4)
                    {
                        _eyeMusicModel.TimeToExam = TIME_EXAM;
                        _eyeMusicModel.withAnswer = false;
                    }
                    return View("ExamExp", _eyeMusicModel);
                }
                //else
                //    return View("../Home/firstAny", _eyeMusicModel);
            }
        }

        //[Authorize]
        public ActionResult feedbackfinish()
        {
            sessionOrNot();
            eyemusic45.DAL.ShelonFinish sf = new ShelonFinish();
            return View(sf);
        }

        [HttpPost]
        public ActionResult feedbackfinish(ShelonFinish sf,
            string HardLesson,string EasyLesson, string ColorEasy,string ColorHard, string eyes, string musical, string playMusic)
        {
            sessionOrNot();

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {

                ShelonFinish found = entities.ShelonFinishes.Where(usr => usr.user_id == _eyeMusicModel.userDAL.user_ID).SingleOrDefault();

                if (found == null)
                {
                    // User found in the database
                    if (eyes == " " || musical == " " || ColorEasy == " " || ColorHard == " " || sf.faces == null || sf.fun == null || sf.hard == null)
                    {
                        ModelState.AddModelError("", "please fill all ");
                        return View("feedbackfinish");
                    }
                    else
                    {
                        sf.user_id = _eyeMusicModel.userDAL.user_ID;
                        sf.HardColor = Int32.Parse(ColorHard);
                        sf.clearColor = Int32.Parse(ColorEasy);

                        if (eyes == "true")
                            sf.closeEyes = true;
                        else
                            sf.closeEyes = false;

                        if (musical == "true")
                            sf.musical = true;
                        else
                            sf.musical = false;

                        if (playMusic == "true")
                            sf.playMusic = true;
                        else
                            sf.playMusic = false;

                        sf.hard = HardLesson;
                        sf.easy = EasyLesson;
                        entities.ShelonFinishes.Add(sf);
                        entities.SaveChanges();
                    }

                }
            }
            return View("menustepExp",_eyeMusicModel);
        }

        public ActionResult OnlyExam()
        {
            sessionOrNot();
            
            _eyeMusicModel.onlyExam = true;
            string map = Server.MapPath("~");
            StepFiles = new ExpStepClass(map, "h");
            _eyeMusicModel.ExpStep = StepFiles;
            _eyeMusicModel.totalNumQuestion = FIRST_EXAM;
            _eyeMusicModel.ExpStep.changeLan("h");
            _eyeMusicModel.ExpStep.setindex(1);
            _eyeMusicModel.blind = false;
            _eyeMusicModel.SoundAfterPicture = false;
            _eyeMusicModel.withAnswer = true;
            _eyeMusicModel.TimeToExam = TIME_EXAM;

            _eyeMusicModel.totalNumQuestion = FIRST_EXAM;
            _eyeMusicModel.num_question_step = FIRST_EXAM - 1;
            _eyeMusicModel.TimeToExam = TIME_EXAM;
            _eyeMusicModel.withAnswer = true;

            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", "") , _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            _eyeMusicModel.ExpStep.resetNumCorrect();

            return View("ExamExp", _eyeMusicModel);

        }

        /*[Authorize]
        public ActionResult gotoI(string GoToThis)
        {
            sessionOrNot();
            _eyeMusicModel.ExpStep.setindex(Int32.Parse(GoToThis));
            RedirectToAction("see");
        }*/

        //see image and description
        public ActionResult see(string GoTo)
        {
            sessionOrNot();
            int togo = 0;
            if (Int32.TryParse(GoTo,out togo))
                _eyeMusicModel.ExpStep.setindex(togo);

            //_eyeMusicModel.ExpStep.setindex(315);

            if (!_eyeMusicModel.complete_register)
            {
                ViewBag.len = _eyeMusicModel.len;
                System.Web.HttpContext.Current.Session["eyeMusic"] = null;
                ViewBag.ReturnUrl = "/ExpStep/see";
                return View("../Home/Login");
            }
            else
            {
                //goToCurrent();
                _eyeMusicModel.blind = false;
                _eyeMusicModel.SoundAfterPicture = false;


                if (_eyeMusicModel.TimeEnd == double.MaxValue)
                {
                    DateTime baseDate = new DateTime(1970, 1, 1);
                    TimeSpan diff = DateTime.UtcNow - baseDate;
                    _eyeMusicModel.TimeEnd = diff.TotalMilliseconds + _eyeMusicModel.TimeTotalExp;
                }


                if (_eyeMusicModel.ExpStep.returnType() == 1)
                    return View("enterExp", _eyeMusicModel);
                else if (_eyeMusicModel.ExpStep.returnType() == 2)
                {
                    showImage();
                    _eyeMusicModel.totalNumImageInLesson = _eyeMusicModel.ExpStep.returnNumImage();
                    _eyeMusicModel.imagePass = _eyeMusicModel.ExpStep.retPassImg();
                    return View("learnExp", _eyeMusicModel);
                }
                else //if (_eyeMusicModel.ExpStep.returnType() == 3)
                {
                    string path = Server.MapPath("~");
                    string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

                    _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
                    _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

                    _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

                    vh.createMp3(fileNameSeesion.Replace(".bmp", "") , _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

                    _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
                    _eyeMusicModel.ExpStep.resetNumCorrect();
                    _eyeMusicModel.num_question_step = _eyeMusicModel.ExpStep.foundNumber();

                    if (_eyeMusicModel.ExpStep.returnType() == 4)
                    {
                        _eyeMusicModel.TimeToExam = TIME_EXAM;
                        _eyeMusicModel.withAnswer = false;
                    }
                    return View("ExamExp", _eyeMusicModel);
                }
                //else
                //    return View("../Home/firstAny", _eyeMusicModel);
            }
        }

        //found the index of user in last session
        private void goToCurrent()
        {
            if (_eyeMusicModel.ExpStep == null)
            {
                string map = Server.MapPath("~");
                StepFiles = new ExpStepClass(map, _eyeMusicModel.len);
                _eyeMusicModel.ExpStep = StepFiles;
            

            _eyeMusicModel.ExpStep.changeLan(_eyeMusicModel.len);

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
                                _eyeMusicModel.ExpStep.setindex((int)indexStep + 1);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

            }
        }

        //the introdaction of lessons and exams
        public ActionResult enterExp(string nextOrPrev)
        {
            sessionOrNot();

            /*
            if (_eyeMusicModel.Timer != DateTime.MaxValue && _eyeMusicModel.Timer.Subtract(DateTime.Now).TotalMinutes > 70)
            {
                _eyeMusicModel.Timer = DateTime.MaxValue;
                _eyeMusicModel.ExpStep.gotoFinish();
                return View("feedbackfinish", _eyeMusicModel);
            }*/


            //goToCurrent();
            _eyeMusicModel.ExpStep.SetLessons();
            _eyeMusicModel.StepByStepLessons = _eyeMusicModel.ExpStep.getLessonsTitles();
            _eyeMusicModel.StepByStepListInts = _eyeMusicModel.ExpStep.getLessonsInt();

            if (nextOrPrev != null)
            {
                if (nextOrPrev == "next")
                {

                }
                else
                    _eyeMusicModel.ExpStep.setindex(Int32.Parse(nextOrPrev));
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

            goToCurrent();
        }

        //start exam
        public ActionResult ExamExp(string current, string funny, string know)
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
            addRow.id = _eyeMusicModel.ExpStep.getindex();

            workerThread.Start(addRow);

            _eyeMusicModel.ExpStep.setindex(Int32.Parse(current));
            _eyeMusicModel.ExpStep.next();

            //The first exam 20 questions with time limit, four answers
            if (_eyeMusicModel.ExpStep.getindex() == FIRST_EXAM_ENTER)
            {
                _eyeMusicModel.totalNumQuestion = FIRST_EXAM;
                _eyeMusicModel.num_question_step = FIRST_EXAM - 1;
                _eyeMusicModel.TimeToExam = TIME_EXAM;
                _eyeMusicModel.withAnswer = false;
                _eyeMusicModel.finishExam = false;
                _eyeMusicModel.onlyExam = false;
                _eyeMusicModel.answers = _eyeMusicModel.ExpStep.FourAnswers();
            }
            //The final exam same 20 questions with time limit, four answers
            else if (_eyeMusicModel.ExpStep.getindex() == FINEL_EXAM_ENTER)
            {
                _eyeMusicModel.totalNumQuestion = FIRST_EXAM;
                _eyeMusicModel.num_question_step = FINAL_EXMA - 1;
                _eyeMusicModel.TimeToExam = TIME_EXAM;
                _eyeMusicModel.withAnswer = false;
                _eyeMusicModel.finishExam = true;
                _eyeMusicModel.onlyExam = false;
                _eyeMusicModel.answers = _eyeMusicModel.ExpStep.FourAnswers();
            }
            //regular exam without time limit
            else
            {
                _eyeMusicModel.totalNumQuestion = NORMAL_EXAM;
                _eyeMusicModel.num_question_step = NORMAL_EXAM - 1;
                _eyeMusicModel.withAnswer = true;
                _eyeMusicModel.finishExam = false;
                _eyeMusicModel.onlyExam = false;
            }
 

            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", "") , _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            _eyeMusicModel.ExpStep.resetNumCorrect();
            //saveImage(_eyeMusicModel.realpath, _eyeMusicModel.ExpStep.TitlesCurrent());

            return View(_eyeMusicModel);
        }

        //start learn
        public ActionResult learnExp(string nextOrPrev, string current)
        {

            if (current == null)
                return RedirectToAction("see");

            sessionOrNot();

            if (_eyeMusicModel.TimeEnd == double.MaxValue)
            {
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.UtcNow - baseDate;
                _eyeMusicModel.TimeEnd = diff.TotalMilliseconds + _eyeMusicModel.TimeTotalExp;
            }

            _eyeMusicModel.ExpStep.setindex(Int32.Parse(current));
            _eyeMusicModel.totalNumImageInLesson = _eyeMusicModel.ExpStep.returnNumImage();
            _eyeMusicModel.imagePass = _eyeMusicModel.ExpStep.retPassImg();
            if (nextOrPrev == "next")
                _eyeMusicModel.ExpStep.next();
            else
                _eyeMusicModel.ExpStep.prev();

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
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;

            //the location of image
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", "") , _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

            //the loaction of sound
            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
            //saveImage(_eyeMusicModel.realpath,_eyeMusicModel.ExpStep.TitlesCurrent());
        }

        [HttpPost]
        //in the last question only check the correct of answer 
        public ActionResult OnlyCheck(string answer, string iInIndex, string diffrent)
        {
            sessionOrNot();

            _eyeMusicModel.ExpStep.setindex(Int32.Parse(iInIndex));

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;
            
            if (answer != "k")
                addRow.right = _eyeMusicModel.ExpStep.ifcorrectLast(answer);

            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.ExpStep.getindex() + 1;

            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";

            addRow.TheAnswer = answer;

            workerThread.Start(addRow);

            if (_eyeMusicModel.ExpStep.ifcorrectLast(answer))
            {
                _eyeMusicModel.ExpStep.addCorrect();
            }

            _eyeMusicModel.previos_path = _eyeMusicModel.currImagePath;

            return Json(new
            {
                correct = _eyeMusicModel.ExpStep.ifcorrectLast(answer),
                numCorrect = _eyeMusicModel.ExpStep.getCorrect(),
                prev = _eyeMusicModel.previos_path
            });

        }

        [HttpPost]
        //check the correct of answer and move to next question
        public ActionResult Examnext(string answer, string iInIndex, string diffrent)
        {
            sessionOrNot();

            _eyeMusicModel.ExpStep.setindex(Int32.Parse(iInIndex));

            _eyeMusicModel.ExpStep.next();
            _eyeMusicModel.num_question_step++;

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;

            if (answer != "k")
                addRow.right = _eyeMusicModel.ExpStep.ifcorrect(answer);

            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.ExpStep.getindex();
            addRow.TheAnswer = answer;

            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";

            _eyeMusicModel.previos_path = _eyeMusicModel.currImagePath;

            workerThread.Start(addRow);

            //return four answers 
            if (_eyeMusicModel.ExpStep.returnType() == 4)
            {
                _eyeMusicModel.answers = _eyeMusicModel.ExpStep.FourAnswers();
            }
            else
                _eyeMusicModel.answers = new string[]{"","","",""};

            if (_eyeMusicModel.ExpStep.returnType() == 3 || _eyeMusicModel.ExpStep.returnType() == 4)
            {
                createimage();

                if (_eyeMusicModel.ExpStep.ifcorrect(answer))
                {
                    _eyeMusicModel.ExpStep.addCorrect();
                }

                return Json(new
                {
                    voice = _eyeMusicModel.theUri,
                    title = _eyeMusicModel.ExpStep.TitlesCurrent(),
                    sendindex = _eyeMusicModel.ExpStep.getindex(),
                    pagetype = _eyeMusicModel.ExpStep.typesCurrent(),
                    correct = _eyeMusicModel.ExpStep.ifcorrect(answer),
                    prev = _eyeMusicModel.previos_path,
                    finish = false,
                    A = _eyeMusicModel.answers[0],
                    B = _eyeMusicModel.answers[1],
                    C = _eyeMusicModel.answers[2],
                    D = _eyeMusicModel.answers[3]
                });
            }
            else
            {
                return Json(new
                {
                    voice = _eyeMusicModel.theUri,
                    title = _eyeMusicModel.ExpStep.TitlesCurrent(),
                    sendindex = _eyeMusicModel.ExpStep.getindex(),
                    pagetype = _eyeMusicModel.ExpStep.typesCurrent(),
                    correct = _eyeMusicModel.ExpStep.ifcorrect(answer),
                    prev = _eyeMusicModel.previos_path,
                    finish = true,
                    A = _eyeMusicModel.answers[0],
                    B = _eyeMusicModel.answers[1],
                    C = _eyeMusicModel.answers[2],
                    D = _eyeMusicModel.answers[3]
                });
            }
            /*else
            {
                return View("enterExp", _eyeMusicModel);
            }*/
        }
        
        [HttpPost]
        //for reduce time between stimulate load the next image and sound 
        //before the user click on next but
        public ActionResult Preparenext(string iInIndex, string nextOrPrev)
        {
            sessionOrNot();
            _eyeMusicModel.ExpStep.setindex(Int32.Parse(iInIndex));

            if (_eyeMusicModel.ExpStep.typesNext() != "1")
                createimageNext();

            return Json(new
            {
                image = "/EM/Images/" + _eyeMusicModel.bmpName + ".bmp",
                voice = _eyeMusicModel.theUri,
            });
        }


        [HttpPost]
        //get the next stimulate in learning
        public ActionResult next(string iInIndex, string nextOrPrev, string diffrent)
        {
            sessionOrNot();

            Thread workerThread = new Thread(addToDBStepByStep);
            stepByStep_user addRow = new stepByStep_user();
            addRow.duration = Int32.Parse(diffrent);
            addRow.Time = DateTime.Now;
            addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
            addRow.id = _eyeMusicModel.ExpStep.getindex();

            if (_eyeMusicModel.blind)
                addRow.blindOrSeeOrafter = "b";
            else if (_eyeMusicModel.SoundAfterPicture)
                addRow.blindOrSeeOrafter = "a";
            else
                addRow.blindOrSeeOrafter = "s";

            workerThread.Start(addRow);

            _eyeMusicModel.ExpStep.setindex(Int32.Parse(iInIndex));

            if (nextOrPrev == "next")
                _eyeMusicModel.ExpStep.next();
            else
                _eyeMusicModel.ExpStep.prev();

            if (_eyeMusicModel.ExpStep.typesCurrent() != "1")
                createimage();

            return Json(new
            {
                image = "/EM/Images/" + _eyeMusicModel.bmpName + ".bmp",
                voice = _eyeMusicModel.theUri,
                title = _eyeMusicModel.ExpStep.TitlesCurrent(),
                sendindex = _eyeMusicModel.ExpStep.getindex(),
                pagetype = _eyeMusicModel.ExpStep.typesCurrent()
            });
        }

        //create the image and sound from the stimulate
        private void createimage()
        {
            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesCurrent().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", "") , _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesCurrent());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            //saveImage(_eyeMusicModel.realpath, _eyeMusicModel.ExpStep.TitlesCurrent());
        }


        //create the next image and sound (for Preparenext)
        private void createimageNext()
        {
            string path = Server.MapPath("~");
            string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + _eyeMusicModel.ExpStep.imagesNext().Replace("\\", "").Replace(".bmp", "") + ".bmp";

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;

            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path + "\\" + _eyeMusicModel.ExpStep.imagesNext());

            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";
        }

        //add how complex and know for this lesson
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

        //add general statistics on this user
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
