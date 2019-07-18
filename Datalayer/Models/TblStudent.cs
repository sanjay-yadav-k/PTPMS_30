using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class TblStudent
    {
        public TblStudent()
        {
            TblstudentSubjectBatch = new HashSet<TblstudentSubjectBatch>();
        }
            public int StudentId { get; set; }
            public string Fname { get; set; }
            public string Lname { get; set; }
            public string Username { get; set; }
            public DateTime? Dob { get; set; }
            public decimal? MobileNo { get; set; }
            public long? ClassId { get; set; }
            public string ParentName { get; set; }
            public string Address { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public decimal? Zipcode { get; set; }
            public string Email { get; set; }
            public string Skype { get; set; }
       
        public ICollection<TblstudentSubjectBatch> TblstudentSubjectBatch { get; set; }
    }
}
