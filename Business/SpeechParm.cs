using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Business
{
    public class SpeechParm
    {
        //Arrayl texts = new Array()<string>; 

        public float Y;
        public string Text;

        /// <summary>
        /// One word to speech 
        /// </summary>
        /// <param name="y">the herz</param>
        /// <param name="textToSpeech">the text to speach</param>
        /// <param name="len">the languge</param>
        public SpeechParm(float y, string textToSpeech, string len)
        {
            Y = y;
            if (len == "h")
                Text = ToHebrew(textToSpeech);
            else
                Text = textToSpeech;
        }

        /// <summary>
        /// Build the webrew speech in Polish sentencess  
        /// </summary>
        /// <param name="textToSpeech"></param>
        /// <returns></returns>
        private string ToHebrew(string textToSpeech)
        {
            string HebrewBuild = "";
            double newFloat;
            int newInt;

            //this is number
            if (double.TryParse(textToSpeech, out newFloat))
            {
                if (newFloat == Math.Floor(newFloat))
                {
                    newInt = Convert.ToInt32(newFloat);
                    HebrewBuild = readInt(newInt);
                }
                else
                {
                    HebrewBuild = readDouble(newFloat);
                }
            }
            //this is letters
            else
            {
                HebrewBuild = readletters(textToSpeech);
            }

            return HebrewBuild;
        }

        private string readletters(string textToSpeech)
        {
            return textToSpeech;
        }

        /// <summary>
        /// Add the letter "nekoda" in middlae of duble
        /// </summary>
        /// <param name="newFloat"></param>
        /// <returns></returns>
        private string readDouble(double newFloat)
        {
            string before = readInt(Convert.ToInt32(Math.Floor(newFloat)));

            before += " nekoda ";

            before += readInt(Int32.Parse(newFloat.ToString().Split('.')[1]));

            return before;
        }

        /// <summary>
        /// Build hebrew sentences of number 
        /// </summary>
        /// <param name="newInt"></param>
        /// <returns></returns>
        private string readInt(int newInt)
        {
            string build = "";
            int numDigit = newInt.ToString().Length;
            int tempDigit;

            if (numDigit == 1)
            {
                return callehad(newInt);
            }

            for (int i = 0; i < numDigit - 1; i++)
            {
                //for 0 - 20
                if (i == numDigit - 2)
                    tempDigit = newInt - ((newInt / 100) * 100);
                else
                {
                    tempDigit = newInt / Convert.ToInt32(Math.Pow(10, (numDigit - i - 1)));
                    newInt = newInt - tempDigit * Convert.ToInt32(Math.Pow(10, (numDigit - i - 1)));
                }
                build += callNumber(tempDigit, i, numDigit);
            }
            return build;
        }

        private string callNumber(int tempDigit, int i, int numDigit)
        {
            switch (numDigit - i)
            {
                case 4:
                    return callAlef(tempDigit);
                case 3:
                    return callMeha(tempDigit);
                case 2:
                    return callEser(tempDigit);
            }
            return "";
        }

        private string callehad(int tempDigit)
        {
            switch (tempDigit)
            {
                case 1:
                    return "ehahat ";
                case 2:
                    return "shtaim  ";
                case 3:
                    return "shalosh  ";
                case 4:
                    return "arba  ";
                case 5:
                    return "hamesh  ";
                case 6:
                    return "shesh  ";
                case 7:
                    return "sheva  ";
                case 8:
                    return "shmone  ";
                case 9:
                    return "tesha  ";
            }

            return "";
        }

        private string cellvehead(int tempDigit)
        {
            if (tempDigit > 0)
                return (" ve" + callehad(tempDigit));
            else
                return "";
        }

        private string callEser(int tempDigit)
        {
            if (tempDigit < 10)
                return callehad(tempDigit);
            else if (tempDigit > 19)
            {
                int aheddigit = tempDigit - (tempDigit / 10) * 10;
                switch (tempDigit / 10)
                {
                    case 2:
                        return " esrim " + cellvehead(aheddigit);
                    case 3:
                        return " shloshim  " + cellvehead(aheddigit);
                    case 4:
                        return " arabim   " + cellvehead(aheddigit);
                    case 5:
                        return " hamishim   " + cellvehead(aheddigit);
                    case 6:
                        return " shishim   " + cellvehead(aheddigit);
                    case 7:
                        return " shivim " + cellvehead(aheddigit);
                    case 8:
                        return " shmonim  " + cellvehead(aheddigit);
                    case 9:
                        return " tishim " + cellvehead(aheddigit);
                }
            }
            // 10 - 19
            else
            {
                switch (tempDigit)
                {
                    case 11:
                        return "veehahat esre";
                    case 12:
                        return "veshtaim  esre";
                    case 13:
                        return "veshalosh  esre";
                    case 14:
                        return "vearba  esre";
                    case 15:
                        return "vehamesh  esre";
                    case 16:
                        return "veshesh  esre";
                    case 17:
                        return "vesheva  esre";
                    case 18:
                        return "veshmone  esre";
                    case 19:
                        return "vetesha  esre";
                }

            }

            return "";
        }

        private string callMeha(int tempDigit)
        {
            switch (tempDigit)
            {
                case 1:
                    return " me'a  ";
                case 2:
                    return " mataiim ";
                case 3:
                    return " shalosh me'aot ";
                case 4:
                    return " arba me'aot ";
                case 5:
                    return " hamesh  me'aot ";
                case 6:
                    return " shesh  me'aot ";
                case 7:
                    return " sheva  me'aot ";
                case 8:
                    return " shmone  me'aot ";
                case 9:
                    return " tesha  me'aot ";
            }

            return "";
        }

        private string callAlef(int tempDigit)
        {
            switch (tempDigit)
            {
                case 1:
                    return " elef  ";
                case 2:
                    return " alpaim  ";
                case 3:
                    return " shaloshet alafim ";
                case 4:
                    return " arbahat alafim  ";
                case 5:
                    return " hameshet alafim  ";
                case 6:
                    return " sheshet alafim  ";
                case 7:
                    return " shevat alafim  ";
                case 8:
                    return " shmonat alafim  ";
                case 9:
                    return " teshat alafim  ";
            }

            return "";
        }

        public void addSpeech(int y, string textToSpeech)
        {

        }
    }
}