using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblBatch
    {
        public long BatchId { get; set; }
        public string BatchName { get; set; }
        public long? ClassId { get; set; }
        public DateTime? Dateofcommencement { get; set; }
        public string DayId { get; set; }
    }
}
