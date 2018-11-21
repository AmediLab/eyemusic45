using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using eyemusic45.Models.ViewModels;

/**
 * this class is a Intermediate layer class between the eyeMusic and the wavCreator class
 * */
namespace eyeMusic45
{
    public partial class managementGUI
    {
        public const String TRAINING_FOLDER_PATH = "\\Training\\";
        public const String TRAINING_NEW_FOLDER_PATH = "\\TrainInOrder\\";
        public const String EXAM_FOLDER_PATH = "\\Exam\\";
        public string EM_DIRECTORY = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\EM";
        public string OUT_DIRECTORY = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\EM" + "\\Out\\";
        public const String FORMT_FILE_NAME = "formatFile";

        private eyeMusic2 _myEyeMusic;
        private wavCreator _myWavCreator;
        private eyeMusicStatistics2 _myStatisticsObj;
        private eyeMusicModel _model;
        private string  _myGUIPath;

        // constructor
        public managementGUI(eyeMusic2 myEyeMusic, wavCreator myWavCreator, eyeMusicStatistics2 statisticsObj,eyeMusicModel model)
        {
            _myEyeMusic = myEyeMusic;
            _myWavCreator = myWavCreator;
            _myStatisticsObj = statisticsObj;
            _model = model;
        }

        
        /// <summary>
        /// this method crops the given image according to the given dimensions
        /// </summary>
        /// <param name="imgToCrop">the image to crop</param>
        /// <param name="left">amount to crop from left</param>
        /// <param name="right">amount to crop from right</param>
        /// <param name="up">amount to crop from top</param>
        /// <param name="down">amount to crop from bottom</param>
        /// <returns>a croped bitmap</returns>
        public Bitmap crop(Bitmap imgToCrop, int left, int right, int up, int down)
        {
            int width = imgToCrop.Width - right - left;
            int height = imgToCrop.Height - down - up;

            Bitmap cropBitmap = new Bitmap(_myWavCreator.ScanWidth * 5, _myWavCreator.ScanHeight * 5);
            Rectangle cloneRect = new Rectangle(left, up, width, height);

            System.Drawing.Imaging.PixelFormat format = cropBitmap.PixelFormat;
            Bitmap cloneBitmap = imgToCrop.Clone(cloneRect, format);

            return resizeImage(cloneBitmap, _myWavCreator.ScanWidth, _myWavCreator.ScanHeight, _myEyeMusic.ResizeMode);
        }

        /// <summary>
        /// this method fixes the size of the image
        /// </summary>
        /// <param name="imageToResize">the image to resize</param>
        /// <param name="newWidth">the desired witdh</param>
        /// <param name="newHeight">the desired height</param>
        /// <param name="interpolationMode">the interpolation to use</param>
        /// <returns>a resized bitmap</returns>
        public static Bitmap resizeImage(Bitmap imageToResize, int newWidth, int newHeight, InterpolationMode interpolationMode)
        {
            Bitmap b = new Bitmap(newWidth, newHeight);

            Graphics g = Graphics.FromImage((Image)b);
            setResizeProperties(g, interpolationMode);

            g.DrawImage(imageToResize, 0, 0, newWidth, newHeight);

            g.Dispose();

            return b;
        }

        /// <summary>
        /// Resizes the image to the given size.
        /// If keepRatio = true, keeps the original image ratio, and paints surrounding area
        /// in black
        /// </summary>
        /// <param name="imageToResize">the image to resize</param>
        /// <param name="newWidth">new width</param>
        /// <param name="newHeight">new height</param>
        /// <param name="interpolationMode">interpolation mode</param>
        /// <param name="keepRatio">if to keep ratio of original image</param>
        /// <returns>Returns the resized image</returns>
        public static Bitmap resizeImage(Bitmap imageToResize, int newWidth, int newHeight, InterpolationMode interpolationMode, bool keepRatio)
        {

            if (!keepRatio)
            {
                return resizeImage(imageToResize, newWidth, newHeight, interpolationMode);
            }            
            else
            {
                // Get the correct ratio by height and width ratios
                double ratio = Math.Max((double)imageToResize.Size.Width / newWidth, (double)imageToResize.Size.Height / newHeight);
                int width = (int)Math.Floor(imageToResize.Size.Width / ratio);
                int height = (int)Math.Floor(imageToResize.Size.Height / ratio);

                Bitmap b = new Bitmap(newWidth, newHeight);

                using (Graphics g = Graphics.FromImage((Image)b))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    // Fill image with black color
                    g.FillRectangle(brush, 0, 0, newWidth, newHeight);

                    setResizeProperties(g, interpolationMode);

                    int startX = (int)Math.Floor((double)(newWidth - width) / 2);
                    int startY = (int)Math.Floor((double)(newHeight - height) / 2);

                    g.DrawImage(imageToResize, startX, startY, width, height);
                }

                return b;
            }
        }

        private static void setResizeProperties(Graphics g, InterpolationMode interpolationMode)
        {
            g.InterpolationMode = interpolationMode;
            if (interpolationMode != InterpolationMode.NearestNeighbor)
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
        }
        

        /// <summary>
        /// Is called after user selecting input type by TraineeName.cs  
        /// creatsa background thread(TODO: make sure that this is what I want) of wavCreator listining method. This method takes the bitmap from the top of the queue, 
        /// creats a wav file, plays it. when finished playing continues to the next stimuli.
        /// Chooses action according to _myInputType:
        /// training - displays training panel + builds training tree 
        ///  Exam - displays exam panel + builds exam tree
        /// Camera - starts camera playing 'RunCameraSession()'
        /// screenSonifier - starts screenSonifier playing 'runscreenSonifier()'
        /// ActiveWindow - starts Active window playing 'runActiveWindow()'
        /// Word - displays word panel
        /// Oval - displays oval panel
        /// </summary>
        public void startSession()
        {
            _myWavCreator.StopOvalSession = true;
            _myWavCreator.StopSession = false;

            OperatingSystem os = Environment.OSVersion; //Get Operating system information.
            Version vs = os.Version; //Get version information about the os.

            eyeMusic2.InputType inputType = _myEyeMusic.MyInputType;
            switch (inputType)
            {
                case eyeMusic2.InputType.Training:
                    buildTrainingTree(TRAINING_FOLDER_PATH);
                    _myGUIPath = TRAINING_FOLDER_PATH;
                    buildTrainingTreeNew(TRAINING_NEW_FOLDER_PATH);
                    _myGUIPath = TRAINING_FOLDER_PATH;
                    break;
                case eyeMusic2.InputType.Exam:
                    _myGUIPath = TRAINING_FOLDER_PATH;
                    break;
            default:
                    break;
            }
        }
    }
}
