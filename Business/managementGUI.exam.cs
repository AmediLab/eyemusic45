using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace eyeMusic45
{
    public partial class managementGUI
    {
        /**
         * this struct holds stimuliExam info
         * */

        public string[] path_for_image;
        public string[] image_names;

        public struct StimuliExam
        {
            private string _stimuliName;
            private stimuliQuestion _question;
            
            public StimuliExam(string stimuliName, stimuliQuestion question)
            {
                _stimuliName = stimuliName;
                _question = question;
            }

            public string StimuliName { get { return _stimuliName; } set { _stimuliName = value; } }
            public stimuliQuestion Question { get { return _question; } set { _question = value; } }
        }

        /// <summary>
        /// struct for representing the question of the stimuli exam
        /// </summary>
        public class stimuliQuestion
        {
            private string _question, _correctAnswer, _fileName;
            private List<string> _answerOptions;
            private bool _isMultiple, _isStopTimer;

            private static readonly char[] DELIMITERS_FORMAT = { '<', '>' };

            public bool IsMulitiple { get { return _isMultiple; } }
            public List<string> AnswerOptions { get { return _answerOptions; } }
            public string QuestionText { get { return _question; } }
            public string FileName { get { return _fileName; } }
            public bool IsStop { get { return _isStopTimer; } }
            public string CorrectAnswer { get { return _correctAnswer; } }

            //constractor for defalut question
            public stimuliQuestion()
            {
                _fileName = "default";
                _isStopTimer = true;
                _isMultiple = true;
                _question = "default question: ";
                _answerOptions = new List<string>() { "right", "wrong" };
                _correctAnswer = "right";
            }

            public stimuliQuestion(string questionFormat)
            {
                string[] parse = questionFormat.Split(DELIMITERS_FORMAT, StringSplitOptions.RemoveEmptyEntries);
                _fileName = parse[0];
                _isStopTimer = parse[1].Equals("S"); //S = stop, C = continue
                _isMultiple = parse[2].Equals("M");  //M = Multiple, O = Open
                _question = parse[3];
                _answerOptions = new List<string>();
                if (_isMultiple)
                    for (int i = 4; i < parse.Length - 3; i++)
                        _answerOptions.Add(parse[i]);
                _correctAnswer = parse.Last();
            }
        }

        private int _examStimuliIndex = 0;
        private string _fullPath;
        private const string QUESTION_FILE = "format.txt";
        private const string EXAM_TITLE = "<Exam>"; //Exam = one question, <Stimuli> = question for each stimuli
        private const string EXAM_FOLDER = "Exam\\";
        private readonly  string[] PICTURE_FORMATS = {".png", ".bmp" };
        private StimuliExam _currentStimuli;
        private int _limitRandomExam = 10;

        public int ExamStimuliIndex { get { return _examStimuliIndex; } set { _examStimuliIndex = value; } }
        public string FullPath { get { return _fullPath; } set { _fullPath = value; } }
        public int Limit { get { return _limitRandomExam; } set { _limitRandomExam = value; } }

        public void create_exam_for_path(string addpath)
        {
            DirectoryInfo lessonFolder = new DirectoryInfo(_model.path_toLessen);
            FileInfo[] loadedFiles = lessonFolder.GetFiles();

            path_for_image = new string[loadedFiles.Count()];
            image_names = new string[loadedFiles.Count()];

            int i = 0;

            foreach (FileInfo item in loadedFiles)
            {
                path_for_image[i] = addpath + item.Name;
                image_names[i++] = Path.GetFileNameWithoutExtension(item.Name).Split('/').Last().Substring(3).Replace("_", " ");
            }

        }

    }
}
