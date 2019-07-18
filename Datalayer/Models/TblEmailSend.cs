using System;
using System.Collections.Generic;

namespace Datalayer.Models
{
    public partial class TblEmailSend
    {
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string SenderMailId { get; set; }
        public string RecieverMailId { get; set; }
        public string Body { get; set; }
        public DateTime? SendDateTime { get; set; }
        public int? SenderTypeId { get; set; }
        public int? RecieverTypeId { get; set; }
        public int? StudentId { get; set; }
        public long? TeacherId { get; set; }
        public bool Isadmin { get; set; }
    }
}
