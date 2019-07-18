using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblmapstudentduration
    {
        public long Id { get; set; }
        public long? Subjectbacthiid { get; set; }
        public long? Studentid { get; set; }
        public long? DurationId { get; set; }
        public int Period { get; set; }
    }
}
