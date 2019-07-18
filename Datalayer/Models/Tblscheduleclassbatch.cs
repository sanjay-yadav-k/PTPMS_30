using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblscheduleclassbatch
    {
        public int Classdatemapid { get; set; }
        public long? Teacherstudentmapid { get; set; }
        public DateTime? Date { get; set; }
        public long? Techermapid { get; set; }
        public bool Isbatch { get; set; }
        public bool? IsStartClass { get; set; }
        public bool? IsCopletedClass { get; set; }
        public TimeSpan? Totime { get; set; }
        public TimeSpan? Fromtime { get; set; }
    }
}
