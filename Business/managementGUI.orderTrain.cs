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
        private Dictionary<String, List<FileInfo>> _lessonTraningFileListNew;

        public Dictionary<string, List<FileInfo>> LessonTrainingFileListNew { get { return _lessonTraningFileListNew; } set { _lessonTraningFileListNew = value; } }

        /// <summary>
        /// sets training panel with the desired stimuli
        /// </summary>
        /// <param name="fullPath">the path of the desired stimuli</param>
        public string[] prepareTrainingGalleryNew(string add_path)
        {
            _model.path_toLessen = _model.path + TRAINING_NEW_FOLDER_PATH + add_path.Replace('/', '\\');
            return buildGalleryNew(TRAINING_NEW_FOLDER_PATH.Replace("\\", "/") + add_path + "/");
        }

        /// <summary>
        /// builds training gallery
        /// </summary>
        public string[] buildGalleryNew(string Addpath)
        {
            DirectoryInfo lessonFolder = new DirectoryInfo(_model.path_toLessen);
            FileInfo[] loadedFiles = lessonFolder.GetFiles();

            int num_rows = (loadedFiles.Count() / 6);

            if (loadedFiles.Count() % 6 != 0)
                num_rows++;

            List<string> onlyNames = new List<string>();

            string[] all = new string[num_rows];
            int j = 0;
            int i = 0;
            string allfiles = "";

            foreach (FileInfo item in loadedFiles)
            {
                if (j++ < 5)
                {
                    onlyNames.Add(item.Name);
                    allfiles += Addpath + item.Name;
                    allfiles += ",";
                }
                else
                {
                    j = 0;
                    onlyNames.Add(item.Name);
                    allfiles += Addpath + item.Name;
                    all[i++] = allfiles;
                    allfiles = "";
                }
            }

            if (loadedFiles.Count() < 6)
            {
                all[0] = allfiles.Substring(0, allfiles.Length - 1);
            }
            else if (loadedFiles.Count() % 6 != 0)
            {
                all[i] = allfiles.Substring(0, allfiles.Length - 1); ;
            }

            _model.onlynames = onlyNames.ToArray();

            return all;
        }


       
        /// <summary>
        /// builds a training tree hirarchy of the available lessons
        /// </summary>
        /// <param name="path">the folder to take the lessons from</param>
        public string buildTrainingTreeNew(string path)
        {
            DirectoryInfo trainingFolder = new DirectoryInfo(_model.path + path);
            DirectoryInfo[] stageFolders = trainingFolder.GetDirectories();

            string zmny = "";

            for (int i = 0; i < stageFolders.Length; i++)
            {
                DirectoryInfo[] lessonFolders = stageFolders[i].GetDirectories();

                string stageNodeArr = stageFolders[i].Name;

                for (int j = 0; j < lessonFolders.Length; j++)
                {
                    stageNodeArr += "," + lessonFolders[j].Name;
                }
                zmny += stageNodeArr;

                if (i + 1 < stageFolders.Length)
                {
                    zmny += ";";
                }

            }

            return zmny;
        }
    }
}
