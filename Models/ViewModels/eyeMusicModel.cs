using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eyemusic45.Business;

namespace eyemusic45.Models.ViewModels
{
    public class eyeMusicModel
    {
        public eyeMusicModel(string mapFile)
        {
            onlyExam = false;
            beep_noise = "beep.wav";
            volume = 0.6;
            withAnswer = true;
            //milisecound per coulman 
            ScanSpeed = 50;

            ScanSpeedsecound = 250;
            exam_number = 0;

            //the langugae  
            len = "e";

            //the num of question in current exam

            num_question_step = 1;

            TimeToExam = 0;
            TimeTotalExp = 1000 * 60 * 82;
            //Timer = DateTime.MaxValue;
            TimeEnd = double.MaxValue;

            //read all library names 
            if (UItrainingStimuliTreeViewNew == null)
            {
                UItrainingStimuliTreeViewNew = System.IO.File.ReadAllLines(mapFile + "/Texts/ENG/Dir.txt")[0];
                UItrainingStimuliHebrow = System.IO.File.ReadAllLines(mapFile + "/Texts/HEB/Dir.txt")[0];
                UItrainingStimuliFrance = System.IO.File.ReadAllLines(mapFile + "/Texts/FRN/Dir.txt")[0];
                UItrainingStimuliGerman = System.IO.File.ReadAllLines(mapFile + "/Texts/GRM/Dir.txt")[0];
                UItrainingStimuliCzech = System.IO.File.ReadAllLines(mapFile + "/Texts/CST/Dir.txt")[0];
            }

            path = mapFile;
            negative = false;
            black_and_white = false;
            filter = "";
        }

        public bool onlyExam;
        //public DateTime Timer;
        public int TimeToExam;
        public double TimeTotalExp;
        public string imagePath;

        //URL of sound 
        public string theUri;

        //the URL of image
        public string currImagePath;

        //the path in server of website
        public string path;        

        //public string UItrainingStimuliTreeView;

        //all image pathes in current lesson 
        public string[] allBmpFiles;
        public string path_toLessen;

        //the names without path 
        public string[] onlynames;

        //the URLO of image that upluad from client
        public string currImagePathupload;

        //path in server to image
        public string realpath;

        //URL to image in herzel directory
        public string webpath;

        public string beep_noise;
        public double volume;
        public double speed = 1.0;

        //the four answers of exams 
        public string[] answers;

        //the name of image that create after resize
        public string bmpName;

        //in training 
        public string select_stage;
        public string select_level;


        public string userid;
        public int useridint;
        public DAL.userID userDAL;

        //in drag and drop incrase in 1 at any upload 
        public int dragid;
        public int ScanSpeed;

        public object ScanSpeedsecound;

        // all lessons in one string 
        public string UItrainingStimuliTreeViewNew;
        public string UItrainingStimuliHebrow;
        public string UItrainingStimuliFrance;
        public string UItrainingStimuliGerman;
        public string UItrainingStimuliCzech;

        public string[] allBmpFilesAtr;
        public int[] thisQuestions;

        public int theTrue;
        public int exam_number;
        public int num_question;
        public string previos_path;
        public string[] image_names;
        public string[] path_for_image;
        public bool complete_register;
        public int thePrevTrue;
        public string len;
        public string NumberPath;

        //the path of svg file
        public string svgHtmlPath;

        //the name of svg file and sound file (without the termination)
        public string SvgGlobalName;

        public StepByStepClass StepSession;

        //string with "," of texts and the frenquncy and the location for mespaek
        public string SvgNumbers;

        //list of all lessons that user did in stepbystep 
        public string[] StepByStepLessons;

        //the entry in index for the lessons
        public int[] StepByStepListInts;

        //if show the image or only descruiption
        public bool blind;

        //the num of question in current exam
        public int num_question_step;

        //the file that upload by user (svg or dragdrop)
        public HttpPostedFileBase file;
        public bool SoundAfterPicture;

        public ExpStepClass ExpStep;

        //the total number of question in this exam
        public int totalNumQuestion;

        //old lesson list
        public string UItrainingStimuliTreeView;
        public bool withAnswer;
        public int totalNumImageInLesson;
        public int imagePass;
        public bool finishExam = false;
        public double TimeEnd;
        public bool negative;
        public bool black_and_white;
        public string filter;
        public bool showNextButton = true;
    }
}