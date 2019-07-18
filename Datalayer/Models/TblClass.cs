using System;
using System.Collections.Generic;

namespace Datalayer.Models
{
    public partial class TblClass
    {
        public TblClass()
        {
            TblBatch = new HashSet<TblBatch>();
        }

        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public ICollection<TblBatch> TblBatch { get; set; }
    }
}
