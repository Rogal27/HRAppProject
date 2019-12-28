using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class HrjobOffers
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobOfferId { get; set; }

        public JobOffers JobOffer { get; set; }
        public Users User { get; set; }
    }
}
