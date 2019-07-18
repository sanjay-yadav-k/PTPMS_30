using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblteachertopay
    {
        public long TeacherpayId { get; set; }
        public long? TeacherId { get; set; }
        public int? Classdatemapid { get; set; }
        public decimal? Ammount { get; set; }
    }
}
