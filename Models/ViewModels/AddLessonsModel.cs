using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Models.ViewModels
{
    public class AddLessonsModel
    {
        public string map;
        public string[] allDirectory;
        public string[] allImages;
        public string DirName;
        public string picFullPath;
        public string imgShowPath;

        //the directory in hebrew 
        public string[] allHebrewDir;

        //scan speed in milisecound per column
        public int ScanSpeed;

        //The sound URL
        public string theUri;

        //The image URL
        public string currImagePath;

        //all tranlate image 
        public string[] imageHebrew;

        //images URL 
        public string[] WebPath;

        public AddLessonsModel()
        {

        }
    }
}