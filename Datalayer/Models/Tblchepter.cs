using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.Models
{
    public partial class Tblchepter
    {
        public long ChepterId { get; set; }
        public string ChepterName { get; set; }
        public long? SubjectId { get; set; }
        public long? ClassId { get; set; }
        public string Syllabus { get; set; }
    }
}
