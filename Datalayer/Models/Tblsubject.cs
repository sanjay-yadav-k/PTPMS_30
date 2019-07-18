using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblsubject
    {
        public long Subjectid { get; set; }
        public string Subjectname { get; set; }
        public string DayId { get; set; }
        public string Imagename { get; set; }
        public bool? Isshow { get; set; }
    }
}
