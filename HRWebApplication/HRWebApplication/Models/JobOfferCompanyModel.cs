using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRWebApplication.Models
{
    public class JobOfferCompanyModel : JobOffers
    {
        public IEnumerable<Companies> CompaniesCollection { get; set; }

        public JobOfferCompanyModel()
        {
        }

        public JobOffers GetJobOffer()
        {
            return (JobOffers)this;
        }
    }
}
