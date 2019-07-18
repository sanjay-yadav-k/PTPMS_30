using System;
using System.Collections.Generic;

namespace Datalayer.Models
{
    public partial class Tbllogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? StudientId { get; set; }
        public int? Userid { get; set; }
        public int? Typeid { get; set; }
    }
}
