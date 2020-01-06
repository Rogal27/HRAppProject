using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class ApplicationStatus
    {
        public ApplicationStatus()
        {
            Applications = new HashSet<Applications>();
        }

        public int ApplicationStatusId { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public ICollection<Applications> Applications { get; set; }
    }
}
