using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblstudentteacher
    {
        public long Teacherstudentmapid { get; set; }
        public long? Teacherid { get; set; }
        public int? Studentid { get; set; }
        public decimal? Ammount { get; set; }
        public DateTime? Date { get; set; }
        public long? Subjectid { get; set; }
    }
}
