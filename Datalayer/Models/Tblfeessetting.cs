using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblfeessetting
    {
        public int Feeid { get; set; }
        public long? SubjecBatchtId { get; set; }
        public decimal? Ammount { get; set; }
        public long? DurationTypeId { get; set; }
        public long? ClassId { get; set; }
    }
}
