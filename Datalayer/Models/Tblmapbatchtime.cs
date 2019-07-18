using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblmapbatchtime
    {
        public long Batchtimemapid { get; set; }
        public long? BatchId { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
