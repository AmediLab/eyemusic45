using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Speech;
using System.Xml;
using System.Speech.Synthesis;
using System.IO;
using System.Speech.AudioFormat;
using SpeechLib;

namespace eyemusic45.Business
{
    public class speech
    {
        float scanSpeed;
        float Xprev;
        int Xprevmespeak = 0;

        float svgWidth = 500f;
        float svgHigh = 300f;
        private static Dictionary<float, SpeechParm> XinOrder;
        string Numbers;
        int toCouple;

        int TimeApp;
        string lenInr;
        XmlDocument document;

        /// <summary>
        /// get parmter form javascript in view SVG for easy change witout need to compile 
        /// </summary>
        /// <param name="speed">the scan speed of EyeMusic in mulisecound per colmun </param>
        /// <param name="mPath"></param>
        /// <param name="Time">The approximate time for sound the speech</param>
        /// <param name="theCouple">Arbitrary coupling for change the synchronize for speech with eyemusic</param>
        /// <param name="len">The language for speech</param>
        public speech(int speed, string mPath, int Time,int theCouple, string len)
        {
            lenInr = len;
            TimeApp = Time;
            toCouple = theCouple;
            XinOrder = new Dictionary<float, SpeechParm>();
            scanSpeed = speed;            
        }

        internal string add(string xml)
        {
            document = new XmlDocument();
            document.LoadXml(xml);
            return parser();
        }

        internal string add(HttpPostedFileBase file)
        {
            document = new XmlDocument();
            document.Load(file.InputStream);
            return parser();
        }

        internal string addFileName(string file)
        {
            document = new XmlDocument();
            document.Load(file);
            return parser();
        }

        /// <summary>
        /// Parser SVG file to found text for speech
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal string parser()
        {
            float transformX = 0;
            float transformY = 0;
            float scaleX = 1;
            float scaleY = 1;
            float matrixX = 0;
            float matrixY = 0;


            if (document.DocumentElement.Attributes != null && document.DocumentElement.Attributes["transform"] != null)
            {
                string transform = document.DocumentElement.Attributes["transform"].Value;
                string[] XAndYTrans;
                char tosplit = ',';

                if (transform.IndexOf(",") == -1)
                    tosplit = ' ';
                //take the values after translate to know where the object is located
                if (transform.StartsWith("translate("))
                {
                    XAndYTrans = transform.Substring(10, transform.IndexOf(')') - 10).Split(tosplit);

                    transformX += float.Parse(XAndYTrans[0]);
                    transformY += float.Parse(XAndYTrans[1]);
                }
                else if (transform.StartsWith("scale("))
                {
                    XAndYTrans = transform.Substring(6, transform.IndexOf(')') - 6).Split(tosplit);
                    scaleX = float.Parse(XAndYTrans[0]);
                    scaleY = float.Parse(XAndYTrans[1]);
                }
                else if (transform.StartsWith("matrix("))
                {
                    XAndYTrans = transform.Substring(7, transform.IndexOf(')') - 7).Split(tosplit);
                    matrixX = float.Parse(XAndYTrans[4]);
                    matrixY = float.Parse(XAndYTrans[5]);
                }
            }

            //check all child node of parent 
            foreach (XmlNode dd in document.ChildNodes)
            {
                //if this the father SVG
                if (dd != null && dd.Name == "svg" && dd.Attributes != null)
                {
                    //found the size of SVG image
                    if (dd.Attributes["viewBox"] != null)
                    {
                        svgWidth = float.Parse(dd.Attributes["viewBox"].Value.Split(' ')[2]);
                        svgHigh = float.Parse(dd.Attributes["viewBox"].Value.Split(' ')[3]);
                    }
                    //another option for size of SVG image
                    else
                    {
                        svgWidth = float.Parse(dd.Attributes["width"].Value.Replace("px",""));
                        svgHigh = float.Parse(dd.Attributes["height"].Value.Replace("px", ""));
                    }
                }

                if (dd.ChildNodes != null)
                    chceckChild(dd, transformX, transformY, matrixX, matrixY);
            }

            createSpeech();
            return Numbers; //return all text and for each text the pitch and location it should be read, to svgController
        }

        /// <summary>
        ///recorsive on all the node in SVG file
        /// </summary>
        /// <param name="document">the checked node</param>
        /// <param name="tranX">The transform in X axis</param>
        /// <param name="tranY">The transform in Y axis</param>
        /// <param name="matX">The location in X axis</param>
        /// <param name="matY">The location in Y axis</param>
        private void chceckChild(XmlNode document, float tranX, float tranY,float matX,float matY)
        {
            foreach (XmlNode dd in document.ChildNodes)
            {
                float scaleX = 1;
                float scaleY = 1;
                float matrixX = 0;
                float matrixY = 0;
                float transformX = tranX;
                float transformY = tranY;

                //check thereal location of text in SVG (according to transform proparty) 
                //save the transform for child nodes 
                if (dd.Attributes != null && dd.Attributes["transform"] != null)
                {
                    string transform = dd.Attributes["transform"].Value;
                    string[] XAndYTrans;
                    char tosplit = ',';

                    if (transform.IndexOf(",") == -1)
                        tosplit = ' ';

                    if (transform.StartsWith("translate("))
                    {    
                        XAndYTrans = transform.Substring(10, transform.IndexOf(')') - 10).Split(tosplit);
                        
                        transformX += float.Parse(XAndYTrans[0]);
                        transformY += float.Parse(XAndYTrans[1]);
                    }
                    else if (transform.StartsWith("scale("))
                    {
                        XAndYTrans = transform.Substring(6, transform.IndexOf(')') - 6).Split(tosplit);
                        scaleX = float.Parse(XAndYTrans[0]);
                        scaleY = float.Parse(XAndYTrans[1]);
                    }
                    else if (transform.StartsWith("matrix("))
                    {
                        XAndYTrans = transform.Substring(7, transform.IndexOf(')') - 7).Split(tosplit);
                        matrixX = float.Parse(XAndYTrans[4]);
                        matrixY = float.Parse(XAndYTrans[5]);
                    }
                }


                //If this is text tag 
                //add to list for speech
                if (dd.Name == "text" && dd.InnerText != "")
                {
                    if (dd.Attributes["x"] != null && dd.Attributes["y"] != null)
                    {
                        //compute the REAL text location (together with all the transformations that were before)
                        float Temp = float.Parse(dd.Attributes["x"].Value.Replace("px","")) * scaleX + tranX + matX;

                        while (XinOrder.ContainsKey(Temp))
                        {
                            Temp += 1;
                        }

                        //if (!XinOrder.ContainsKey(float.Parse(dd.Attributes["x"].Value) * scaleX + tranX + matX))
                        //creates Xinorder which has all text includings their x and y values
                        XinOrder.Add(Temp, new SpeechParm(float.Parse(dd.Attributes["y"].Value.Replace("px", "")) * scaleY + tranY + matY, dd.InnerText.Replace("-", " -"), lenInr));
                    }
                    else
                        XinOrder.Add(tranX + matrixX, new SpeechParm(tranY + matrixY, dd.InnerText, lenInr));
                    //addSpeace(dd.InnerText, float.Parse(dd.Attributes["y"].Value), float.Parse(dd.Attributes["x"].Value));
                    //Xprev = float.Parse(dd.Attributes["x"].Value);
                }
                else if (dd.ChildNodes != null) //if it's not a text and it has a child, check its child
                    chceckChild(dd, transformX, transformY, matrixX, matrixY);
            }
        }

        /// <summary>
        /// create speech from the list of texts 
        /// </summary>
        void createSpeech()
        {
            //arrange list by order - by the x key of each
            var list = XinOrder.Keys.ToList();
            list.Sort();

            foreach (var key in list)
            {
                addSpeace(XinOrder[key].Text, XinOrder[key].Y, key);
                Xprev = key;
            }
        }

        /// <summary>
        /// clculate the frequancy and the syncronize for specipic text  
        /// </summary>
        /// <param name="speechIt">the text to speech</param>
        /// <param name="Yattr">the Y axis location</param>
        /// <param name="Xattr">The X axis location</param>
        private void addSpeace(string speechIt, float Yattr, float Xattr)
        {
            //calculte the pitch and position for each text object
            //calculate pitch
            string heraz = (Convert.ToInt32((svgHigh - Yattr) * 100) / svgHigh).ToString();            //pb.AppendSsmlMarkup("<prosody pitch=\"" + heraz + "Hz\">");
          
            Xprevmespeak = TimeApp;
            //calculate position 
            int precent = Convert.ToInt32((Xattr / svgWidth) * scanSpeed * toCouple);
            
                precent = precent - Xprevmespeak;

            if (precent < 0)
                precent = 0;

            //The format for mespeck in javascript
            Numbers += ";" + speechIt + "," + heraz + "," + precent + "," + Xattr + "," + Yattr;
        }

       
    }
}