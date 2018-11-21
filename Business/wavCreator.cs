using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using System.Web;
using System.Web.Mvc;
using EricOulashin;

namespace eyeMusic45
{
    public class wavCreator
    {
        private MediaPlayer _player;
        private bool _stopSession = false;
        private bool _stopOvalSession = false;
        private int _scanWidth = 50;
        private int _scanHeight;
        private bool _equalizeImage = false; // TBD: do equalizing process for image
        private bool _negativeImageMode = false;
        private bool _blackAndWhiteImageMode = false;
        private string _cueTypeFileName;
        private bool _paused = false;
        private bool _firstScreenSonifierInput = true; // for reume in virtual training
        private int _imageCounter = 0;

        // oval data types
        private MediaPlayer _ovalPlayer;
        private List<char[]> _ovalText; // holds the current text of OVAL
        private List<List<Uri>> _ovalWavFiles; // holds the wav files including thedesired space of the current text of OVAL
        private Dictionary<string, float[,]> _ovalMapping; // mappig of oval letters with a matching array 
        private Dictionary<string, Bitmap> _ovalMappingBitmap; // mapping of oval letter with matching bitmap for display purpose
        private int _wordIndex;
        private int _letterIndex;
        private int _letterSpace;
        private int _wordSpace;

        private bool _stopInsertOvalText = false; // a bool variable to signal to the running helper thread of oval to stop its processing
        private bool _openedOvalWavFile = false; // a bool variable to determine if _ovalPlayer called open and still didn't open
        private bool _closedWav = true; // a bool variable to determine if a file opened but still didn't close
        private bool _stratedPlayingOval = false; // this variable is used to determine if to play a wav file after changing the space between letters and words
        private Semaphore _ovalSemaphore; // used to insure that the 'open' call complited its run before closing a wav file in oval mode

        private Semaphore _semaphore; // used to insure that the 'open' call complited its run before closing a wav file

        public const float Epsilon = 0.00001f;
        public const float MaximumBrightness = 1f - Epsilon;
        public const float MinimumBrightness = Epsilon;

        // Clustering algorithm threshold
        public const float BlackBrightnessThreshold = 0.05f;//0.13
        public const float WhiteBrightnessThreshold = 0.90f;
        public const float SaturationThreshold = 0.13f;//0.15
        public const float GrayThreshold = 0.535f;

        public const int RedHueThreshold = 0;
        public const int YellowHueThreshold = 60;
        public const int GreenHueThreshold = 110;
        public const int BlueHueThreshold = 240;

        // parameters for creating wav file
        private string _wavFilePath = HighResPath;
        public const int FirstSample = 20480;
        public const int SampleRate = 44100;
        public const int TotalColors = 5;

        public const String OutputFilePath = "Out";
        //public const string HighResPath = "mp3high";
        public const string HighResPath = "WavHighRes";
        public const string LowResPath = "WavLowRes";
        public const int NumTotalFilesHighRes = 150;
        public const int NumTotalFilesLowRes = 120;
        public const int ScanWidthHighRes = 50;
        public const int ScanWidthLowRes = 40;
        public const int HighResWavLength = 10;
        public const int LowResWavLength = 5;

        static short[,] _samplesArray = null;
        static bool IAmIn = false;
        private int _numTotalFiles = 150;

        private eyeMusic2 _myEyeMusic;
        private eyeMusicStatistics2 _myStatisticsObj;

        public bool StopSession { get { return _stopSession; } set { _stopSession = value; } }
        public int ScanWidth { get { return _scanWidth; } set { _scanWidth = value; } }
        public int ScanHeight { get { return _scanHeight; } set { _scanHeight = value; } }
        public bool EqualizeImage { get { return _equalizeImage; } set { _equalizeImage = value; } }
        public bool NegativeImageMode { get { return _negativeImageMode; } set { _negativeImageMode = value; } }
        public bool BlackAndWhiteImageMode { get { return _blackAndWhiteImageMode; } set { _blackAndWhiteImageMode = value; } }
        public int LetterIndex { get { return _letterIndex; } set { _letterIndex = value; } }
        public int WordIndex { get { return _wordIndex; } set { _wordIndex = value; } }
        public int LetterSpace { get { return _letterSpace; } set { _letterSpace = value; } }
        public int WordSpace { get { return _wordSpace; } set { _wordSpace = value; } }
        public bool StopOvalSession { get { return _stopOvalSession; } set { _stopOvalSession = value; } }

        public string WavFilePath { get { return _wavFilePath; } set { _wavFilePath = value; } }
        public int NumTotalFiles { get { return _numTotalFiles; } set { _numTotalFiles = value; } }
        public string CueTypeFileName { get { return _cueTypeFileName; } set { _cueTypeFileName = value; } }
        public bool StopInsertOvalText { get { return _stopInsertOvalText; } set { _stopInsertOvalText = value; } }
        public bool StratedPlayingOval { get { return _stratedPlayingOval; } set { _stratedPlayingOval = value; } }
        public bool FirstScreenSonifierInput { get { return _firstScreenSonifierInput; } set { _firstScreenSonifierInput = value; } }
        public string MyFileMap;

        // constructor
        public wavCreator(eyeMusic2 myEyeMusic, eyeMusicStatistics2 statisticsObj, string fileMap)
        {
            MyFileMap = fileMap;
            _myEyeMusic = myEyeMusic;
            _myStatisticsObj = statisticsObj;
            _ovalMapping = new Dictionary<string, float[,]>();
            _ovalMappingBitmap = new Dictionary<string, Bitmap>();
            _ovalWavFiles = new List<List<Uri>>();
            _player = new MediaPlayer();
            _ovalPlayer = new MediaPlayer();
            _cueTypeFileName = "Sounds\\beep.wav";
            _scanHeight = _numTotalFiles / TotalColors;
            _ovalSemaphore = new System.Threading.Semaphore(1, 1); // same as _openSemaphore but for oval
            _semaphore = new System.Threading.Semaphore(1, 1);

            if (_samplesArray == null && !IAmIn)
            {
                IAmIn = true;
                allocateArray();
                loadSamples();
            }

        }

      
        /// <summary>
        /// reconstructs the color of the bitmap according to available colors
        ///  Important note: C# works in HLS space even though documentation says it works in HSB space. GetBrightness() actually gets Luminance.
        /// </summary>
        /// <param name="bitmapData">the bitmap to reconstruct</param>
        /// <returns>reconstructed bitmap</returns>
        public float[,] ReconstructColor(System.Drawing.Imaging.BitmapData bitmapData)
        {
            float[,] imArr = new float[_scanHeight, _scanWidth];
            byte[] theBmpPtr;
            System.Drawing.Color pixcol;

            System.Drawing.Imaging.BitmapData bmpData = bitmapData;
            //Main procedure for color matching for each pixel
            for (int i = 0; i < _scanHeight; i++)
            {
                for (int j = 0; j < _scanWidth; j++)
                {
                    theBmpPtr = GetPixel(j, i, bmpData);
                    pixcol = System.Drawing.Color.FromArgb((int)theBmpPtr[2], (int)theBmpPtr[1], (int)theBmpPtr[0]);

                    if ((pixcol.GetBrightness() > WhiteBrightnessThreshold) || grayscale(pixcol) == true)
                    {
                        imArr[i, j] = 1;
                        imArr[i, j] += pixcol.GetBrightness() - Epsilon; // -Epsilon to avoid overflow to 2

                        if (_negativeImageMode) // switches between black and white
                        {
                            imArr[i, j] = 1;
                            imArr[i, j] += (MaximumBrightness - pixcol.GetBrightness()) + 2 * Epsilon; // brightness sets the hight of the tone, in order to keep levels of white 
                            // when moving to black
                        }
                    }
                    else if (pixcol.GetBrightness() < BlackBrightnessThreshold)
                    {
                        //if pixel brightness is less than black brightness threshold,then pixel is black, no brightness for black    
                        imArr[i, j] = 0;

                        if (_negativeImageMode) // switches between black and white
                        {
                            imArr[i, j] = 1;
                            imArr[i, j] += MaximumBrightness;
                        }
                    }
                    else
                    {
                        imArr[i, j] = getColorCluster(pixcol); // sets color

                        if ((_blackAndWhiteImageMode) && (_negativeImageMode))
                        {
                            imArr[i, j] += (MaximumBrightness - pixcol.GetBrightness()) + 2 * Epsilon;
                        }
                        else // sets brightness
                        {
                            imArr[i, j] += Math.Min(pixcol.GetBrightness() * 2 - Epsilon, MaximumBrightness);
                        }
                    }
                }
            }
            return imArr;
        }

        /// <summary>
        ///  helper function for reconstructColor- gets value of specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bitmapData"></param>
        /// <returns></returns>
        public Byte[] GetPixel(int x, int y, System.Drawing.Imaging.BitmapData bitmapData)
        {
            int size = bitmapData.Stride * bitmapData.Height;

            /*Allocate buffer for image*/
            byte[] data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, size);

            if (x >= 0 && y >= 0 && x <= bitmapData.Width && y <= bitmapData.Height)
            {
                int sub_size = (y) * bitmapData.Stride + (x) * 4;
                byte[] data2 = new byte[size - sub_size];
                Array.Copy(data, sub_size, data2, 0, size - sub_size);
                return data2;
                //return (Byte*)bitmapData.Scan0.ToPointer() + (y) * bitmapData.Stride + (x) * 4;
            }

            return data;
            //return (Byte*)bitmapData.Scan0;
        }

        /// <summary>
        /// checks if pixol is grayscale
        /// </summary>
        /// <param name="pixol">the pixol to check</param>
        /// <returns>true- if grayscale, false- otherwise</returns>
        private bool grayscale(System.Drawing.Color pixol)
        {
            double mean, sd;
            mean = pixol.B + pixol.G + pixol.R;
            sd = Math.Sqrt((Math.Pow((mean - pixol.B), 2) + Math.Pow((mean - pixol.G), 2) + Math.Pow((mean - pixol.R), 2)) / 3);
            if (pixol.B < GrayThreshold * sd && pixol.G < GrayThreshold * sd && pixol.R < GrayThreshold * sd
                || pixol.GetSaturation() < SaturationThreshold && (pixol.GetBrightness() > BlackBrightnessThreshold))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// generates the clustred bitmap
        /// </summary>
        /// <param name="imArr"></param>
        /// <returns></returns>
        private Bitmap generatePostClassifictionBitmap(float[,] imArr)
        {
            int width = imArr.GetLength(1);
            int height = imArr.GetLength(0);
            Bitmap postClassifictionBitmap = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((imArr[i, j] < 1.0f)) // black
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.Black);
                    }
                    if ((imArr[i, j] >= 1.0f) && (imArr[i, j] < 2f)) // white
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.FromArgb((int)(255 * (imArr[i, j] - 1.0f)), (int)(255 * (imArr[i, j] - 1.0f)), (int)(255 * (imArr[i, j] - 1.0f))));
                    }
                    if ((imArr[i, j] >= 2.0f) && (imArr[i, j] < 3f)) // red
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.FromArgb((int)(255 * (imArr[i, j] - 2.0f)), 0, 0));
                    }
                    if ((imArr[i, j] >= 3.0f) && (imArr[i, j] < 4f)) // green
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.FromArgb(0, (int)(255 * (imArr[i, j] - 3.0f)), 0));
                    }
                    if ((imArr[i, j] >= 4.0f) && (imArr[i, j] < 5f)) // yellow
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.FromArgb((int)(255 * (imArr[i, j] - 4.0f)), (int)(255 * (imArr[i, j] - 4.0f)), 0));
                    }
                    if ((imArr[i, j] >= 5.0f) && (imArr[i, j] < 6f)) // blue
                    {
                        postClassifictionBitmap.SetPixel(j, i, System.Drawing.Color.FromArgb(0, 0, (int)(255 * (imArr[i, j] - 5.0f))));
                    }
                }
            }
            return postClassifictionBitmap;
        }

        /// <summary>
        /// returns the color that color.GetHue() is closest to (between red, green, yellow and blue)
        /// </summary>
        /// <param name="color"></param>
        /// <returns>closest color</returns>
        private int getColorCluster(System.Drawing.Color color)
        {
            int hue = (int)color.GetHue();

            // An array holding hue distances from the color thresholds
            int[] hueDistances = new int[4];
            int minIndex;
            int clusteredColorIndex;

            //Red    
            hueDistances[0] = Math.Min(Math.Abs(RedHueThreshold - (hue)), Math.Abs(360 - (Math.Abs(RedHueThreshold - (hue)))));
            //Green                         
            hueDistances[1] = Math.Min(Math.Abs(GreenHueThreshold - (hue)), Math.Abs(360 - (Math.Abs(GreenHueThreshold - (hue)))));
            //Yellow                    
            hueDistances[2] = Math.Min(Math.Abs(YellowHueThreshold - (hue)), Math.Abs(360 - (Math.Abs(YellowHueThreshold - (hue)))));
            //Blue                     
            hueDistances[3] = Math.Min(Math.Abs(BlueHueThreshold - (hue)), Math.Abs(360 - (Math.Abs(BlueHueThreshold - (hue)))));

            minIndex = findMinIndex(hueDistances);

            if (_blackAndWhiteImageMode)
            {
                clusteredColorIndex = 1; // white and black can be represented by 1 and the brightness is the influencing factor whether it will be white or black
            }
            else
            {
                clusteredColorIndex = minIndex + 2;
            }

            // Color indices 0,1,2,3,4,5 for Black,White,Red,Green,Yellow,Blue respectively
            return clusteredColorIndex;
        }

        /// <summary>
        /// returns the index of the minimum value
        /// </summary>
        /// <param name="array">array to compare values</param>
        /// <returns>index of min value</returns>
        private int findMinIndex(int[] array)
        {
            int index = 0;
            int value = 360;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < value)
                {
                    value = array[i];
                    index = i;
                }
            }
            return index;
        }

      
        /// <summary>
        /// creats a wav file of the given bitmap under the given name.
        /// </summary>
        /// <param name="bitmap">btimap to creat a wav file of</param>
        /// <param name="name"></param>
        public void createWav(Bitmap bitmap, string name, bool create_img)
        {
            _myEyeMusic.model.bmpName = name;
            bitmap = managementGUI.resizeImage(bitmap, _scanWidth, _scanHeight, _myEyeMusic.ResizeMode);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            float[,] imArr = ReconstructColor(bmpData);
            bitmap.UnlockBits(bmpData);

            if (create_img)
            {
                Bitmap clusteredImage = generatePostClassifictionBitmap(imArr);
                // display image
                _myEyeMusic.setScanPictureBoxes(clusteredImage, name);
            }

            WAVFile sonifiedWAV = new WAVFile();
            sonifiedWAV.Create(_myEyeMusic.OutDirectory + "\\" + name + ".wav", false, 44100, 16);
            double volume = _myEyeMusic.MyVolume;
            if (_myEyeMusic.MyCueType.Equals(eyeMusic2.CueType.NoCue))
            {
                volume = 0.0;
            }

            short sample;
            WAVFile cueWAVFile = new WAVFile();             // adding cue
            cueWAVFile.Open(_cueTypeFileName, WAVFile.WAVFileMode.READ);
            for (int j = 0; j < cueWAVFile.NumSamples; j++)
            {
                sample = (short)(volume * cueWAVFile.GetNextSample_16bit());
                sample = Math.Min(short.MaxValue, sample);
                sonifiedWAV.AddSample_16bit(sample);
            }
            cueWAVFile.Close();
            for (int i = 0; i < _scanWidth; i++)
            {
                createMixer(i, sonifiedWAV, imArr);
            }
            if (sonifiedWAV != null)
            {
                sonifiedWAV.Close();
            }
        }

        public MemoryStream createWavMemory(Bitmap bitmap, string name, bool create_img, bool with_cue)
        {
            bitmap = managementGUI.resizeImage(bitmap, _scanWidth, _scanHeight, _myEyeMusic.ResizeMode);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            float[,] imArr = ReconstructColor(bmpData);
            bitmap.UnlockBits(bmpData);

            if (create_img)
            {
                Bitmap clusteredImage = generatePostClassifictionBitmap(imArr);
                // display image
                _myEyeMusic.setScanPictureBoxes(clusteredImage);
            }

            MemoryStreamEM sonifiedWAV = new MemoryStreamEM();
            MemoryStream toREt = sonifiedWAV.Create(_myEyeMusic.OutDirectory + "\\" + name + ".wav", false, 44100, 16);
            double volume = _myEyeMusic.MyVolume;
            if (_myEyeMusic.MyCueType.Equals(eyeMusic2.CueType.NoCue))
            {
                volume = 0.0;
            }

            if (with_cue)
            {
                short sample;
                WAVFile cueWAVFile = new WAVFile();             // adding cue
                cueWAVFile.Open(_myEyeMusic._mapFile + "\\Sounds\\" + _myEyeMusic.model.beep_noise, WAVFile.WAVFileMode.READ);
                for (int j = 0; j < cueWAVFile.NumSamples; j++)
                {
                    sample = (short)(volume * cueWAVFile.GetNextSample_16bit());
                    sample = Math.Min(short.MaxValue, sample);
                    sonifiedWAV.AddSample_16bit(sample);
                }
                cueWAVFile.Close();
            }

            for (int i = 0; i < _scanWidth; i++)
            {
                createMixer(i, sonifiedWAV, imArr);
            }

            sonifiedWAV.addLength();

            toREt = sonifiedWAV.mFileStream;


            return toREt;
        }

       
        // Declare external functions - for active window
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner  
            public int Top;         // y position of upper-left corner  
            public int Right;       // x position of lower-right corner  
            public int Bottom;      // y position of lower-right corner  
        }

        /// <summary>
        /// gets position of _plyer for scanTrackBar of eyeMusic
        /// </summary>
        /// <returns></returns>
        public int getPlayerValue()
        {
            return _player.Position.Milliseconds + _player.Position.Seconds * 1000;
        }

        //methods for creating wav file

        /// <summary>
        /// allocates an array of according to the number of files in WavFilePath
        /// </summary>
        private void allocateArray()
        {
            int cols, rows;

            string[] fileNames = Directory.GetFiles(MyFileMap + "\\" + _wavFilePath);
            rows = fileNames.Length;

            if (rows != _numTotalFiles)
            {
                throw new System.ArgumentException("total number of files must be equal to the number of files in wav folder");
            }

            // Set the number of cols according to the number samples in the first WAV file
            //WAVFileMP3 tempWAVFile = new WAVFileMP3();
            WAVFile tempWAVFile = new WAVFile();
            tempWAVFile.Open(fileNames[0], WAVFile.WAVFileMode.READ);
            cols = tempWAVFile.NumSamples;
            tempWAVFile.Close();

            // Allocate the array
            _samplesArray = new short[rows, cols];
        }

        /// <summary>
        /// loads the samples from WavFilePath to _samplesArray
        /// </summary>
        private void loadSamples()
        {
            //WaitForm waitForm = new WaitForm();
            //ProgressBar pb = new ProgressBar();
            //pb.Minimum = 0;

            //WAVFileMP3 currentWAVFile;
            WAVFile currentWAVFile;

            string[] fileNames = Directory.GetFiles(MyFileMap + "\\" + _wavFilePath);
            currentWAVFile = new WAVFile();
            currentWAVFile.Open(fileNames[0], WAVFile.WAVFileMode.READ);
            //pb.Maximum = fileNames.Length * currentWAVFile.NumSamples;
            int seconds = (currentWAVFile.NumSamples - FirstSample);
            currentWAVFile.Close();
            //pb.Value = 0;
            //pb.Location = new Point(40, 80);

            //waitForm.Show();
            //waitForm.Controls.Add(pb);

            Application.DoEvents();
            for (int i = 0; i < fileNames.Length; i++)
            {
                // Create the WAVFile object and open the current sample WAV file.
                currentWAVFile = new WAVFile();
                currentWAVFile.Open(fileNames[i], WAVFile.WAVFileMode.READ);

                // Get the sample from the file and copy it to our array
                for (int j = 0; j < currentWAVFile.NumSamples; j++)
                {
                    _samplesArray[i, j] = currentWAVFile.GetNextSample_16bit();
                }
                //pb.Value += currentWAVFile.NumSamples;
                currentWAVFile.Close();     // Close the WAV file 
            }
            //waitForm.Close();
        }

        /// <summary>
        /// creats a mixer samlple from the given column index
        /// </summary>
        /// <param name="column">column number</param>
        /// <param name="sonifiedWAV"></param>
        /// <param name="imArr">the array to create from it a wav file</param>
        private void createMixer(int column, WAVFile sonifiedWAV, float[,] imArr)
        {
            int arrNotesLength = imArr.GetLength(0);
            float[] paramArray = new float[_scanHeight];

            for (int i = 0; i < arrNotesLength; i++)
            {
                paramArray[i] = imArr[i, column];
            }
            generateColumn(paramArray, column, _myEyeMusic.model.ScanSpeed, sonifiedWAV);
        }

        private void createMixer(int column, MemoryStreamEM sonifiedWAV, float[,] imArr)
        {
            int arrNotesLength = imArr.GetLength(0);
            float[] paramArray = new float[_scanHeight];

            for (int i = 0; i < arrNotesLength; i++)
            {
                paramArray[i] = imArr[i, column];
            }
            generateColumn(paramArray, column, _myEyeMusic.model.ScanSpeed, sonifiedWAV);
        }

        /// <summary>
        /// creates the sample of the desired column
        /// </summary>
        /// <param name="parameterVector">values in column</param>
        /// <param name="column">column number</param>
        /// <param name="duration">current duration</param>
        /// <param name="sonifiedWAV">the wav file to add a sample to</param>
        public void generateColumn(float[] parameterVector, int column, int duration, WAVFile sonifiedWAV)
        {
            int samplesToExtract = (int)(duration * ((double)SampleRate / 1000.0));
            int startSample = FirstSample + column * samplesToExtract;
            int colorNumber = 0;
            double attenuation = 1.0;
            int samplesPerColumn = 0;
            int sampleSum = 0;

            for (int j = 0; j < samplesToExtract; j++)
            {
                sampleSum = 0;
                samplesPerColumn = 0;
                for (int i = 0; i < parameterVector.Length; i++)
                {
                    // Get color number and color attenuation
                    colorNumber = (int)(Math.Floor(Math.Abs(parameterVector[i])));
                    if (colorNumber >= 5)
                    {
                        colorNumber = 5;
                    }

                    if (colorNumber != 0) // color isn't black
                    {
                        attenuation = ((parameterVector[i]) - colorNumber); // the height of the tone
                        samplesPerColumn++;
                        sampleSum += (int)(attenuation * _samplesArray[calculateWaveIndex(colorNumber, i), j + startSample]);
                    }
                }

                sampleSum = (int)((double)sampleSum / (double)Math.Sqrt(samplesPerColumn));
                sampleSum = Math.Min(short.MaxValue, sampleSum);
                sonifiedWAV.AddSample_16bit((short)sampleSum); // Clipping may occur here
            }
        }

        public void generateColumn(float[] parameterVector, int column, int duration, MemoryStreamEM sonifiedWAV)
        {
            int samplesToExtract = (int)(duration * ((double)SampleRate / 1000.0));
            int startSample = FirstSample + column * samplesToExtract;
            int colorNumber = 0;
            double attenuation = 1.0;
            int samplesPerColumn = 0;
            int sampleSum = 0;

            for (int j = 0; j < samplesToExtract; j++)
            {
                sampleSum = 0;
                samplesPerColumn = 0;
                for (int i = 0; i < parameterVector.Length; i++)
                {
                    // Get color number and color attenuation
                    colorNumber = (int)(Math.Floor(Math.Abs(parameterVector[i])));
                    if (colorNumber >= 5)
                    {
                        colorNumber = 5;
                    }

                    if (colorNumber != 0) // color isn't black
                    {
                        attenuation = ((parameterVector[i]) - colorNumber); // the height of the tone
                        samplesPerColumn++;
                        sampleSum += (int)(attenuation * _samplesArray[calculateWaveIndex(colorNumber, i), j + startSample]);
                    }
                }

                sampleSum = (int)((double)sampleSum / (double)Math.Sqrt(samplesPerColumn));
                sampleSum = Math.Min(short.MaxValue, sampleSum);
                sonifiedWAV.AddSample_16bit((short)sampleSum); // Clipping may occur here
            }
        }

        /// <summary>
        /// retrives the currect row index according to color and row in column
        /// </summary>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private int calculateWaveIndex(int color, int row)
        {
            int wavesPerColumn = _numTotalFiles / TotalColors;
            return (wavesPerColumn * (color - 1) + (wavesPerColumn - row - 1));
        }

        /// <summary>
        /// inlarges the given array to a array of size _scanHeight * _scanWidth for display purpose
        /// </summary>
        /// <param name="ovalMapping">the array to display in center</param>
        /// <returns>array for display</returns>
        private float[,] inlargeArr(float[,] ovalMapping)
        {
            float[,] ovalArr = new float[_scanHeight, _scanWidth];
            int center = (_scanWidth / 2) - (ovalMapping.GetLength(1) / 2);
            for (int i = 0; i < ovalMapping.GetLength(0); i++) // rows
            {
                for (int j = 0; j < center; j++)
                {
                    ovalArr[i, j] = 0.0f;
                }
                for (int j = center; j < center + ovalMapping.GetLength(1); j++) // copy columns from ovalMapping 
                {
                    ovalArr[i, j] = ovalMapping[i, j - center];
                }
                for (int j = center + ovalMapping.GetLength(1); j < _scanWidth; j++)
                {
                    ovalArr[i, j] = 0.0f;
                }
            }
            return ovalArr;
        }

    }
}
