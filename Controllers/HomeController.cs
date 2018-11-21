using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyeMusic45;
using System.IO;
using System.Drawing;
using NAudio.Lame;
using System.Threading;
using eyemusic45.Models;
using System.Web.Security;
using eyemusic45.DAL;
using Ionic.Zip;
using System.Data.Entity.Validation;
using System.Diagnostics;
using eyemusic45.Business;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using eyemusic45.Models.ViewModels;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.EntityClient;

namespace eyemusic45.Controllers
{

    public class HomeController : Controller
    {
        public eyeMusic2 vh;
        public eyemusic45.Models.ViewModels.eyeMusicModel _eyeMusicModel;
        public static int Id_number = 0;
        public static int Id_number_drag = 0;
        public static int id_fordrag_before = 0;
        public static int maxUser = 0;
        public RespTrainShort ToAddToDB;
        public userID newUser;
        public level_1 addRow;
        public static DateTime baseDate = new DateTime(1970, 1, 1);
        public static string binPath = "";
        
        //
        // GET: /Home/
        //private const string TempPath = @"C:\Temp";

        public ActionResult OVAL()
        {
            return View();
        }

        public ActionResult create_user_short()
        {
            return View();
        }
        /*
        public ActionResult sessionDetails()
        {

            _eyeMusicModel =
              (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //string connectionString = "data source=198.38.83.33;initial catalog=amedilab_data;user id=amedilab_forcsharp;password=12345678";
            string connectionString = "Data Source=ekeksql00.ekmd.huji.uni;Initial Catalog=amira_virtual_exp;uid=amira_virtual_exp;User ID=amira_virtual_exp_admin;Password=+3Am^vxp67*";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {

                }

                //string selectCommandText = "SELECT * FROM amedilab_data.amedilab_menahem.level_1";
                string selectCommandText = "SELECT * FROM sessionDetails";

                using (SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection))
                {
                    using (DataTable table = new DataTable("level_1"))
                    {
                        adapter.Fill(table);
                        StringBuilder commaDelimitedText = new StringBuilder();
                        //commaDelimitedText.AppendLine("col1,col2,col3"); // optional if you want column names in first row
                        foreach (DataRow row in table.Rows)
                        {
                            string value = string.Format("{0},{1},{2},{3}", row[0], row[1], row[2], row[3]); // how you format is up to you (spaces, tabs, delimiter, etc)
                            commaDelimitedText.AppendLine(value);
                        }

                        System.IO.File.WriteAllText(Server.MapPath("~") + "\\EM\\Images\\steps.txt", commaDelimitedText.ToString());
                    }
                }
            }

            return View("firstAny", _eyeMusicModel);
        }

        public ActionResult save()
        {

            _eyeMusicModel =
              (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //string connectionString = "data source=198.38.83.33;initial catalog=amedilab_data;user id=amedilab_forcsharp;password=12345678";
            string connectionString = "Data Source=ekeksql00.ekmd.huji.uni;Initial Catalog=amira_virtual_exp;uid=amira_virtual_exp;User ID=amira_virtual_exp_admin;Password=+3Am^vxp67*";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    
                }

                //string selectCommandText = "SELECT * FROM amedilab_data.amedilab_menahem.level_1";
                string selectCommandText = "SELECT * FROM steps";
                    
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection))
                {
                    using (DataTable table = new DataTable("level_1"))
                    {
                        adapter.Fill(table);
                        StringBuilder commaDelimitedText = new StringBuilder();
                        //commaDelimitedText.AppendLine("col1,col2,col3"); // optional if you want column names in first row
                        foreach (DataRow row in table.Rows)
                        {
                            string value = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}", row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16]); // how you format is up to you (spaces, tabs, delimiter, etc)
                            commaDelimitedText.AppendLine(value);
                        }

                        System.IO.File.WriteAllText(Server.MapPath("~") + "\\EM\\Images\\steps.txt", commaDelimitedText.ToString());
                    }
                }
            }

            return View("firstAny",_eyeMusicModel);
        }
        */
        public ActionResult menustepEnter(eyemusic45.Models.ViewModels.userPass model, string gander)
        {
            if (ModelState.IsValid)
            {
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    string username = model.name;
                    string password = model.password;
                    string password2 = model.password2;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    userID userValid = entities.userIDs.Where(user => user.name == username).SingleOrDefault();

                    // User found in the database
                    if (userValid != null)
                    {
                        ModelState.AddModelError("", "השם הזה כבר קיים אנא בחר שם אחר");
                        return View("create_user_short");
                    }
                    if (username ==  null || username == "")
                    {
                        ModelState.AddModelError("", "הכנס שם משתמש");
                        return View("create_user_short");
                    }
                    if (username.Length > 30)
                    {
                        ModelState.AddModelError("", "שם המשתמש יכול להיות מקסימום 30 תווים");
                        return View("create_user_short");
                    }
                    else if (model.birth_year < 1901 || model.birth_year > DateTime.Now.Year)
                    {
                        ModelState.AddModelError("", "שנת לידה לא חוקית");
                        return View("create_user_short");
                    }
                    else
                    {

                        if (maxUser++ == 0)
                        {
                            try
                            {
                                maxUser = entities.userIDs.Max(user2 => user2.user_ID) + 1;
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        newUser = new userID();
                        newUser.name = username;
                        newUser.password =  "";
                        newUser.user_ID = maxUser;

                        newUser.birth_year = model.birth_year;
                        newUser.age_of_blind = model.age_of_blind;
                        newUser.gander = gander;
                        newUser.place = "Pro Isreal";
                        newUser.precent_blind = 0;
                        newUser.previos_expirments_num = model.previos_expirments_more;
                        newUser.yourself = "y";
                        newUser.level_blind = model.level_blind;
                        //newUser.len = "H";
                        //newUser.rule = "C";
                        String mapFile = Server.MapPath("~");
                        _eyeMusicModel = new Models.ViewModels.eyeMusicModel(mapFile);
                        _eyeMusicModel.userid = username;

                        vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

                        vh = new eyeMusic2(mapFile, _eyeMusicModel);


                        _eyeMusicModel.userDAL = newUser;

                       newUser.fill_somting = true;
                       _eyeMusicModel.complete_register = true;
                       

                        System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
                        System.Web.HttpContext.Current.Session["eyeMusic"] = vh;

                        try
                        {
                            entities.userIDs.Add(newUser);
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

                        return View("../ExpStep/menuStepExp", _eyeMusicModel);
                    }

                }
            }

            ViewBag.len = _eyeMusicModel.len;

            return View("menuStepExp", "ExpStep", _eyeMusicModel);
        }
        


        [Authorize]
        public void UserNewRule(string user, string NewRule)
        {
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            int theUserId = Int32.Parse(user);
            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //if (_eyeMusicModel.userDAL.rule == "A")
            {
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    userID toChange = entities.userIDs.Where(usr => usr.user_ID == theUserId).SingleOrDefault();
                    //toChange.rule = NewRule;
                    entities.userIDs.Attach(toChange);
                    var entry = entities.Entry(toChange);
                    //toChange.rule = NewRule;

                    entities.Entry(toChange).State = EntityState.Modified;
                    //entry.Property(e => e.rule).IsModified = true;
                    //toChange.rule = NewRule;


                    entities.SaveChanges();

                }
            }

        }

        public ActionResult UserFoundRule(string user)
        {
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];
            userID toChange = null;

            int theUserId = Int32.Parse(user);
            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //if (_eyeMusicModel.userDAL.rule == "A")
            //{
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    toChange = entities.userIDs.Where(usr => usr.user_ID == theUserId).SingleOrDefault();
                }
            //}

            //return Json(new { rule = toChange.rule});
                return Json(new { rule = "A" });
        }

        [Authorize]
        public ActionResult ChangeRule()
        {
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            
            //if (_eyeMusicModel.userDAL.rule == "A")
            {
                List<userList> for_sent = new List<userList>();

                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    try
                    {
                        /*
                        var allUsers = entities.userIDs.Where(user => user.name.Split('.').Count() != 4);
                        */
                        var allUsers = (from p in entities.userIDs
                                        where (!p.name.Contains("."))
                                        orderby p.name ascending
                                        select p).ToList();


                        foreach (userID forAdd in allUsers)
                        {
                            for_sent.Add(new userList(forAdd.user_ID, forAdd.name));
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }

                return View(for_sent);
            }
            //else
            //{
            //    ViewBag.ReturnUrl = "/launch/Home/ChangeRule";
            //    return View("Login");
            //}
}

        public ActionResult Index()
        {
            not_session_dead();
            return View("firstAny", _eyeMusicModel);
        }

        [Authorize]
        //The sceret enter to Lab memeber only
        public ActionResult inner()
        {
            ViewBag.anonymous = false;
            addPathForMp3();

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //if (_eyeMusicModel.userDAL.rule == "A" || _eyeMusicModel.userDAL.rule == "B")
            //{
                return View("first", _eyeMusicModel);
            //}
            //else
            //{ 
            //    return View("firstAny", _eyeMusicModel);
            //}

            
        }
                
        public ActionResult create_user()
        {
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            eyemusic45.Models.ViewModels.userPass usrpass = new Models.ViewModels.userPass();
            return View(usrpass);
        }

        [HttpPost]
        //if the user create but not all the details filled
        public ActionResult ComplateUser(eyemusic45.Models.ViewModels.userPass model,string returnUrl, string gander, string blindYesNo, string expirment, string yourself)
        {
                    if(model.birth_year < 0 || model.birth_year > DateTime.Now.Year) 
                    {
                        ModelState.AddModelError("", "The year of birth not legal.");
                        return View("create_user");
                    }
                    else
                    {
                        using (amedilab_dataEntities entities = new amedilab_dataEntities())
                        {
                            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
                            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

                            newUser = entities.userIDs.Attach(_eyeMusicModel.userDAL);
                            newUser.birth_year = model.birth_year;
                            newUser.age_of_blind = model.age_of_blind;
                            newUser.gander = gander;
                            newUser.place = model.place;
                            newUser.precent_blind = model.precent;
                            newUser.previos_expirments_num = model.previos_expirments_more;
                            newUser.yourself = yourself;
                            newUser.level_blind = model.level_blind;
                            //newUser.rule = "C";

                            if (model.birth_year > 0 && model.previos_expirments != " " && model.yourself != " " && blindYesNo != " " && gander != " ")
                            {
                                newUser.fill_somting = true;
                                _eyeMusicModel.complete_register = true;
                            }
                            else
                            {
                                newUser.fill_somting = false;
                                _eyeMusicModel.complete_register = false;
                            }

                            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
                            
                            try
                            {
                                entities.Entry(newUser).State = EntityState.Modified;
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
                        }
                        return View("first",_eyeMusicModel);
                    }

        }

        [HttpPost]
        public ActionResult CreateUser(eyemusic45.Models.ViewModels.userPass model, string ReturnUrl, string len, string gander,
            string blindYesNo, string expirment, string yourself)
        {
            if (ModelState.IsValid)
            {
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    string username = model.name;
                    string password = model.password;
                    string password2 = model.password2;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly
                    
                    userID userValid = entities.userIDs.Where(user => user.name == username && user.password == password).SingleOrDefault();

                    // User found in the database
                    if (userValid != null)
                    {
                        ModelState.AddModelError("", "This name already in use.");
                        return View("create_user");
                    }
                    if (username == "")
                    {
                        ModelState.AddModelError("", "Enter user name.");
                        return View("create_user");
                    }
                    if (username.Length > 30)
                    {
                        ModelState.AddModelError("", "user name is max 30 character");
                        return View("create_user");
                    }
                    else if (password == "" || password == null || password2 == null)
                    {
                        ModelState.AddModelError("", "Enter password.");
                        return View("create_user");
                    }
                    else if (password.Length > 10)
                    {
                        ModelState.AddModelError("", "password is max 10 character");
                        return View("create_user");
                    }
                    /*else if (entities.userIDs.Any(user => user.name == username))
                    {
                        ModelState.AddModelError("", "This name already in use.");
                        return View("create_user");
                    }*/
                    else if (password != password2)
                    {
                        ModelState.AddModelError("", "The confirm password is not identical to password.");
                        return View("create_user");
                    }
                    else if (model.birth_year < 0 || model.birth_year > DateTime.Now.Year) 
                    {
                        ModelState.AddModelError("", "The year of birth not legal.");
                        return View("create_user");
                    }
                    else if (model.previos_expirments == " " || model.yourself == " " || blindYesNo == " " || gander == " ")
                    {
                        ModelState.AddModelError("", "Please fill all details");
                        return View("create_user");
                    }
                    else
                    {

                        if (maxUser++ == 0)
                        {
                            try
                            {
                                maxUser = entities.userIDs.Max(user2 => user2.user_ID) + 1;
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        newUser = new userID();
                        newUser.name = username;
                        newUser.password = password;
                        newUser.user_ID = maxUser;

                        newUser.birth_year = model.birth_year;
                        newUser.age_of_blind = model.age_of_blind;
                        newUser.gander = gander;
                        newUser.place = model.place;
                        newUser.precent_blind = model.precent;
                        newUser.previos_expirments_num = model.previos_expirments_more;
                        newUser.yourself = yourself;
                        newUser.level_blind = model.level_blind;
                        //newUser.len = len;
                        //newUser.rule = "C";
                        String mapFile = Server.MapPath("~");
                        _eyeMusicModel = new Models.ViewModels.eyeMusicModel(mapFile);
                        _eyeMusicModel.userid = username;

                        vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

                        vh = new eyeMusic2(mapFile, _eyeMusicModel);


                        _eyeMusicModel.userDAL = newUser;


                        if (model.birth_year > 0 && model.previos_expirments != " " && model.yourself != " " && blindYesNo != " " && gander != " ")
                        {
                            newUser.fill_somting = true;
                            _eyeMusicModel.complete_register = true;
                        }
                        else
                        {
                            newUser.fill_somting = false;
                            _eyeMusicModel.complete_register = false;
                        }

                        System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
                        System.Web.HttpContext.Current.Session["eyeMusic"] = vh;

                        try
                        {
                            entities.userIDs.Add(newUser);
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

                        _eyeMusicModel.len = len;
                        ViewBag.len = _eyeMusicModel.len;
                        ViewBag.ReturnUrl = ReturnUrl;

                        return View("Login");
                    }

                }
            }

            ViewBag.len = _eyeMusicModel.len;

            ViewBag.ReturnUrl = ReturnUrl;
            return View("Login");
        }

        [HttpPost]
        //after the user write feedback (in view of feedback)
        public ActionResult feedbackBack(feedBackWithDetail model, string blindYesNo, string MainlingList)
        {
            Session.Add("name", "simon");
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

            Thread workerThread = new Thread(addToDBfeedback);
            model.datetime = DateTime.Now;
            model.user_ID = _eyeMusicModel.userDAL.user_ID;

            if (blindYesNo == "yes")
                model.blind = true;
            else if (blindYesNo == "no")
                model.blind = false;

            if (MainlingList == "yes")
                model.mailList = true;
            else if (MainlingList == "no")
                model.mailList = false;

            workerThread.Start(model);

            if (ViewBag.anonymous == null || ViewBag.anonymous == true)
                return View("firstAny", _eyeMusicModel);
            else
                return View("first", _eyeMusicModel);
        }


        [HttpPost]
        //if you aleardy have user or passord
        public ActionResult Login(userID model, string ReturnUrl,string len)
        {
            // Lets first check if the Model is valid or not
            //if (ModelState.IsValid)
            //{
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    string username = model.name;
                    string password = model.password;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    userID userValid = entities.userIDs.Where(user => user.name == username && user.password == password).SingleOrDefault();
                    // User found in the database
                    if (userValid != null)
                    {
                        _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
                        vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

                        
                        if (_eyeMusicModel == null || vh == null)
                        {
                            String mapFile = Server.MapPath("~");
                            _eyeMusicModel = new Models.ViewModels.eyeMusicModel(mapFile);
                        }

                        _eyeMusicModel.userid = username;
                        _eyeMusicModel.useridint = userValid.user_ID;
                        _eyeMusicModel.userDAL = userValid;
                        _eyeMusicModel.complete_register = userValid.fill_somting;
                        System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
                        _eyeMusicModel.len = len;
                        
                        FormsAuthentication.SetAuthCookie(username, false);


                        if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                            && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
               //}
            }
            ViewBag.anonymous = false;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("inner", "Home");
        }

        [Authorize]
        //change parmters of speed or cue type
        public ActionResult paramters()
        {
            
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            return View(_eyeMusicModel);
        }


        [Authorize]
        //the old version of eyemusic (lesson 1-5)
        public ActionResult selectclass()
        {
            Session.Add("username", "Simon");

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            //vh.startTraining("");
            _eyeMusicModel.UItrainingStimuliTreeView = vh._myManager.buildTrainingTree("\\Training\\");

            return View(_eyeMusicModel);
        }

        [HttpPost]
        [Authorize]
        //the old version 
        public ActionResult selectLevel(string selected_level)
        {
            _eyeMusicModel =
               (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }


            _eyeMusicModel.select_level = selected_level;
            //_eyeMusicModel.select_stage = selected_stage;

            return View(_eyeMusicModel);
        }


        //the new version of eyemusic
        public ActionResult selectclassTrain()
        {
            Session.Add("username", "Simon");

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh ==null)
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


        [HttpPost]
        public ActionResult selectLevelTrain(string selected_level)
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

          /*if (selected_level.Contains("Clothes"))
            {
                _eyeMusicModel.negative = true;
            }
            else
            {
                _eyeMusicModel.negative = false;
            }*/

            _eyeMusicModel.select_level = selected_level;

            return View(_eyeMusicModel);
        }
        
        [HttpPost]
        //the new version of eyemusic
        public ActionResult IselectedTrain(string selected_level, string selected_stage)
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
            _eyeMusicModel.select_stage = selected_stage;

            vh.startNewTraining(selected_level + "/" + selected_stage);

                string path = Server.MapPath("~");
                string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + selected_level + selected_stage + _eyeMusicModel.beep_noise.Replace(".mp3", "") + _eyeMusicModel.onlynames[0].Replace(".bmp", "") + ".bmp";

                _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
                _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;
                
                _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

                vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path_toLessen + "\\" + _eyeMusicModel.onlynames[0]);

                _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

                Thread workerThread = new Thread(addToDBTrain);

                ToAddToDB = new RespTrainShort();
            
                ToAddToDB.level1 = new level();
                ToAddToDB.stage1 = new stage();
                ToAddToDB.desacriptaion1 = new desacriptaion();

                ToAddToDB.level1.level1 = selected_level;
                ToAddToDB.stage1.stage1 = selected_stage;
                ToAddToDB.desacriptaion1.desacriptaion1 = _eyeMusicModel.onlynames[0].Substring(3).Replace("_", " ").Replace(".bmp", "").Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".JPG", "");;
                ToAddToDB.datetime = DateTime.Now;
                ToAddToDB.user_ID = _eyeMusicModel.userDAL.user_ID;
                workerThread.Start(ToAddToDB);


                return View(_eyeMusicModel);            
        }
        
        [HttpPost]
        [Authorize]
        //the old version of eyemusic 
        public ActionResult Iselected(string selected_level, string selected_stage)
        {
             _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
             vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

             if (_eyeMusicModel == null || vh == null)
             {
                 session_dead(System.Web.HttpContext.Current.User.Identity.Name);
             }


            _eyeMusicModel.select_level = selected_level;
            _eyeMusicModel.select_stage = selected_stage;

            vh.startTraining(selected_level + "/" + selected_stage);

                string path = Server.MapPath("~");
                string fileNameSeesion = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + selected_level + selected_stage + _eyeMusicModel.onlynames[0].Replace(".bmp", "") + ".bmp";

                _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameSeesion;
                _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion;
                
                _eyeMusicModel.currImagePathupload = _eyeMusicModel.currImagePath;

                vh.createMp3(fileNameSeesion.Replace(".bmp", ""), _eyeMusicModel.path_toLessen + "\\" + _eyeMusicModel.onlynames[0]);

                _eyeMusicModel.theUri = "/EM/Out/" + fileNameSeesion.Replace(".bmp", "") + ".mp3";

                Thread workerThread = new Thread(addToDBTrain);

                ToAddToDB = new RespTrainShort();

                ToAddToDB.level1 = new level();
                ToAddToDB.stage1 = new stage();
                ToAddToDB.desacriptaion1 = new desacriptaion();

                ToAddToDB.level1.level1 = selected_level;
                ToAddToDB.stage1.stage1 = selected_stage;
                ToAddToDB.desacriptaion1.desacriptaion1 = _eyeMusicModel.onlynames[0].Substring(3).Replace("_", " ").Replace(".bmp", "").Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".JPG", "");
                ToAddToDB.datetime = DateTime.Now;
                ToAddToDB.user_ID = _eyeMusicModel.userDAL.user_ID;
                workerThread.Start(ToAddToDB);
                return View(_eyeMusicModel);
        }

       
      
        public ActionResult about()
        {
            return View();
        }

        public ActionResult deleteAll()
        {
            //DirectoryInfo lessonFolder = new DirectoryInfo(Server.MapPath("~") + "\\EM\\Out\\");
            //FileInfo[] formatFileList = lessonFolder.GetFiles("*", SearchOption.AllDirectories);
            string[] toDeleteFiles = Array.FindAll(Directory.GetFiles(Server.MapPath("~") + "\\EM\\Out\\"), x => !x.StartsWith("50"));

            foreach (string forDel in toDeleteFiles)
            {
                System.IO.File.Delete(forDel);
            }

            toDeleteFiles = Array.FindAll(Directory.GetFiles(Server.MapPath("~") + "\\EM\\Images\\"), x => !x.StartsWith("50"));

            foreach (string forDel in toDeleteFiles)
            {
                System.IO.File.Delete(forDel);
            }

            toDeleteFiles = Directory.GetFiles(Server.MapPath("~") + "\\uploads\\");

            foreach (string forDel in toDeleteFiles)
            {
                System.IO.File.Delete(forDel);
            }

            toDeleteFiles = Directory.GetFiles(Server.MapPath("~") + "\\uploadReal\\");

            foreach (string forDel in toDeleteFiles)
            {
                System.IO.File.Delete(forDel);
            }
            return View("about");
        }

        [Authorize]
        //unity games
        public ActionResult virtual_training()
        {
            _eyeMusicModel =
            (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];


            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            if (_eyeMusicModel.complete_register)
                return View(_eyeMusicModel);
            else
                return View("complateDetails");
        }

        [Authorize]
        //camare with unity (not work good)
        public ActionResult camera()
        {
            _eyeMusicModel =
              (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];


            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            return View(_eyeMusicModel);
        }


        
        [AcceptVerbs(HttpVerbs.Post)]
        //in unity games proccess the image and return sound
        public FileStreamResult frameCountNeg()
        {
             _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
             vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

             if (_eyeMusicModel == null || vh == null)
             {
                 session_dead(System.Web.HttpContext.Current.User.Identity.Name);
             }

            HttpPostedFileBase photo = Request.Files["fileUpload"];
            MemoryStream streamM = new MemoryStream();

            if (photo != null)
            {
 
                Bitmap graphicsImage = System.Drawing.Image.FromStream(photo.InputStream, true, true) as Bitmap;
                streamM = vh.createWavNeg(photo.FileName.Replace(".png", "") + Session.SessionID + Id_number++.ToString(), graphicsImage);
            }
    
            //System.Diagnostics.Debug.WriteLine(_eyeMusicModel.theUri);
            var file = Server.MapPath(_eyeMusicModel.theUri);

            streamM.Position = 0;
            return File(streamM, "audio/wav");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        //in unity games proccess the image and return sound
        public FileStreamResult frameCount()
        {
             _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
             vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

             if (_eyeMusicModel == null || vh == null)
             {
                 session_dead(System.Web.HttpContext.Current.User.Identity.Name);
             }

            HttpPostedFileBase photo = Request.Files["fileUpload"];
            MemoryStream streamM = new MemoryStream();

            if (photo != null)
            {
 
                Bitmap graphicsImage = System.Drawing.Image.FromStream(photo.InputStream, true, true) as Bitmap;
                streamM = vh.createWav(photo.FileName.Replace(".png", "") + Session.SessionID + Id_number++.ToString(), graphicsImage);
            }
    
            //System.Diagnostics.Debug.WriteLine(_eyeMusicModel.theUri);
            var file = Server.MapPath(_eyeMusicModel.theUri);

            streamM.Position = 0;
            return File(streamM, "audio/wav");
        }

        //for avoid use with many memory delete the last ten picture on unity games
        private void deleteLastBmp(object obj)
        {
            try
            {
                System.IO.File.Delete((string)obj + ".bmp");
            }
            catch
            {

            }
        }

        //for avoid use with many memory delete the last ten sounds on unity games
        private void deleteLast(object obj)
        {
            try
            {
                System.IO.File.Delete((string)obj + ".mp3");
            }
            catch
            {

            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBfeedback(object obj)
        {
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    entities.feedBackWithDetails.Add((feedBackWithDetail)obj);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }
         
        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBShadow()
        {
          _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

             
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    shadow addRowS = new shadow();
                    addRowS.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRowS.level = int.Parse(Request.Params["level"]);

                    if (Request.Params["line"] != null)
                        addRowS.line = float.Parse(Request.Params["line"]);

                    if (Request.Params["direction"] != null)
                        addRowS.direction = Request.Params["direction"];

                    addRowS.freame =  int.Parse(Request.Params["frame"]);

                    if (Request.Params["success"] != null)
                    addRowS.success = int.Parse(Request.Params["success"]);
                    addRowS.datetime = DateTime.Now;
                    entities.shadows.Add(addRowS);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
       
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel1()
        {
          _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

             
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    addRow = new level_1();
                    addRow.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow.x = int.Parse(Request.Params["x"]);
                    addRow.y = int.Parse(Request.Params["y"]);
                    addRow.Color = Request.Params["Color"];
                    addRow.shape = Request.Params["Shape"];
                    addRow.feame = int.Parse(Request.Params["frame"]);
                    addRow.datetime = DateTime.Now; 
                    entities.level_1.Add(addRow);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
       
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel2()
        {
             _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    level_2 addRow2 = new level_2();
                    addRow2.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow2.X1 = int.Parse(Request.Params["X1"]);
                    addRow2.Y1 = int.Parse(Request.Params["Y1"]);
                   
                    addRow2.color1 = Request.Params["Color1"];
                    addRow2.shape1 = Request.Params["Shape1"];

                    addRow2.X2 = int.Parse(Request.Params["X2"]);
                    addRow2.Y2 = int.Parse(Request.Params["Y2"]);
                   
                    addRow2.color2 = Request.Params["Color2"];
                    addRow2.shape2 = Request.Params["Shape2"];

                    addRow2.Frame = int.Parse(Request.Params["frame"]);
                    addRow2.datetime = DateTime.Now;
                    entities.level_2.Add(addRow2);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel3()
        {
            _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    level_3 addRow3 = new level_3();
                    addRow3.level_unity = int.Parse(Request.Params["level"]);
                    addRow3.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow3.x = int.Parse(Request.Params["x"]);
                    addRow3.y = int.Parse(Request.Params["y"]);
                    addRow3.distance = int.Parse(Request.Params["distance"]);
                    addRow3.angle = int.Parse(Request.Params["angle"]);
                    addRow3.success = int.Parse(Request.Params["success"]);
                    addRow3.time_out = int.Parse(Request.Params["timeOut"]);
                    addRow3.frame = int.Parse(Request.Params["frame"]);
                    addRow3.datetime = DateTime.Now;
                    entities.level_3.Add(addRow3);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel4()
        {
             _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    level_4 addRow3 = new level_4();
                    addRow3.level_unity = int.Parse(Request.Params["level"]);
                    addRow3.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow3.x1 = int.Parse(Request.Params["x1"]);
                    addRow3.y1 = int.Parse(Request.Params["y1"]);
                    addRow3.x2 = int.Parse(Request.Params["x2"]);
                    addRow3.y2 = int.Parse(Request.Params["y2"]);
                    addRow3.x3 = int.Parse(Request.Params["x3"]);
                    addRow3.y3 = int.Parse(Request.Params["y3"]);
                    addRow3.num_collision = int.Parse(Request.Params["collison"]);
                    addRow3.numsuccess = int.Parse(Request.Params["numsuccess"]);
                    addRow3.frame = int.Parse(Request.Params["frame"]);
                    addRow3.datetime = DateTime.Now;
                    entities.level_4.Add(addRow3);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel5()
        {
           _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    level_5 addRow3 = new level_5();
                    addRow3.level_unity = int.Parse(Request.Params["level"]);
                    addRow3.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow3.x1 = int.Parse(Request.Params["x1"]);
                    addRow3.y1 = int.Parse(Request.Params["y1"]);
                    addRow3.num_collision = int.Parse(Request.Params["num_collision"]);
                    addRow3.frame = int.Parse(Request.Params["frame"]);
                    addRow3.datetime = DateTime.Now;
                    entities.level_5.Add(addRow3);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void addToDBUnityLevel6()
        {
               _eyeMusicModel =
        (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];


            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    level_6 addRow3 = new level_6();
                    addRow3.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addRow3.x1 = int.Parse(Request.Params["x1"]);
                    addRow3.y1 = int.Parse(Request.Params["y1"]);
                    addRow3.x2 = int.Parse(Request.Params["x2"]);
                    addRow3.y2 = int.Parse(Request.Params["y2"]);
                    addRow3.numsuccess = int.Parse(Request.Params["numsuccess"]);
                    addRow3.frame = int.Parse(Request.Params["frame"]);
                    addRow3.datetime = DateTime.Now;
                    entities.level_6.Add(addRow3);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

       //collect statistics on training
        public void addToDBTrain(object obj)
        {
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    int? maxlvl;
                    int? maxstg;
                    int? maxdes;
                    level the_level = new level();
                    stage the_stage = new stage();
                    desacriptaion the_description = new desacriptaion();

                    var levelVar = entities.levels.SingleOrDefault(usr => usr.level1 == ((RespTrainShort)obj).level1.level1);

                    if (levelVar != null)
                    {
                        the_level = levelVar;
                    }
                    else
                    {
                        maxlvl = entities.levels.Max(usr => (int?)usr.id);
                       
                        if (maxlvl == null)
                            maxlvl = 0;

                        the_level.id = (int)++maxlvl;
                        the_level.level1 = ((RespTrainShort)obj).level1.level1;

                    }

                    var stageVar = entities.stages.SingleOrDefault(usr => usr.stage1 == ((RespTrainShort)obj).stage1.stage1);

                    if (stageVar != null)
                    {
                        the_stage = stageVar;
                    }
                    else
                    {
                        maxstg = entities.stages.Max(usr => (int?)usr.id);

                        if (maxstg == null)
                            maxstg = 0;

                        the_stage.id = (int)++maxstg;
                        the_stage.stage1 = ((RespTrainShort)obj).stage1.stage1;
                    }

                    var desVar = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == ((RespTrainShort)obj).desacriptaion1.desacriptaion1);

                    if (desVar != null)
                    {
                        the_description = desVar;
                    }
                    else
                    {
                        maxdes = entities.desacriptaions.Max(usr => (int?)usr.id);

                        if (maxdes == null)
                            maxdes = 0;

                        the_description.id = (int)++maxdes;
                        the_description.desacriptaion1 = ((RespTrainShort)obj).desacriptaion1.desacriptaion1;
                    }

                    ((RespTrainShort)obj).level1 = the_level;
                    ((RespTrainShort)obj).stage1 = the_stage;
                    ((RespTrainShort)obj).desacriptaion1 = the_description;

                    entities.RespTrainShorts.Add((RespTrainShort)obj);
                    entities.SaveChanges();
                }
                catch(Exception e)
                {
                   
                }
            }
        }

       /// <summary>
       /// general statistics about unity games and HTML games
       /// </summary>
       /// <param name="num_level"></param>
       /// <param name="if_hide">if the user play without see the game</param>
       /// <param name="if_HTML">if this is HTML games</param>
        public void sendStart(string num_level,string if_hide, string if_HTML)
        {
               _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    virtualGame addvir = new virtualGame();
                    addvir.hide = Boolean.Parse(if_hide);
                    addvir.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addvir.datetime = DateTime.Now;
                    addvir.HTML5 = Boolean.Parse(if_HTML);
                    addvir.num_level = int.Parse(num_level);
                    entities.virtualGames.Add(addvir);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        //change the cue on start of stimulate
        public void changeBeep(string beepWav)
        {
               _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            _eyeMusicModel.beep_noise = beepWav;

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    parmater addpar = new parmater();
                    addpar.cue_type = beepWav.First().ToString();
                    addpar.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addpar.datetime = DateTime.Now;
                    addpar.filter = _eyeMusicModel.filter;
                    addpar.speed = _eyeMusicModel.ScanSpeed;
                    addpar.cue_Volume_10 = Convert.ToInt32(_eyeMusicModel.volume * 10);
                    entities.parmaters.Add(addpar);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }

        }

        public void changeFilter(string filter)
        {
            _eyeMusicModel =
                 (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

            _eyeMusicModel.filter = filter;
            if (filter == "g")
            {
                _eyeMusicModel.negative = true;
                _eyeMusicModel.black_and_white = false;

            }
            else if (filter == "b")
            {
                _eyeMusicModel.black_and_white = true;
                _eyeMusicModel.negative = false;
            }
            else
            {
                _eyeMusicModel.black_and_white = false;
                _eyeMusicModel.negative = false;
            }

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    parmater addpar = new parmater();
                    addpar.cue_type = _eyeMusicModel.beep_noise.First().ToString();
                    addpar.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addpar.datetime = DateTime.Now;
                    addpar.filter = filter;
                    addpar.cue_Volume_10 = Convert.ToInt32(_eyeMusicModel.volume * 10);
                    addpar.speed = _eyeMusicModel.ScanSpeed;
                    entities.parmaters.Add(addpar);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }


        //change the volume of cue
        public void changeVol(double Vol)
        {
               _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            _eyeMusicModel.volume = Vol;

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    parmater addpar = new parmater();
                    addpar.cue_type = _eyeMusicModel.beep_noise.First().ToString();
                    addpar.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addpar.datetime = DateTime.Now;
                    addpar.filter = _eyeMusicModel.filter;
                    addpar.cue_Volume_10 = Convert.ToInt32(Vol * 10);
                    addpar.speed = _eyeMusicModel.ScanSpeed;
                    entities.parmaters.Add(addpar);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        //change the speed of eyemusic 
        public void changeSpeed(int speed)
        {
               _eyeMusicModel =
                    (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

            _eyeMusicModel.ScanSpeedsecound = speed;
            _eyeMusicModel.ScanSpeed = speed / 5 ;

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    parmater addpar = new parmater();
                    addpar.cue_type = " ";
                    addpar.user_ID = _eyeMusicModel.userDAL.user_ID;
                    addpar.datetime = DateTime.Now;
                    addpar.filter = _eyeMusicModel.filter;
                    addpar.speed = speed;
                    addpar.cue_Volume_10 = Convert.ToInt32(_eyeMusicModel.volume * 10);
                    addpar.cue_type = _eyeMusicModel.beep_noise.First().ToString();
                    entities.parmaters.Add(addpar);
                    entities.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        
        

        //add all descriptions (image names) to DB 
        //use only after delete the table 
        //take a loot of time
        public void addallto()
        {
            DirectoryInfo lessonFolder = new DirectoryInfo(Server.MapPath("~") + "\\TrainInOrder\\");
            FileInfo[] formatFileList = lessonFolder.GetFiles("*", SearchOption.AllDirectories);
            int index = 0;
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                int maxdes;
                //desacriptaion des;
                desacriptaion des1;
                maxdes = 0;

                foreach (FileInfo fi in formatFileList)
                {
                    index++;
                    string new_image_name = fi.Name.Substring(3).Replace("_", " ").Replace(".bmp", "").Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", "");
                    var descVar = entities.desacriptaions.SingleOrDefault(usr => usr.desacriptaion1 == new_image_name);
                    if (descVar == null)
                    {
                        des1 = new desacriptaion();
                        des1.id = ++maxdes;
                        des1.desacriptaion1 = fi.Name.Substring(3).Replace("_", " ").Replace(".bmp", "").Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".JPG", "");
                        entities.desacriptaions.Add(des1);
                    }
                }

                entities.SaveChanges();
            }
        }
        
        public ActionResult feedback()
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

            ViewBag.len = _eyeMusicModel.len;
            return View();
        }

  

        /*
        [HttpPost]
        // In HTML games 
        //get the image and return sound
        public ActionResult getGameImg(string pdfString)
        {
            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            addPathForMp3();

            byte[] imageData = Convert.FromBase64String(pdfString);

            
            //Thread workerThread = new Thread(deleteLast);
            //Thread workerThread2 = new Thread(deleteLastBmp);
            //workerThread.Start(_eyeMusicModel.path + "\\EM\\Out\\" + Session.SessionID + (Id_number - 10).ToString());
            //workerThread2.Start(_eyeMusicModel.path + "\\EM\\Images\\" + Session.SessionID + (Id_number - 10).ToString());
            

            TimeSpan diff = DateTime.UtcNow - baseDate;

            string fileNameSeesion = Session.SessionID + Id_number++;
            _eyeMusicModel.realpath = _eyeMusicModel.path + "\\EM\\Images\\" + fileNameSeesion + ".bmp";
            vh.createMp3Save(fileNameSeesion, new Bitmap(Image.FromStream(new MemoryStream(imageData), true, true)), true);
            _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameSeesion + ".bmp";

            return Json(new { voice = _eyeMusicModel.theUri, img = _eyeMusicModel.currImagePath, time = diff.TotalMilliseconds});
        }

         * */

        [Authorize]
        public ActionResult complateDetails()
        {
            return View();
        }

        public void changeLen(string len)
        {
            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];


            if (_eyeMusicModel == null || vh == null)
            {
                session_dead(System.Web.HttpContext.Current.User.Identity.Name);
            }

            vh.heb.changeLan(len);

            ViewBag.len = len;

            if (_eyeMusicModel != null)
                _eyeMusicModel.len = len;
        }

        //found this user in DB after the cookie delete
        public void session_dead(string user_name)
        {
            String mapFile = Server.MapPath("~");
            _eyeMusicModel = new Models.ViewModels.eyeMusicModel(mapFile);
            _eyeMusicModel.userid = user_name;

            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];
            vh = new eyeMusic2(mapFile, _eyeMusicModel);

            if (_eyeMusicModel.userDAL == null)
            {
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    newUser = new userID();
                    var first_new_user = entities.userIDs.Where(user2 => user2.name == user_name);

                    foreach (var item in first_new_user)
                    {
                        _eyeMusicModel.userDAL = item;
                        _eyeMusicModel.complete_register = item.fill_somting;
                    }
                }

            }

            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
            System.Web.HttpContext.Current.Session["eyeMusic"] = vh;
        }

        //get the user from cokkies
        public void not_session_dead()
        {
            ViewBag.anonymous = true;

            addPathForMp3();

            string user_name = Request.ServerVariables["REMOTE_ADDR"].ToString();

            _eyeMusicModel = (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];
            String mapFile = Server.MapPath("~");

            if (_eyeMusicModel == null)
            {
                _eyeMusicModel = new Models.ViewModels.eyeMusicModel(mapFile);
                _eyeMusicModel.userid = user_name;
                _eyeMusicModel.len = "e";
            }

            vh = new eyeMusic2(mapFile, _eyeMusicModel);

            if (_eyeMusicModel.userDAL == null)
            {
                using (amedilab_dataEntities entities = new amedilab_dataEntities())
                {
                    try { 
                    var first_new_user = entities.userIDs.Where(user2 => user2.name == user_name).SingleOrDefault();

                    if (first_new_user != null)
                    {
                        _eyeMusicModel.userDAL = first_new_user;
                    }
                    else
                    {
                        if (maxUser++ == 0)
                        {
                            try
                            {
                                maxUser = entities.userIDs.Max(user2 => user2.user_ID) + 1;
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        newUser = new userID();
                        newUser.name = user_name;
                        newUser.password = " ";
                        newUser.user_ID = maxUser;
                        newUser.birth_year = 1900;
                        newUser.age_of_blind = 0;
                        newUser.gander = "m";
                        newUser.place = "isreal";
                        newUser.precent_blind = 0;
                        newUser.previos_expirments_num = 0;
                        newUser.yourself = "y";
                        newUser.level_blind = " ";
                        newUser.fill_somting = true;
                        entities.userIDs.Add(newUser);
                        try
                        {
                            entities.SaveChanges();
                        }
                        catch (Exception e)
                        {

                        }
                        _eyeMusicModel.userDAL = newUser;
                    }
                    }
                    catch (Exception e) 
                    {
                        newUser = new userID();
                        newUser.name = user_name;
                        newUser.password = " ";
                        newUser.user_ID = maxUser;
                        newUser.birth_year = 1900;
                        newUser.age_of_blind = 0;
                        newUser.gander = "m";
                        newUser.place = "isreal";
                        newUser.precent_blind = 0;
                        newUser.previos_expirments_num = 0;
                        newUser.yourself = "y";
                        newUser.level_blind = " ";
                        newUser.fill_somting = true;
                        _eyeMusicModel.userDAL = newUser;                    
                    }
                }
            }

            System.Web.HttpContext.Current.Session["Themodel"] = _eyeMusicModel;
            System.Web.HttpContext.Current.Session["eyeMusic"] = vh;
        }

        private void addPathForMp3()
        {
            if (binPath == "")
            {
                binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "bin" });
                // get current search path from environment
                var path = Environment.GetEnvironmentVariable("PATH") ?? "";

                // add 'bin' folder to search path if not already present
                if (!path.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
                {
                    path = string.Join(Path.PathSeparator.ToString(), new string[] { path, binPath });
                    Environment.SetEnvironmentVariable("PATH", path);
                }
            }

        }


        [HttpPost]
        //select another picture on the lesson 
        //work with ajax and send the image and the sound 
        public ActionResult ajaxreg(string fileName)
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


            if (fileName != null)
            {

                string[] alldb = fileName.Split('/');

                string DBnumber = alldb[alldb.Length - 1];
                string DBstage = alldb[alldb.Length - 2];
                string DBclass = alldb[alldb.Length - 3];

                string fileNameshort = _eyeMusicModel.ScanSpeed + _eyeMusicModel.filter + DBclass + DBstage + _eyeMusicModel.beep_noise.Replace(".mp3", "") + DBnumber.Replace(".bmp", "");

                string fileNameWithSeesion = fileName;


                string path = Server.MapPath("~");
                _eyeMusicModel.realpath = path + "\\EM\\Images\\" + fileNameshort + ".bmp";
                _eyeMusicModel.theUri = "/EM/Out/" + fileNameshort + ".mp3";

                _eyeMusicModel.currImagePath = "/EM/Images/" + fileNameWithSeesion;

                vh.createMp3(fileNameshort, path + fileName.Replace("/", "\\"));

                Thread workerThread = new Thread(addToDBTrain);
                ToAddToDB = new RespTrainShort();

                ToAddToDB.level1 = new level();
                ToAddToDB.stage1 = new stage();
                ToAddToDB.desacriptaion1 = new desacriptaion();

                ToAddToDB.desacriptaion1.desacriptaion1 = DBnumber.Substring(3).Replace("_", " ").Replace(".bmp", "").Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".JPG", "");
                ToAddToDB.level1.level1 = DBclass;
                ToAddToDB.stage1.stage1 = DBstage;
                ToAddToDB.datetime = DateTime.Now;
                ToAddToDB.user_ID = _eyeMusicModel.userDAL.user_ID;


                workerThread.Start(ToAddToDB);
            }


            return Json(new { image = "/EM/Images/" + _eyeMusicModel.bmpName + ".bmp", voice = _eyeMusicModel.theUri });
        }

        [HttpPost]
        public ActionResult TrainResualtName(string NameInput)
        {
                        List<showTrain> for_sent = new List<showTrain>();

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    userID userValid = entities.userIDs.Where(user => user.name == NameInput).SingleOrDefault();

                    if (userValid != null)
                    {
                        var resualt = entities.RespTrainViews.Where(theresp => theresp.ID == userValid.user_ID).OrderByDescending(user => user.datetime);

                        foreach (RespTrainView forAdd in resualt)
                        {
                            for_sent.Add(new showTrain(forAdd.level, forAdd.stage,forAdd.desacriptaion, forAdd.datetime));
                        }

                    }


                }
                catch (Exception e)
                {

                }
            }

            return View("TrainResualt", for_sent);
        }

         [HttpPost]
        public ActionResult TrainResualt(int userId)
        {
            List<showTrain> for_train = new List<showTrain>();
            List<resualt_exam_row> for_test = new List<resualt_exam_row>();

             using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
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
                    

           
                    userID userSelect = entities.userIDs.Where(user => user.user_ID == userId).SingleOrDefault();
                    
                    if (userSelect != null)
                    {
                        var usrids = userSelect.user_ID;

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
                  "order by [level], [stage]", usrids);


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

                            rsnew.level = rs.level.Substring(2).Replace("_", " ").Trim();
                            rsnew.stage = rs.stage.Substring(2).Replace("_", " ").Trim();
                            for_test.Add(rsnew);
                        }
                        //var resualt =  entities.RespTrainViews.Where(theresp => theresp.ID == userSelect.user_ID).OrderByDescending(user => user.datetime);
                        /*                    
                        var resualtT = entities.Database.SqlQuery<examWithLevel>(" select exams.num_exam, " +
                            "case when ques.correct = 1 then 1 else 0 end as max_num_question, " +
			                        "lvl.[level], " +
			                        "stg.[stage], " +
			                        "ques.[user_ID] " +
                                "FROM [" + schema + "].[exam] ques, " +
                                        "[" + schema + "].[levels] lvl, " +
                                        "[" + schema + "].[stages] stg, " +
                                        "[" + schema + "].[exam_q_levels] exams " +
                        "WHERE exams.exam_stage = stg.id " +
	                    "AND exams.exam_level = lvl.id " +
	                    "AND exams.num_exam = ques.num_exam " +
	                    "AND exams.num_question = ques.num_q " +
	                    "AND exams.exam_level = ques.[level] " +
	                    "AND exams.exam_stage = ques.[stage] " +
                        "And  ques.user_ID = @p0", usrids).ToList();

                        //var resualtT = entities.examWithLevels.Where(theresp2 => theresp2.user_ID == userSelect.user_ID).OrderByDescending(user1 => user1.level).ThenBy(user2 => user2.stage);

                        foreach (examWithLevel forAdd in resualtT)
                        {
                            for_test.Add(new showTests(forAdd.level, forAdd.stage, forAdd.num_exam, (int)forAdd.max_num_question));
                        }
                        */
                        /*
                        var resualt = entities.Database.SqlQuery<RespTrainView>("select [desc].desacriptaion as desacriptaion, " +
                          " stg.stage as [stage], " +
                          " lvl.[level] as [level], " +
                           "[resp].[user_ID] as [ID], " +
                           "[resp].[datetime] as [datetime] " +
                         "FROM [" + schema + "].[levels] lvl, " +
                         "[" + schema + "].[stages] stg, " +
                         "[" + schema + "].[desacriptaions] [desc], " +
                         "[" + schema + "].[RespTrainShort] [resp] " +
                         "where resp.[level] = lvl.[id] " +
                         "AND resp.[stage] = stg.[id] " +
                         "AND resp.[desacriptaion] = [desc].id " +
                         "And [resp].[user_ID] = @p0", usrids);

                        foreach (RespTrainView forAdd in resualt)
                        {
                            for_train.Add(new showTrain(forAdd.level, forAdd.stage, forAdd.desacriptaion, forAdd.datetime));
                        }
                        */

                    }
                }
                catch (Exception e)
                {
                    for_train.Add(new showTrain(e.Message,"","",DateTime.Now));
                }
            }

             return View(new showUserAll(for_test,for_train));
        }

        public ActionResult feedbackReturn()
        {
            List<userList> for_sent = new List<userList>();

            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    /*
                    var allUsers = entities.userIDs.Where(user => user.name.Split('.').Count() != 4);
                    */
                    var allUsers = (from p in entities.feedBackWithDetails
                                       select p).ToList();


                    foreach(feedBackWithDetail forAdd in allUsers)
                    {
                        for_sent.Add(new userList(forAdd.user_ID,forAdd.free_text));
                    }

                }
                catch(Exception e)
                {

                }
            }

            return View("TrainResualtSelect",for_sent);

        }



        [Authorize]
        public ActionResult TrainResualtSelect()
        {
            List<userList> for_sent = new List<userList>();
            using (amedilab_dataEntities entities = new amedilab_dataEntities())
            {
                try
                {
                    /*
                    var allUsers = entities.userIDs.Where(user => user.name.Split('.').Count() != 4);
                    */
                    var allUsers = (from p in entities.userIDs
                                where (!p.name.Contains(".") )
                                       orderby p.name ascending
                                       select p).ToList();


                    foreach(userID forAdd in allUsers)
                    {
                        for_sent.Add(new userList(forAdd.user_ID,forAdd.name));
                    }

                }
                catch(Exception e)
                {

                }
            }

            return View(for_sent);
        }

        public ActionResult gameMemoryStream()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getGameImg(string pdfString)
         {
             _eyeMusicModel =
               (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];

             pdfString = pdfString.Replace(" ", "+");
             vh = (eyeMusic2)System.Web.HttpContext.Current.Session["eyeMusic"];

             if (_eyeMusicModel == null || vh == null)
             {
                 session_dead(System.Web.HttpContext.Current.User.Identity.Name);
             }

             addPathForMp3();

             byte[] imageData = Convert.FromBase64String(pdfString);

             TimeSpan diff = DateTime.UtcNow - baseDate;

             string fileNameSeesion = Session.SessionID + Id_number++;
             _eyeMusicModel.realpath = _eyeMusicModel.path + "\\EM\\Images\\" + fileNameSeesion + ".bmp";

             MemoryStream streamM = new MemoryStream();

             Bitmap graphicsImage = new Bitmap(Image.FromStream(new MemoryStream(imageData), true, true));
             streamM = vh.createWavCue(fileNameSeesion, graphicsImage);
             
             streamM.Position = 0;
             string j = _eyeMusicModel.theUri;
             return new FileContentResult(streamM.ToArray(), "audio/wav");
         }
    }
}
