using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
//using Thinktecture.IdentityModel.Http.Cors.Mvc;
namespace eyemusic45
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //RegisterCors(MvcCorsConfiguration.Configuration);
        }

        /*
        private void RegisterCors(MvcCorsConfiguration corsConfig)
        {
            corsConfig
               .ForResources("Home.sendStart")
               .ForOrigins("http://localhost:55795","http://brain.huji.ac.il")
               .AllowAll();

            corsConfig
               .ForResources("Home.getGameImg")
               .ForOrigins("http://localhost:55795","http://brain.huji.ac.il")
               .AllowAll();       
        }
        */

        protected void Session_End(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Session_End");
            string sesid = this.Session.SessionID;

            string path = HttpRuntime.AppDomainAppPath;
            //string path = Server.MapPath("~");

            DirectoryInfo di = new DirectoryInfo(path + "\\EM\\Images\\");
            FileInfo[] files = di.GetFiles("*"+ sesid +".bmp")
                                 .Where(p => p.Extension == ".bmp").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch { }

            di = new DirectoryInfo(path + "\\EM\\Out\\");

             files = di.GetFiles("*" + sesid + ".wav")
                                 .Where(p => p.Extension == ".wav").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch { }

            di = new DirectoryInfo(path + "\\uploads");

            files = di.GetFiles("*" + sesid + ".bmp")
                                .Where(p => p.Extension == ".bmp").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch { }

        }
    }
}