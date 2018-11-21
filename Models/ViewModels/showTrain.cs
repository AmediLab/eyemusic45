using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Models.ViewModels
{
    public class showTrain
    {
        public showTrain(string t_level, string t_stage,string t_desc, DateTime t_date)
        {
            level = t_level;
            stage = t_stage;
            date = t_date;
            desc = t_desc;
        }

        public string level;
        public string stage;
        public string desc;
        public DateTime date;
    }
}