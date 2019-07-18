using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblmapteacherammount
    {
        public long TecherammountId { get; set; }
        public long? TeacherId { get; set; }
        public long? ClassBatchId { get; set; }
        public bool? Isbatch { get; set; }
        public long? SubjectId { get; set; }
        public long? Typeid { get; set; }
        public decimal? AmountTopay { get; set; }
    }
}
