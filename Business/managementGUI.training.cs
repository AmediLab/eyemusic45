using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace eyeMusic45
{
    public partial class managementGUI
    {
        public const int FILE_PREFIX = 1;
        public const int TAG_TEXT = 0;

        private Dictionary<String, List<FileInfo>> _lessonTraningFileList;
        private FileInfo _formatFile;

        public Dictionary<string, List<FileInfo>> LessonTrainingFileList { get { return _lessonTraningFileList; } set { _lessonTraningFileList = value; } }

        /// <summary>
        /// sets training panel with the desired stimuli
        /// </summary>
        /// <param name="fullPath">the path of the desired stimuli</param>
        public string[] prepareTrainingGallery(string add_path)
        {
            splitFiles(_model.path  + _myGUIPath +  add_path);
            _model.path_toLessen = _model.path + _myGUIPath + add_path.Replace('/','\\');
            return buildGallery(_myGUIPath.Replace("\\", "/") + add_path + "/");
        }

        /// <summary>
        /// builds training gallery
        /// </summary>
        public string[] buildGallery(string Addpath)
        {
            List<string> onlyNames = new List<string>();

            string[] all = new string[_lessonTraningFileList.Count];
            int i = 0;
            foreach (KeyValuePair<String, List<FileInfo>> item in _lessonTraningFileList)
            {
                string allfiles = "";
       
                for (int j = 0; j < item.Value.Count; j++)
                {
                    onlyNames.Add(item.Value[j].Name);
                    allfiles += Addpath + item.Value[j].Name;

                    if (j + 1 < item.Value.Count)
                    {
                        allfiles += ",";
                    }
                }
                all[i] = allfiles;
                i++;
            }

            _model.onlynames = onlyNames.ToArray();

            return all;

        }

        
        /// <summary>
        /// sorts images in fullPath according to desired tags. 
        /// the sorted files are kept in in _lessonTraningFileList
        /// </summary>
        /// <param name="fullPath"> the path to the deired lesson</param>
        private void splitFiles(string fullPath)
        {
            DirectoryInfo lessonFolder = new DirectoryInfo(fullPath);
            FileInfo[] loadedFiles;

            _lessonTraningFileList = new Dictionary<string, List<FileInfo>>();
            FileInfo[] formatFileList = lessonFolder.GetFiles(managementGUI.FORMT_FILE_NAME + ".txt");
            if (formatFileList.Length > 0)
            {
                _formatFile = formatFileList[0];

                StreamReader myReader = _formatFile.OpenText(); // open file
                // read line by line and add files to dictionary
                string content = "";
                string[] words;
                string key;
                List<FileInfo> fileList;
                while ((content = myReader.ReadLine()) != null)
                {
                    fileList = new List<FileInfo>();
                    words = content.Split('#');
                    loadedFiles = lessonFolder.GetFiles(words[FILE_PREFIX] + "*.bmp");
                    fileList.AddRange(loadedFiles);
                    loadedFiles = lessonFolder.GetFiles(words[FILE_PREFIX] + "*.png");
                    fileList.AddRange(loadedFiles);
                    key = words[TAG_TEXT];
                    _lessonTraningFileList.Add(key, fileList);
                }
            }
            if (fullPath != null)
            {
                if (fullPath.Contains("\\"))
                {
                    _myEyeMusic.SelectedStage = fullPath.Substring(0, fullPath.IndexOf("\\"));
                }
            }
            _myEyeMusic.SelectedLesson = lessonFolder.Name;
        }

        /// <summary>
        /// builds a training tree hirarchy of the available lessons
        /// </summary>
        /// <param name="path">the folder to take the lessons from</param>
        public string buildTrainingTree(string path)
        {
            DirectoryInfo trainingFolder = new DirectoryInfo(_model.path + path);
            DirectoryInfo[] stageFolders = trainingFolder.GetDirectories();

            _model.UItrainingStimuliTreeView = "";

            for (int i = 0; i < stageFolders.Length; i++)
            {
                DirectoryInfo[] lessonFolders = stageFolders[i].GetDirectories();

                string stageNodeArr = stageFolders[i].Name;

                for (int j = 0; j < lessonFolders.Length; j++)
                {
                    stageNodeArr +=  "," + lessonFolders[j].Name;
                }
                _model.UItrainingStimuliTreeView += stageNodeArr;

                if (i + 1 < stageFolders.Length)
                {
                    _model.UItrainingStimuliTreeView += ";";
                }

            }

            return _model.UItrainingStimuliTreeView;
        }
    }
}
