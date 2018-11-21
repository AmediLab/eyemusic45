using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

/*
 * Interaction: this class is called by eyeMusic, to start timers for session and for each stimuli. 
 * In exam mode, called also for holding the trainees answer 
 * */
namespace eyeMusic45
{
    /*
     * This class delas with the statiscis of the session
     * */
    public class eyeMusicStatistics2
    {
        // variables
        private string _stimulusName = string.Empty; // File name without extention
        private int _totalScans = 0;  // Total number of scans

        public enum AnswerValue { NoAnswer, CorrectAnswer, IncorrectAnswer }; //now this is more complex
        private string _answerElements = string.Empty; // the values of the components of the trainees answer
        private Stopwatch _stimulusTimer = new Stopwatch(); // timer for each stimulus
        private Stopwatch _sessionTimer = new Stopwatch(); // timer for the hole training session

        private eyeMusic2 _myEyeMusic;
        private StreamWriter _logFile;

        // functions

        // get and set for private variables
        public string StimulusName { get { return _stimulusName; } set { _stimulusName = value; } }
        public int TotalScans { get { return _totalScans; } set { _totalScans = value; } }
        public string AnswerElements { get { return _answerElements; } set { _answerElements = value; } }
        public Stopwatch StimulusTimer { get { return _stimulusTimer; } set { _stimulusTimer = value; } }
        public Stopwatch SessionTimer { get { return _sessionTimer; } set { _sessionTimer = value; } }

        // constructor
        public eyeMusicStatistics2(eyeMusic2 myEyeMusic)
        {
            _myEyeMusic = myEyeMusic;
        }

        /// <summary>
        /// Total time in msec from startCollecting to stopCollecting 
        /// </summary>
        public long timeElapsed
        {
            get { return _stimulusTimer.ElapsedMilliseconds; }
        }
        /// <summary>
        ///  Total time in msec from first startTrainingTimer to last pauseTrainingTimer
        /// </summary>
        public long sessionTimeElapsed
        {
            get { return _sessionTimer.ElapsedMilliseconds; }
        }

        /// <summary>
        ///  starts the session timer
        /// </summary>
        public void startSessionTimer()
        {
            _sessionTimer.Start();
        }
        /// <summary>
        ///  pauses the timers
        /// </summary>
        public void pauseTimers()
        {
            _sessionTimer.Stop();
            _stimulusTimer.Stop();
        }
        /// <summary>
        ///  resumes the timers
        /// </summary>
        public void resumeTimers()
        {
            _sessionTimer.Start();
            _stimulusTimer.Start();
        }
        /// <summary>
        ///  resets the session timer
        /// </summary>
        public void resetSessionTimer()
        {
            _sessionTimer.Restart();
        }
        /// <summary>
        ///  starts the timer of a single stimuli
        /// </summary>
        public void startCollecting()
        {
            _stimulusTimer.Start();
        }
        /// <summary>
        ///  stops the timer of a single stimuli
        /// </summary>
        public void stopCollecting()
        {
            _stimulusTimer.Stop();
        }
        /// <summary>
        ///  resets statistics of one simulus, not whole training
        /// </summary>
        public void resetStatistics()
        {
            _stimulusTimer.Reset();
            _stimulusName = "";
            _totalScans = 0;
            _answerElements = "";
        }

        /// <summary>
        /// writes the statistics of the stimuli to the log file
        /// </summary>
        public void writeLogString()
        {
            if (_logFile != null)
            {
                _logFile.WriteLine(_stimulusName + " " + timeElapsed.ToString() + " " + (_totalScans).ToString() + " " + _answerElements);
            }
        }

        /// <summary>
        /// opens a log file
        /// </summary>
        /// <param name="typeText">type of subject input</param>
        /// <param name="traineeName">subject name</param>
        /// <param name="stageName">input folder</param>
        /// <param name="lessonName">sub input folder</param>
        public void openLogFile(string typeText, string traineeName, string stageName, string lessonName)
        {
            DateTime timeStamp = DateTime.Now;
            string timeStampString = String.Format("{0:dd.MM.yy-hh.mm.ss}", timeStamp);

            _logFile = (new FileInfo(_myEyeMusic.LogDirectory + traineeName + "-" + timeStampString + "-" + stageName + "-" + lessonName + "-" + typeText + ".txt")).CreateText();
        }

        /// <summary>
        /// writes end of training to log file + session statistics
        /// </summary>
        public void writeTrainingLogString()
        {
            if (_logFile != null)
            {
                _logFile.WriteLine("TTT " + sessionTimeElapsed.ToString());
            }
        }

        /// <summary>
        /// closes the log file
        /// </summary>
        public void closeLogFile()
        {
            if (_logFile != null)
            {
                _logFile.Close();
            }
        }

        /// <summary>
        /// writes end of exam to log file + session statistics
        /// </summary>
        public void writeExamLogString()
        {
            if (_logFile != null)
            {
                _logFile.WriteLine("EEE " + sessionTimeElapsed.ToString());
            }
        }
    }
}
