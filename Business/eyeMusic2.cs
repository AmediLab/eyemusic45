using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using eyemusic45.Models.ViewModels;
using NAudio.Wave;
using NAudio.Lame;
using System.Drawing.Imaging;


/**
 * Handels the eyeMusic GUI   
 * */
namespace eyeMusic45
{
    public partial class eyeMusic2
    {
        //variables
        public eyeMusicModel model;
        public eyemusic45.Business.languages heb;

        public enum Volume { Loud, Medium, Low };
        public enum InputType { Training, Exam, Camera, ScreenSonifier, ActiveWindow, Word, Oval, VirtualTraining,NewTraining };
        private InputType _myInputType;
        private double _myVolume;
        public enum CueType { Drum, Beat, Click, Beep, NoCue };
        private CueType _myCueType = CueType.Drum;
        private bool _stopOval = false;
        private bool _firstStimClicked = false;
        private bool _zoomMode = false;
        private bool _ovalMode = false;
        private bool _trainingMode = false;
        private bool _examMode = false;
        private bool _edgeDetectionMode = false;
        private bool _keepImageRatio = false;
        private string _traineeName;
        private string _selectedStage;
        private string _selectedLesson;
        private string _answer;
        private string _letterFolder;

        // Resolution reduction resize mode
        private System.Drawing.Drawing2D.InterpolationMode _resizeMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

        private wavCreator _myWavCreator;
        public managementGUI _myManager;
        private eyeMusicStatistics2 _myStatisticsObj;
        private ZoomScreen _myZoomScreen;

        // Directory in My Documents
        private string _EMDirectory;
        private string _logDirectory;
        private string _outDirectory;
        private string _syncDirectory;
        private string _soundDirectory;
        private string _imagesDirectory;

        //private EventLog _eventLog;

        public const double Loud = 1;
        public const double Medium = 0.4;
        public const double Low = 0.1;

        private int _formHeight = 730;
        private int _regFormWidth = 650;
        private int _inlargedFormWidth = 1017;

        public CueType MyCueType { get { return _myCueType; } set { _myCueType = value; } }
        public InputType MyInputType { get { return _myInputType; } set { _myInputType = value; } }
        public double MyVolume { get { return _myVolume; } set { _myVolume = value; } }
        public bool StopOval { get { return _stopOval; } set { _stopOval = value; } }
        public bool FirstStimClicked { get { return _firstStimClicked; } set { _firstStimClicked = value; } }
        public bool ZoomMode { get { return _zoomMode; } set { _zoomMode = value; } }
        public string EMDirectory { get { return _EMDirectory; } set { _EMDirectory = value; } }
        public string LogDirectory { get { return _logDirectory; } set { _logDirectory = value; } }
        public string OutDirectory { get { return _outDirectory; } set { _outDirectory = value; } }
        public string SyncDirectory { get { return _syncDirectory; } set { _syncDirectory = value; } }
        public string SoundDirectory { get { return _soundDirectory; } set { _soundDirectory = value; } }
        public string ImagesDirectory { get { return _imagesDirectory; } set { _imagesDirectory = value; } }
        public System.Drawing.Drawing2D.InterpolationMode ResizeMode { get { return _resizeMode; } set { _resizeMode = value; } }
        public bool OvalMode { get { return _ovalMode; } set { _ovalMode = value; } }
        public String TraineeName { get { return _traineeName; } set { _traineeName = value; } }
        public string SelectedStage { get { return _selectedStage; } set { _selectedStage = value; } }
        public string SelectedLesson { get { return _selectedLesson; } set { _selectedLesson = value; } }
        public bool EdgeDetectionMode { get { return _edgeDetectionMode; } set { _edgeDetectionMode = value; } }
        public bool KeepImageRatio { get { return _keepImageRatio; } set { _keepImageRatio = value; } }
        public int FormHeight { get { return _formHeight; } set { _formHeight = value; } }
        public int RegFormWidth { get { return _regFormWidth; } set { _regFormWidth = value; } }
        public int InlargedFormWidth { get { return _inlargedFormWidth; } set { _inlargedFormWidth = value; } }
        public string LetterFolder { get { return _letterFolder; } set { _letterFolder = value; } }
        public string _mapFile;

        // constructor
        public eyeMusic2(string mapFile, eyeMusicModel eyeMusicModel)
        {
             heb = new eyemusic45.Business.languages(mapFile, false);
             model = eyeMusicModel;
            _mapFile = mapFile;
            _myStatisticsObj = new eyeMusicStatistics2(this);
            _myWavCreator = new wavCreator(this, _myStatisticsObj,mapFile);
            _myManager = new managementGUI(this, _myWavCreator, _myStatisticsObj,model);
            _myZoomScreen = new ZoomScreen(this, _myManager, _myWavCreator);

            _letterFolder = "\\HebrewLettersExperiment\\02-Bold font\\";
            _myVolume = Loud;
            _answer = "";
            createDirectories();          
            model.theUri =  "/EM/Out/Out.wav";

            MyInputType = InputType.Training;
            _myManager.startSession();
        }

        ///*
        // * insures the needed directories exist in the system
        // * */
        private void createDirectories()
        {
            // Check if EM directoy exists, if not create it 
            _EMDirectory = _mapFile + "\\EM";
            if (!Directory.Exists(_EMDirectory))
            {
                Directory.CreateDirectory(_EMDirectory);
            }

            // Check if logs directoy exists, if not create it 
            _logDirectory = _EMDirectory + "\\Logs\\";
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            // Check if out directoy exists, if not create it 
            _outDirectory = _EMDirectory + "\\Out\\";
            if (!Directory.Exists(_outDirectory))
            {
                Directory.CreateDirectory(_outDirectory);
            }

            // Check if sync directoy exists, if not create it 
            _syncDirectory = _EMDirectory + "\\Sync\\";
            if (!Directory.Exists(_syncDirectory))
            {
                Directory.CreateDirectory(_syncDirectory);
            }

            _imagesDirectory = _EMDirectory + "\\Images\\";
            if (!Directory.Exists(_imagesDirectory))
            {
                Directory.CreateDirectory(_imagesDirectory);
            }

            _soundDirectory = _EMDirectory + "\\Sounds\\";
            if (!Directory.Exists(_soundDirectory))
            {
                Directory.CreateDirectory(_soundDirectory);
            }
        }

        /// <summary>
        /// create mp3 file from bitmap and save it. 
        /// </summary>
        /// <param name="fileName">name of bitmap and name for output mp3 file </param>
        /// <param name="bitmap">The Image</param>
        /// <param name="create">Save or not save</param>
        public void createMp3Save(string fileName, Bitmap bitmap,bool create)
        {
            model.bmpName = fileName;
            _myWavCreator.NegativeImageMode = model.negative;

            MemoryStream ms = _myWavCreator.createWavMemory(bitmap, fileName, create,true);
            ms.Position = 0;

            using (var retMs = File.OpenWrite(OutDirectory + "\\" + fileName + ".mp3"))
            using (var rdr = new WaveFileReader(ms))
            using (var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, 128))
            {
                rdr.CopyTo(wtr);
            }

            model.theUri = "/EM/Out/" + fileName + ".mp3";
        }

        /// <summary>
        /// Create mp3 from file name 
        /// </summary>
        /// <param name="fileName">The output mp3 file name</param>
        /// <param name="fileFull">Full image path</param>
        public void createMp3(string fileName, string fileFull)
        {
            model.bmpName = fileName.Replace(".bmp", "");

            using (var bmpTemp = new Bitmap(fileFull))
            {
                Bitmap bitmap = new Bitmap(bmpTemp);
                _myWavCreator.NegativeImageMode = model.negative;


                //                if (!File.Exists(OutDirectory + "\\" + fileName + ".mp3") || !File.Exists(model.realpath))

                if (!File.Exists(OutDirectory + "\\" + fileName + ".mp3"))
                {
                    MemoryStream ms = _myWavCreator.createWavMemory(bitmap, fileName, true, true);
                    ms.Position = 0;

                    using (var retMs = File.OpenWrite(OutDirectory + "\\" + fileName + ".mp3"))
                    using (var rdr = new WaveFileReader(ms))
                    using (var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, 128))
                    {
                        rdr.CopyTo(wtr);
                    }
                }
            }
        }

        /// <summary>
        /// Create wav from Image 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public MemoryStream createWav(string fileName,Bitmap bitmap)
        {
            MemoryStream ms = _myWavCreator.createWavMemory(bitmap, fileName,false,false);
            return ms;
        }

        /// <summary>
        /// Create wav from Image 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public MemoryStream createWavCue(string fileName, Bitmap bitmap)
        {
            MemoryStream ms = _myWavCreator.createWavMemory(bitmap, fileName, false, true);
            return ms;
        }

        /// <summary>
        /// sets stopSession to true to continue program flow
        /// </summary>
        public void setStopSession()
        {
            _myWavCreator.StopSession = false;
        }

       

        ///**
        // * Get selected folder and build gallery accordingly- calls 'buildGallery(e.Node.FullPath)' of managementGUI.cs.
        // * */
        public void startTraining(string addPath)
        {
            model.allBmpFiles = _myManager.prepareTrainingGallery(addPath);
        }

        /// <summary>
        /// Create tree of folder for Trainging
        /// </summary>
        /// <param name="addPath"></param>
        public void startNewTraining(string addPath)
        {
            model.allBmpFiles = _myManager.prepareTrainingGalleryNew(addPath);
        }

        /// <summary>
        /// Create tree of folder for exam 
        /// </summary>
        /// <param name="addPathlevel"></param>
        /// <param name="addPathStage"></param>
        public void startExam(string addPathlevel , string addPathStage)
        {
            model.path_toLessen = model.path + managementGUI.TRAINING_NEW_FOLDER_PATH + addPathlevel + "\\" + addPathStage + "\\";
            _myManager.create_exam_for_path(managementGUI.TRAINING_NEW_FOLDER_PATH.Replace("\\", "/") + addPathlevel + "/" +  addPathStage + "/");
        }

        /// <summary>
        /// 
        /// </summary>
        private void endTraining()
        {
            if (_trainingMode)
            {
                // Statistics

                // If a stimulus was already clicked on, log entry.
                if (_firstStimClicked)
                {
                    _myStatisticsObj.stopCollecting();
                    _myStatisticsObj.writeLogString();
                }

                // Write total time
                _myStatisticsObj.pauseTimers();
                _myStatisticsObj.writeTrainingLogString();
                _myStatisticsObj.closeLogFile();


                // GUI
                model.UItrainingStimuliTreeView = "";

            }
        }

        public void endExam()
        {
            if (_examMode)
            {
                // Statistics

                _myStatisticsObj.stopCollecting();
                _myStatisticsObj.writeLogString();

                // Write total time
                _myStatisticsObj.pauseTimers();
                _myStatisticsObj.writeExamLogString();
                _myStatisticsObj.closeLogFile();


                // GUI
            }
            //model.UIdoneExamButton = false;
            //model.UIpauseExamButton = false;

           // model.UIbeginExamButton = true;
            //model.UIexamTreeView = "";
            //model.UInextStimulusButton = false;
            //model.UIcurrentStimulusNumberLabel = "--";
            //model.UItotalStimuliLabel = "--";
            //model.UIexamFileNameLabel = "No file loaded";

            //model.UIexamGalleryPanel = false;
            //model.UIexamToolStripMenuItem = false;
            //UItraineesAnswerPanel.Controls.Clear();
            //model.UIexamPanelVisible = false;
            //this.Size = new System.Drawing.Size(RegFormWidth, FormHeight);
            //_examMode = false;
        }

        /// <summary>
        /// Change Image to fit 50 * 30 and save it if not exist 
        /// </summary>
        /// <param name="bitmap">The Bitmap to save</param>
        public void setScanPictureBoxes(Bitmap bitmap)
        {
            Bitmap resizedImage = managementGUI.resizeImage(bitmap, _myWavCreator.ScanWidth * 5, _myWavCreator.ScanHeight * 5,
                                                            System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor, KeepImageRatio);
            _myZoomScreen.setScanImage(resizedImage);

            if (!File.Exists(model.realpath))
            {
                /*
                using (var m = new MemoryStream())
                {
                    bitmap.Save(m, ImageFormat.Bmp);
                    var img = Image.FromStream(m);
                    img.Save(model.realpath);
                }*/
                resizedImage.Save(model.realpath);
            }

            if (model.realpath.Contains("\\uploads\\"))
                bitmap.Save(model.realpath.Replace("\\uploads\\", "\\uploadReal\\"));

        }

        /// <summary>
        /// Change Image to fit 50 * 30 and save it if not exist 
        /// </summary>
        /// <param name="bitmap">The image to save</param>
        /// <param name="name">the name for save the image</param>
        public void setScanPictureBoxes(Bitmap bitmap,string name)
        {
            Bitmap resizedImage = managementGUI.resizeImage(bitmap, _myWavCreator.ScanWidth * 5, _myWavCreator.ScanHeight * 5,
                                                            System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor, KeepImageRatio);

            model.realpath = model.path + "\\EM\\Images\\" + name + ".bmp";

            _myZoomScreen.setScanImage(resizedImage);
            resizedImage.Save(model.realpath);
        }

        internal MemoryStream createWavNeg(string fileName, Bitmap bitmap)
        {
            _myWavCreator.NegativeImageMode = true;
            MemoryStream ms = _myWavCreator.createWavMemory(bitmap, fileName,false,false);
            return ms;
            
        }
    }
}
