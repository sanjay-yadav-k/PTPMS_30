using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblmapsubjecttime
    {
        public long Subjecttimemapid { get; set; }
        public long? SubjectId { get; set; }
        public TimeSpan? Time { get; set; }
    }
}

