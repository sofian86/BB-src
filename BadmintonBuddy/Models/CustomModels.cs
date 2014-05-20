using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BadmintonBuddy.Models
{
    #region ViewModel
    public partial class ClubViewModel
    {
        public String ClubName{get;set;}
        [DataType(DataType.MultilineText)]
        public String Address { get; set; }
        public String AreaTag { get; set; }
        public String Phone { get; set; }
        public Int32 Surface { get; set; }
        public String Timings { get; set; }
        public String Fees { get; set; }
        public String Website { get; set; }
        public Int32 NoOfCourts { get; set; }
        public String MetaName { get; set; }
        [DataType(DataType.MultilineText)]
        public String MetaValue { get; set; }
        
        public String City { get; set; }
        public String State { get; set; }
        public String Country { get; set; }    

    }

    public partial class SearchClubViewModel
    {
        public Club club { get; set; }
        public List<Metadata> metaDataList { get; set; }
    }
    #endregion
}