using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblTransaction
    {
        public int TransactionId { get; set; }
        public int? StudentId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? DateTime { get; set; }
        public string Status { get; set; }
        public string PayUMoneyTxnId { get; set; }
        
    }
}
