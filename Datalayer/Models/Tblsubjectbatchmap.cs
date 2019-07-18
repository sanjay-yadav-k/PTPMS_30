using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblsubjectbatchmap
    {
        public long BatchmapId { get; set; }
        public long? Batchid { get; set; }
        public long? SubjectId { get; set; }
    }
}
