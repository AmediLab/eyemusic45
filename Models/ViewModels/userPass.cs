using System;
using System.Collections.Generic;

namespace eyemusic45.Models.ViewModels
{
    public partial class userPass
    {
        public string name { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public string level_blind { get; set; }
        public int birth_year { get; set; }
        public string gander { get; set; }
        public int age_of_blind { get; set; }
        public string place { get; set; }
        public string previos_expirments { get; set; }
        public int previos_expirments_more { get; set; }
        public string yourself { get; set; }
        public int precent { get; set; }
    }
}