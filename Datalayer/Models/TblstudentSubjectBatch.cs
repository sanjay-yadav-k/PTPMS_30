using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblstudentSubjectBatch
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public long? SubjectId { get; set; }
        public long? BatchId { get; set; }
        public int? DurationId { get; set; }
        public int? Period { get; set; }
    }
}
