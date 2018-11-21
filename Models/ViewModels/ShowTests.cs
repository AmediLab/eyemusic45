using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Models.ViewModels
{
    public class showTests
    {
        public showTests(string t_level, string t_stage,int numEX, int NumQuestions)
        {
            level = t_level;
            stage = t_stage;
            num_exam = numEX;
            max_num_question = NumQuestions;
        }

        public int num_exam { get; set; }
        public Nullable<int> max_num_question { get; set; }
        public string level { get; set; }
        public string stage { get; set; }
        public int user_ID { get; set; }
    }
}