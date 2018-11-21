using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Models.ViewModels
{
    public class userList
    {

        public userList(int tID,string tName)
        {
            ID = tID;
            name = tName;
        }

        public int ID;
        public string name; 
    }
}