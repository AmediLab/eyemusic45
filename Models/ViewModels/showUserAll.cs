using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyemusic45.Models.ViewModels
{
    public class showUserAll
    {
        public List<DAL.resualt_exam_row> for_test;
        public List<showTrain> for_train;

        public showUserAll(List<DAL.resualt_exam_row> for_test, List<showTrain> for_train)
        {
            // TODO: Complete member initialization
            this.for_test = for_test;
            this.for_train = for_train;
        }

    }
}