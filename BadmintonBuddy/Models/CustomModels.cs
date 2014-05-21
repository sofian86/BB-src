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
        public Club Club { get; set; }
        
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