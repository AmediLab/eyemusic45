using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Business
{
    public class StepByStepClass
    {
        int index = 0;
        int lastsession = 0;
        int numCorrect = 0;

        static string[] titlesHEB;
        static string[] LessonHEB;

        static string[] titlesENG;
        static string[] LessonENG;

        static string[] titlesFRN;
        static string[] LessonFRN;

        static string[] titlesGRM;
        static string[] LessonGRM;

        static string[] titlesCST;
        static string[] LessonCST;

        static string[] images;
        static string[] types;
        static string[] corrects;

        string[] titels;
        List<string> PreLessonTitle;

        static List<string> PreLessonTitleENG;
        static List<string> PreLessonTitleHEB;
        static List<string> PreLessonTitleFRN;
        static List<string> PreLessonTitleGRM;
        static List<string> PreLessonTitleCST;

        static string[] PreLessonInt;

        List<string> SentLessonsTitle;
        List<int> SentLessonsint;


        //read all text s in four languges 
        public StepByStepClass(string map, string lan)
        {
            SentLessonsTitle = new List<string>();
            SentLessonsint = new List<int>();

            if (titlesENG == null)
            {
                titlesENG = System.IO.File.ReadAllLines(map + "/Texts/ENG/Steptitles.txt");
                titlesHEB = System.IO.File.ReadAllLines(map + "/Texts/HEB/Steptitles.txt");
                titlesFRN = System.IO.File.ReadAllLines(map + "/Texts/FRN/Steptitles.txt");
                titlesGRM = System.IO.File.ReadAllLines(map + "/Texts/GRM/Steptitles.txt");
                titlesCST = System.IO.File.ReadAllLines(map + "/Texts/CST/Steptitles.txt");

                LessonENG = System.IO.File.ReadAllLines(map + "/Texts/ENG/lesson.txt");
                LessonHEB = System.IO.File.ReadAllLines(map + "/Texts/HEB/lesson.txt");
                LessonGRM = System.IO.File.ReadAllLines(map + "/Texts/GRM/lesson.txt");
                LessonFRN = System.IO.File.ReadAllLines(map + "/Texts/FRN/lesson.txt");
                LessonCST = System.IO.File.ReadAllLines(map + "/Texts/CST/lesson.txt");

                images = System.IO.File.ReadAllLines(map + "/Texts/StepImages.txt");
                types = System.IO.File.ReadAllLines(map + "/Texts/StepTypes.txt");
                corrects = System.IO.File.ReadAllLines(map + "/Texts/StepCorrect.txt");
                PreLessonInt = System.IO.File.ReadAllLines(map + "/Texts/LessonInt.txt");

                PreLessonTitleENG = new List<string>();
                PreLessonTitleHEB = new List<string>();
                PreLessonTitleFRN = new List<string>();
                PreLessonTitleGRM = new List<string>();
                PreLessonTitleCST = new List<string>();

                for (int i = 0; i < PreLessonInt.Length; i++)
                {
                    PreLessonTitleENG.Add(LessonENG[i]);
                    PreLessonTitleHEB.Add(LessonHEB[i]);
                    PreLessonTitleFRN.Add(LessonFRN[i]);
                    PreLessonTitleGRM.Add(LessonGRM[i]);
                    PreLessonTitleCST.Add(LessonCST[i]);
                }

            }


            //found the correct text for selected languge
            if (lan == "h")
            {
                titels = titlesHEB;
                PreLessonTitle = PreLessonTitleHEB;
            }
            else if (lan == "e")
            {
                titels = titlesENG;
                PreLessonTitle = PreLessonTitleENG;
            }
            else if (lan == "f")
            {
                titels = titlesFRN;
                PreLessonTitle = PreLessonTitleFRN;
            }
            else if (lan == "c")
            {
                titels = titlesCST;
                PreLessonTitle = PreLessonTitleCST;
            }
            else
            {
                titels = titlesGRM;
                PreLessonTitle = PreLessonTitleGRM;
            }
        }

        // get index of the user
        public int getindex()
        {
            return index;
        }

        // move user to next index
        public void next()
        {
            if (titlesHEB.Length - 1 > index)
                index++;
        }

        public void prev()
        {
            index--;
        }

        //return the title in the user index
        public string TitlesCurrent()
        {
            return titels[index];
        }

        public string imagesCurrent()
        {
            return images[index];
        }

        public string typesCurrent()
        {
            return types[index];
        }

        public string typesNext()
        {
            if (types.Length - 2 > index)
                return types[index + 1];
            else
                return types[index];
        }

        //check if the user click on correct answer
        public bool ifcorrect(string answer)
        {
            if (corrects[index - 1] == answer)
                return true;
            else
                return false;
        }

        public void setindex(int ind)
        {
            index = ind;
        }

        public void changeToLast()
        {
            index = lastsession;
        }

        public void setLastSession()
        {
            lastsession = index;
        }

        public void addCorrect()
        {
            numCorrect++;
        }

        public int getCorrect()
        {
            return numCorrect;
        }

        public void resetNumCorrect()
        {
            numCorrect = 0;
        }

        public int returnType()
        {
            return Int32.Parse(types[index]);
        }

        public string typesPrev()
        {
            if (index > 0)
                return types[index - 1];
            else
                return types[index];
        }

        //create list of all lessons for return to prevois lessons
        public void SetLessons()
        {
            SentLessonsTitle = new List<string>();
            SentLessonsint = new List<int>();

            for (int i = 0; i < PreLessonInt.Length; i++)
            {
                if (Int32.Parse(PreLessonInt[i]) < index)
                {
                    SentLessonsint.Add(Int32.Parse(PreLessonInt[i]));
                    SentLessonsTitle.Add(PreLessonTitle[i]);
                }
            }
        }

        public int[] getLessonsInt()
        {
            return SentLessonsint.ToArray();
        }

        public string[] getLessonsTitles()
        {
            return SentLessonsTitle.ToArray();
        }


        internal void changeLan(string lan)
        {
            if (lan == "h")
            {
                titels = titlesHEB;
                PreLessonTitle = PreLessonTitleHEB;
            }
            else if (lan == "e")
            {
                titels = titlesENG;
                PreLessonTitle = PreLessonTitleENG;
            }
            else if (lan == "c")
            {
                titels = titlesCST;
                PreLessonTitle = PreLessonTitleCST;
            }
            else if (lan == "f")
                titels = titlesFRN;
            else
                titels = titlesGRM;
        }

        //found the number of questions that the user need to do for complate the test
        public int foundNumber()
        {
            foreach (string i in PreLessonInt)
            {
                if (index < Int32.Parse(i))
                {
                    return (Int32.Parse(i) - index) - 1;
                }
            }

            return 0;
        }

        public bool ifcorrectLast(string answer)
        {
            if (corrects[index] == answer)
                return true;
            else
                return false;
        }
    }
}