using eyemusic45.Business;
using eyemusic45.DAL;
using eyeMusic45;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eyemusic45.Controllers
{
    public class ExamController : HomeController
    {
        /// <summary>
        ///  Add new exam on specipic level and stage
        /// </summary>
        /// <param name="obj"></param>
        private void addExamToDBUser(object obj)
        {
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    exam addThis = obj as exam;

                    //found level by level_ID
                    addThis.level1 = (from levels in entities.levels
                                     where levels.level1 == _eyeMusicModel.select_level
                                     select levels).SingleOrDefault();

                    //found stage by stage_ID
                    addThis.stage1 = (from stages in entities.stages
                                     where stages.stage1 == _eyeMusicModel.select_stage
                                     select stages).SingleOrDefault();

                    entities.exams.Add((DAL.exam)obj);
                    entities.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }



        /// <summary>
        /// add specipic question of question
        /// </summary>
        /// <param name="obj"></param>
        private void AddToDBExamQ(object obj)
        {

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    pic_paths the_path;
                    level lvl;
                    stage stg;
                    desacriptaion des;
                    desacriptaion des1;
                    desacriptaion des2;
                    desacriptaion des3;

                    int? maxlvl;
                    int? maxstg;
                    int? maxdes;
                    int? maxpath;

                    //check if this image already in DB
                    var picPathVar = entities.pic_paths.SingleOrDefault(usr => usr.pic_path == ((exam_q_levels)obj).pic_paths.pic_path);

                    if (picPathVar != null)
                    {
                        the_path = picPathVar;
                    }
                    else
                    {
                        //create row with image path
                        maxpath = entities.pic_paths.Max(usr => (int?)usr.id);

                        if (maxpath == null)
                            maxpath = 0;

                        the_path = new pic_paths();
                        the_path.id = (int)++maxpath;
                        the_path.pic_path = ((exam_q_levels)obj).pic_paths.pic_path;
                        //entities.levels.Add(lvl);
                        //entities.SaveChanges();
                    }

                    ((exam_q_levels)obj).pic_paths = the_path;

                    //check if this level already in DB
                    var levelsVar = entities.levels.SingleOrDefault(usr => usr.level1 == ((exam_q_levels)obj).level.level1);

                    if (levelsVar != null)
                    {
                        lvl = levelsVar;
                    }
                    else
                    {
                        //create row for level
                        maxlvl = entities.levels.Max(usr => (int?)usr.id);

                        if (maxlvl == null)
                            maxlvl = 0;

                        lvl = new level();
                        lvl.id = (int)++maxlvl;
                        lvl.level1 = ((exam_q_levels)obj).level.level1;
                        //entities.levels.Add(lvl);
                        //entities.SaveChanges();
                    }

                    ((exam_q_levels)obj).level = lvl;

                    //check if this stage already in DB
                    var stageVar = entities.stages.SingleOrDefault(usr => usr.stage1 == ((exam_q_levels)obj).stage.stage1);
                    if (stageVar != null)
                    {
                        stg = stageVar;
                    }
                    else
                    {
                        //add new row with stage
                        maxstg = entities.stages.Max(usr => (int?)usr.id);

                        if (maxstg == null)
                            maxstg = 0;

                        stg = new stage();
                        stg.id = (int)++maxstg;
                        stg.stage1 = ((exam_q_levels)obj).stage.stage1;
                        //entities.stages.Add(stg);
                        //entities.SaveChanges();
                    }
                    ((exam_q_levels)obj).stage = stg;

                    //check if this four descriptions already in DB
                    var descVar = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == ((exam_q_levels)obj).desacriptaion.desacriptaion1);
                    if (descVar != null)
                    {
                        des = descVar;
                        maxdes = entities.desacriptaions.Max(usr => (int?)usr.id);
                    }
                    else
                    {
                        maxdes = entities.desacriptaions.Max(usr => (int?)usr.id);

                        if (maxdes == null)
                            maxdes = 0;

                        des = new desacriptaion();
                        des.id = (int)++maxdes;
                        des.desacriptaion1 = ((exam_q_levels)obj).desacriptaion.desacriptaion1;
                    }

                    ((exam_q_levels)obj).desacriptaion = des;

                    //1
                    var descVar1 = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == ((exam_q_levels)obj).desacriptaion2.desacriptaion1);
                    if (descVar1 != null)
                    {
                        des1 = descVar1;
                    }
                    else
                    {
                        des1 = new desacriptaion();
                        des1.id = (int)++maxdes;
                        des1.desacriptaion1 = ((exam_q_levels)obj).desacriptaion1.desacriptaion1;
                    }

                    ((exam_q_levels)obj).desacriptaion1 = des1;

                    //2
                    var descVar2 = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == ((exam_q_levels)obj).desacriptaion2.desacriptaion1);
                    if (descVar2 != null)
                    {
                        des2 = descVar2;
                    }
                    else
                    {
                        des2 = new desacriptaion();
                        des2.id = (int)++maxdes;
                        des2.desacriptaion1 = ((exam_q_levels)obj).desacriptaion2.desacriptaion1;
                    }

                    ((exam_q_levels)obj).desacriptaion2 = des2;

                    //3
                    var descVar3 = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == ((exam_q_levels)obj).desacriptaion3.desacriptaion1);
                    if (descVar3 != null)
                    {
                        des3 = descVar3;
                    }
                    else
                    {
                        des3 = new desacriptaion();
                        des3.id = (int)++maxdes;
                        des3.desacriptaion1 = ((exam_q_levels)obj).desacriptaion3.desacriptaion1;
                    }

                    ((exam_q_levels)obj).desacriptaion3 = des3;

                    entities.exam_q_levels.Add((exam_q_levels)obj);
                    entities.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }


        //menu of exam
        public ActionResult introExam()
        {
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

            if (_eyeMusicModel.complete_register)
                return View(_eyeMusicModel);
            else
                return View("complateDetails");
        }

        public ActionResult exam_specific_level()
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

            return View(_eyeMusicModel);

        }


        public ActionResult exam_specific_class()
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

            return View(_eyeMusicModel);
        }

        /// <summary>
        /// after the user answer, check correct and move to next question
        /// </summary>
        /// <param name="answer">the answer of the user</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Examnext(string answer)
        {
            int theanswer = int.Parse(answer);
            int if_currect = 0;

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            string prev_des = _eyeMusicModel.answers[_eyeMusicModel.theTrue];

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

            _eyeMusicModel.previos_path = _eyeMusicModel.currImagePath;


            if (answer != null)
            {
                if (!checkeifHaveQuestion())
                {
                    askQuestion();
                }

                Thread addExamToDB = new Thread(addExamToDBUser);
                exam curEx = new DAL.exam();

                curEx.datetime = DateTime.Now;
                curEx.answer = theanswer;
                curEx.num_exam = _eyeMusicModel.exam_number;
                curEx.num_q = _eyeMusicModel.num_question - 1;
                curEx.user_ID = _eyeMusicModel.userDAL.user_ID;

                if (curEx.answer == _eyeMusicModel.thePrevTrue)
                {
                    if_currect = 1;
                    curEx.correct = true;
                }
                else
                {
                    if_currect = 0;
                    curEx.correct = false;
                }

                curEx.session = 1;
                addExamToDB.Start(curEx);
            }

            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;

            return Json(new
            {
                image = _eyeMusicModel.previos_path,
                voice = _eyeMusicModel.theUri,
                correct = if_currect,
                description = prev_des,
                next_des = _eyeMusicModel.answers
            });
        }

        /// <summary>
        /// finish current exam and 
        /// showing resualt for all exams of this user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult examResualt(string id)
        {
            int theanswer = int.Parse(id);
            int if_currect = 0;

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            string prev_des = _eyeMusicModel.answers[_eyeMusicModel.theTrue];

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

            _eyeMusicModel.previos_path = _eyeMusicModel.currImagePath;

            Thread addExamToDB = new Thread(addExamToDBUser);
            exam curEx = new DAL.exam();

            curEx.datetime = DateTime.Now;
            curEx.answer = theanswer;
            curEx.num_exam = _eyeMusicModel.exam_number;
            curEx.num_q = _eyeMusicModel.num_question;
            curEx.user_ID = _eyeMusicModel.userDAL.user_ID;

            if (curEx.answer == _eyeMusicModel.theTrue)
            {
                if_currect = 1;
                curEx.correct = true;
            }
            else
            {
                if_currect = 0;
                curEx.correct = false;
            }

            curEx.session = 1;
            addExamToDB.Start(curEx);



            _eyeMusicModel.exam_number++;
            _eyeMusicModel.num_question = 0;

            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;


            //Get the result for all exmas
            List<resualt_exam> allRes = new List<resualt_exam>();
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    var all = (from thisexam in entities.resualt_exam
                               where thisexam.user_ID == _eyeMusicModel.userDAL.user_ID
                               && thisexam.level == _eyeMusicModel.select_level
                               && thisexam.stage == _eyeMusicModel.select_stage
                               && thisexam.num_exam == _eyeMusicModel.exam_number - 1
                               select thisexam);

                    foreach (resualt_exam rs in all)
                    {
                        allRes.Add(rs);
                    }
                }
                catch (Exception e)
                {

                }
            }

            return Json(new { the_stage = allRes[0].stage, the_level = allRes[0].level, num_correct = allRes[0].num_correct, num_exam = allRes[0].num_exam, ifCorrect = if_currect });
        }

        //show only grades of all exmas
        public ActionResult grades()
        {
            
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

            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;

            List<resualt_exam_row> allRes = new List<resualt_exam_row>();
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                //get object models from context
                ObjectContext objContext = ((IObjectContextAdapter)entities).ObjectContext;
                //get all full names types from object model
                var fullNameTypes = objContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.OSpace);
                ///////////////////
                var conStr = objContext.Connection.ConnectionString;
                var connection = new EntityConnection(conStr);
                var workspace = connection.GetMetadataWorkspace();

                var entitySets = workspace.GetItems<EntityContainer>(DataSpace.SSpace).First().BaseEntitySets;
                string schema = entitySets[0].Schema;


                try
                {

                var all = entities.Database.SqlQuery<resualt_exam_row>(
                "Select * from (select ROW_NUMBER () OVER (ORDER BY [user_ID]) as row_num, " +
		        "*  " +
		        "FROM ( " +
        "SELECT exam.[user_ID], " +
	          "exam.[num_exam], " +
	          "stg.stage, " +
	          "lvl.[level], " +
	          "count(*) as num_correct " +
        "FROM  [" + schema + "].[exam] exam, " +
              "[" + schema + "].[stages] stg, " +
              "[" + schema + "].[levels] lvl " +
        "WHERE exam.[correct] = 1 " +
	          "AND exam.[level] = lvl.id " +
	          "AND exam.[stage] = stg.id " +
	        "GROUP BY exam.[user_ID], " +
			         "exam.[num_exam], " +
			         "stg.stage, " +
			         "lvl.[level] " +
        "union select exam.[user_ID], " +
			         "exam.[num_exam], " +
			         "stg.stage, " +
			         "lvl.[level], " +
			         "0 as num_correct " +
        "FROM  [" + schema + "].[exam] exam, " +
              "[" + schema + "].[stages] stg, " +
              "[" + schema + "].[levels] lvl " +
        "WHERE exam.[correct] = 0 " +
	          "AND exam.[level] = lvl.id " +
	          "AND exam.[stage] = stg.id " +
	        "GROUP BY exam.[user_ID], " +
			         "exam.[num_exam], " +
			         "stg.stage, " +
			         "lvl.[level] " +
                     "having count(*) = 10 ) as [rows_number]) as dd where user_ID = @p0 " +
                     "order by [level], [stage]", _eyeMusicModel.userDAL.user_ID);


                    /*var all = (from thisexam in entities.resualt_exam_row
                               where thisexam.user_ID == _eyeMusicModel.userDAL.user_ID
                               select thisexam).OrderBy(usr => usr.level).ThenBy(usr2 => usr2.stage);
                    */
                    foreach (resualt_exam_row rs in all)
                    {
                        resualt_exam_row rsnew = new resualt_exam_row();
                        rsnew.num_correct = rs.num_correct;
                        rsnew.num_exam = rs.num_exam;
                        rsnew.user_ID = rs.user_ID;

                        rsnew.level = vh.heb.returnStr(rs.level.Substring(2).Replace("_", " ").Trim());
                        rsnew.stage = vh.heb.returnStr(rs.stage.Substring(2).Replace("_", " ").Trim());
                        allRes.Add(rsnew);
                    }
                }
                catch (Exception e)
                {
                    resualt_exam_row rsnew = new resualt_exam_row();
                        rsnew.num_correct = 0;
                        rsnew.num_exam = 1;
                        rsnew.user_ID = 122;
                        rsnew.level = e.Message;
                        rsnew.stage = "";
                        allRes.Add(rsnew);
                }
            }

            ViewBag.len = _eyeMusicModel.len;
            ViewBag.stage = _eyeMusicModel.select_stage;
            ViewBag.level = _eyeMusicModel.select_level;
            return View(allRes);
        }


        public ActionResult examStage(string selected_level)
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


            _eyeMusicModel.select_level = selected_level;

            return View(_eyeMusicModel);
        }


        /// <summary>
        /// the regular exam on specipic level stage
        /// </summary>
        /// <param name="selected_level"></param>
        /// <param name="selected_stage"></param>
        /// <returns></returns>
        public ActionResult exam(string selected_level, string selected_stage)
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

            vh.startExam(selected_level, selected_stage);
            _eyeMusicModel.image_names = vh._myManager.image_names;
            _eyeMusicModel.path_for_image = vh._myManager.path_for_image;
            _eyeMusicModel.select_level = selected_level;
            _eyeMusicModel.select_stage = selected_stage;

            _eyeMusicModel.num_question = 0;
            _eyeMusicModel.exam_number = 1;


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    examWithLevel foundmax = (from thisexam in entities.examWithLevels
                                              where thisexam.user_ID == _eyeMusicModel.userDAL.user_ID
                                              && thisexam.level == selected_level
                                              && thisexam.stage == selected_stage
                                              select thisexam).OrderByDescending(p => p.num_exam).First();

                    _eyeMusicModel.exam_number = (int)foundmax.num_exam;
                    _eyeMusicModel.num_question = (int)foundmax.max_num_question;

                    if (_eyeMusicModel.num_question > 9)
                    {
                        _eyeMusicModel.num_question = 0;
                        _eyeMusicModel.exam_number++;
                    }



                }
                catch (Exception e)
                {

                }

            }


            if (!checkeifHaveQuestion())
            {
                askQuestion();
            }

            _eyeMusicModel.previos_path = "";

            return View("exam", _eyeMusicModel);
        }

        //check if aleardy have question for this exam 
        //also create a new question
        private bool checkeifHaveQuestion()
        {
          
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    _eyeMusicModel.num_question++;// = (int)maxQuestion + 1;

                    var exams_res = (from examq in entities.exam_QUESTION
                                     where examq.level == _eyeMusicModel.select_level
                                     && examq.stage == _eyeMusicModel.select_stage
                                     && examq.num_exam == _eyeMusicModel.exam_number
                                     && examq.num_question == _eyeMusicModel.num_question
                                     select new full_question
                                     {
                                         answer_A = examq.A,
                                         answer_B = examq.B,
                                         answer_C = examq.C,
                                         answer_D = examq.D,
                                         realpath = examq.pic_path,
                                         the_true = examq.the_true
                                     }).SingleOrDefault();
                    
                    if (exams_res == null)
                    {
                        return false;
                    }
                    else
                    {
                        _eyeMusicModel.answers[0] = vh.heb.returnStr(exams_res.answer_A);
                        _eyeMusicModel.answers[1] = vh.heb.returnStr(exams_res.answer_B);
                        _eyeMusicModel.answers[2] = vh.heb.returnStr(exams_res.answer_C);
                        _eyeMusicModel.answers[3] = vh.heb.returnStr(exams_res.answer_D);
                        _eyeMusicModel.thePrevTrue = _eyeMusicModel.theTrue;
                        _eyeMusicModel.theTrue = exams_res.the_true;
                        _eyeMusicModel.realpath = exams_res.realpath;

                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        //found random question 
        //add to db 
        public void askQuestion()
        {
            string path = Server.MapPath("~");

            _eyeMusicModel.thisQuestions = found_new_questions();
            _eyeMusicModel.answers = new string[4];

            for (int i = 0; i < 4; i++)
            {
                _eyeMusicModel.answers[i] = vh.heb.returnStr(vh._myManager.image_names[_eyeMusicModel.thisQuestions[i]]);
            }

            Thread workerThread = new Thread(AddToDBExamQ);
            exam_q_levels toAdd = new exam_q_levels();
            toAdd.desacriptaion = new desacriptaion();
            toAdd.desacriptaion1 = new desacriptaion();
            toAdd.desacriptaion2 = new desacriptaion();
            toAdd.desacriptaion3 = new desacriptaion();
            toAdd.stage = new stage();
            toAdd.pic_paths = new pic_paths();
            toAdd.level = new level();

            toAdd.desacriptaion.desacriptaion1 = vh._myManager.image_names[_eyeMusicModel.thisQuestions[0]];
            toAdd.desacriptaion1.desacriptaion1 = vh._myManager.image_names[_eyeMusicModel.thisQuestions[1]];
            toAdd.desacriptaion2.desacriptaion1 = vh._myManager.image_names[_eyeMusicModel.thisQuestions[2]];
            toAdd.desacriptaion3.desacriptaion1 = vh._myManager.image_names[_eyeMusicModel.thisQuestions[3]];
            toAdd.level.level1 = _eyeMusicModel.select_level;
            toAdd.stage.stage1 = _eyeMusicModel.select_stage;

            Random ee = new Random();
            _eyeMusicModel.thePrevTrue = _eyeMusicModel.theTrue;
            _eyeMusicModel.theTrue = ee.Next(0, 4);

            toAdd.the_true = _eyeMusicModel.theTrue;
            toAdd.pic_paths.pic_path = _eyeMusicModel.path_for_image[_eyeMusicModel.thisQuestions[_eyeMusicModel.theTrue]];
            string fileNameSeesion = toAdd.pic_paths.pic_path.Split('/').Last();

            _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;

            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;
            _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;
            _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

            vh.createMp3(fileNameSeesion.Replace(".bmp", ""), path + toAdd.pic_paths.pic_path.Replace("/", "\\"));
            toAdd.num_exam = _eyeMusicModel.exam_number;
            toAdd.num_question = _eyeMusicModel.num_question;
            workerThread.Start(toAdd);

        }

        //found new three random worng answers for this image
        public int[] found_new_questions()
        {
            Random rr = new Random();
            int[] randNext = new int[4];

            randNext[0] = rr.Next(0, vh._myManager.path_for_image.Count());

            randNext[1] = rr.Next(0, vh._myManager.path_for_image.Count());

            while (randNext[0] == randNext[1])
            {
                randNext[1] = rr.Next(0, vh._myManager.path_for_image.Count());
            }

            randNext[2] = rr.Next(0, vh._myManager.path_for_image.Count());

            while (randNext[0] == randNext[2] || randNext[1] == randNext[2])
            {
                randNext[2] = rr.Next(0, vh._myManager.path_for_image.Count());
            }

            randNext[3] = rr.Next(0, vh._myManager.path_for_image.Count());

            while (randNext[0] == randNext[3] || randNext[1] == randNext[3] || randNext[2] == randNext[3])
            {
                randNext[3] = rr.Next(0, vh._myManager.path_for_image.Count());
            }

            return (randNext);
        }

    }
}
