using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class CV
    {
        public CV()
        {
            Applications = new HashSet<Applications>();
        }

        public int CVID { get; set; }
        public string Path { get; set; }

        public ICollection<Applications> Applications { get; set; }
    }
}
