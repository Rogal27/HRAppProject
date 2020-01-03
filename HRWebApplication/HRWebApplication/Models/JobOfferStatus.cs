using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    [Serializable]
    public partial class JobOfferStatus
    {
        public JobOfferStatus()
        {
            JobOffers = new HashSet<JobOffers>();
        }

        public int JobOfferStatusId { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        public ICollection<JobOffers> JobOffers { get; set; }
    }
}
