using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblteacherclassbatchmap
    {
        public long Techermapid { get; set; }
        public long? Subjectid { get; set; }
        public long? Classid { get; set; }
        public int Batchid { get; set; }
        public decimal? Amount { get; set; }
        public long TeacherId { get; set; }
    }
}
