using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Business
{
    public class ExpStepClass
    {
        int index = 0;
        int lastsession = 0;
        int numCorrect = 0;
        int maxIGo = 0;
        int FINISHEXAM = 379;
        int START_LEARN = 21;
        int numImagePass = 0;

        static string[] titlesHEBStep;
        static string[] LessonHEBStep;

        static string[] imagesStep;
        static string[] typesStep;
        static string[] correctsStep;

        static string[] A;
        static string[] B;
        static string[] C;
        static string[] D;

        string[] titels;
        List<string> PreLessonTitle;

        static List<string> PreLessonTitleHEBStep;

        static string[] PreLessonIntStep;

        List<string> SentLessonsTitle;
        List<int> SentLessonsint;

        //static string[] LessonList;
        //static int[] ListInt;

        public ExpStepClass(string map, string lan)
        {
            SentLessonsTitle = new List<string>();
            SentLessonsint = new List<int>();

            if (titlesHEBStep == null)
            {
                titlesHEBStep = System.IO.File.ReadAllLines(map + "/ExpTexts/HEB/Steptitles.txt");
                LessonHEBStep = System.IO.File.ReadAllLines(map + "/ExpTexts/HEB/lesson.txt");
                imagesStep = System.IO.File.ReadAllLines(map + "/ExpTexts/StepImages.txt");
                typesStep = System.IO.File.ReadAllLines(map + "/ExpTexts/StepTypes.txt");
                correctsStep = System.IO.File.ReadAllLines(map + "/ExpTexts/StepCorrect.txt");
                PreLessonIntStep = System.IO.File.ReadAllLines(map + "/ExpTexts/LessonInt.txt");
                
                A = System.IO.File.ReadAllLines(map + "/ExpTexts/Answer_A.txt");
                B = System.IO.File.ReadAllLines(map + "/ExpTexts/Answer_B.txt");
                C = System.IO.File.ReadAllLines(map + "/ExpTexts/Answer_C.txt");
                D = System.IO.File.ReadAllLines(map + "/ExpTexts/Answer_D.txt");

                PreLessonTitleHEBStep = new List<string>();

                //PreLessonInt = new List<int>();

                for (int i = 0; i < PreLessonIntStep.Length; i++)
                {
                    PreLessonTitleHEBStep.Add(LessonHEBStep[i]);
                }

                //LessonList = PreLessonTitle.ToArray();
                //ListInt = PreLessonLocation.ToArray();
            }


            titels = titlesHEBStep;
            PreLessonTitle = PreLessonTitleHEBStep;
            
        }

        public int retPassImg()
        {
            return numImagePass;
        }

        public void gotoFinish()
        {
            index = FINISHEXAM;
        }

        public int returnNumImage()
        {
            int prev = 0;
            for (int i = 0; i < PreLessonIntStep.Length; i++)
            {
                if (Int32.Parse(PreLessonIntStep[i]) > index)
                {
                    //the final exam with 30 questions
                    /*if (i + 1 < PreLessonIntStep.Length)
                        return (Int32.Parse(PreLessonIntStep[i]) - prev) - 30;
                    //all the other exams 10 questions
                    else */
                    numImagePass = index - prev + 1;
                    return  (Int32.Parse(PreLessonIntStep[i]) - prev) - 12;
                }

                prev = Int32.Parse(PreLessonIntStep[i]);
            }

            //at finish lesson
            numImagePass = index - prev + 1;
            return 67;
        }

        public void gotoStart()
        {
            index = START_LEARN;
        }

        public int getindex()
        {
            return index;
        }

        public void next()
        {
            if (titlesHEBStep.Length - 1 > index)
                index++;
        }

        public void prev()
        {
            index--;
        }

        public string TitlesCurrent()
        {
            return titels[index];
        }

        public string imagesCurrent()
        {
            return imagesStep[index];
        }

        public string imagesNext()
        {
            return imagesStep[index + 1];
        }
        public string typesCurrent()
        {
            return typesStep[index];
        }

        public string typesNext()
        {
            if (typesStep.Length - 2 > index)
                return typesStep[index + 1];
            else
                return typesStep[index];
        }

        public bool ifcorrect(string answer)
        {
            if (correctsStep[index - 1] == answer)
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
            return Int32.Parse(typesStep[index]);
        }

        public string typesPrev()
        {
            if (index > 0)
                return typesStep[index - 1];
            else
                return typesStep[index];
        }

        public void SetLessons()
        {
            SentLessonsTitle = new List<string>();
            SentLessonsint = new List<int>();

            for (int i = 0; i < PreLessonIntStep.Length; i++)
            {
                if (Int32.Parse(PreLessonIntStep[i]) < index)
                {
                    SentLessonsint.Add(Int32.Parse(PreLessonIntStep[i]));
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
            titels = titlesHEBStep;
            PreLessonTitle = PreLessonTitleHEBStep;
        }

        public int foundNumber()
        {
            foreach (string i in PreLessonIntStep)
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
            if (correctsStep[index] == answer)
                return true;
            else
                return false;
        }

        internal string[] FourAnswers()
        {
            string [] TheAnswers = new string[4];
            TheAnswers[0] = A[index];
            TheAnswers[1] = B[index];
            TheAnswers[2] = C[index];
            TheAnswers[3] = D[index];

            return (TheAnswers);
        }
    }
}