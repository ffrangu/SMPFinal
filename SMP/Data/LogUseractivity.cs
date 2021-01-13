using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class LogUseractivity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Activity { get; set; }
        public string HttpMethod { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
