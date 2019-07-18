using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.BusinessLogic
{
    class Configuration
    {
        public class Classmodel
        {
            public string ClassName { get; set; }
        }

        public class Batchmodel
        {
            public string BatchName { get; set; }
            public string Timing { get; set; }
            public string SubscriptionType { get; set; }
            public decimal? Fees { get; set; }
            public int? ClassId { get; set; }
            public DateTime? Dateofcommencement { get; set; }
            public int? CourseDuration { get; set; }

        }
    }
}
