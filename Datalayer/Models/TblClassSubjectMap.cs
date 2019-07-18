using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblClassSubjectMap
    {
        public long ClassSubjectId { get; set; }
        public long? Classid { get; set; }
        public long? Subjectid { get; set; }
        public long? Batchid { get; set; }
    }
}
