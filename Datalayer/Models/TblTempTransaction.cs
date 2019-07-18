using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblTempTransaction
    {
        public int TempTransactionId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsSubject { get; set; }
        public long? SubBatchId { get; set; }
        public int? TransactionId { get; set; }
        public long? DurationType { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? Period { get; set; }
    }
}
