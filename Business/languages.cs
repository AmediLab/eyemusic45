using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eyemusic45.DAL;

namespace eyemusic45.Business
{
    public class languages
    {
        bool ifHebrow = false;
        static bool IAmIn = false;
        private static Dictionary<string, string> EngHeb;


        public languages(string map,bool Hebrow)
        {
            if (EngHeb == null && !IAmIn)
            {
                IAmIn = true;
                addHeb(map);
            }

            ifHebrow = Hebrow;
        }

        public void changeLan(string hebrow)
        {
            if (hebrow == "h")
                ifHebrow = true;
            else
                ifHebrow = false;
        }

        /// <summary>
        /// Create dictionary for translate from English image file name to Hebrew name
        /// </summary>
        /// <param name="map">The path of website</param>
        private void addHeb(string map)
        {
            EngHeb = new Dictionary<string, string>();
            int index = 0;

            string[] textHeb = System.IO.File.ReadAllLines(map + "/Texts/HEB/Dic.txt");
            string[] textEng = System.IO.File.ReadAllLines(map + "/Texts/ENG/Dic.txt");

            for (index = 0; index < textEng.Count();index++)
            {
                if (!EngHeb.ContainsKey(textEng[index]))
                    EngHeb.Add(textEng[index], textHeb[index]);
            }
        }

        /// <summary>
        /// Get hebrew name from dictionary 
        /// </summary>
        /// <param name="english">The english for translate</param>
        /// <returns>The Hebrew name</returns>
        public string returnStr(string english)
        {
            if (ifHebrow)
            {
                if (EngHeb.ContainsKey(english))
                    return EngHeb[english];
                else
                    return english;
            }
            else
                return english;
        }
    }
}