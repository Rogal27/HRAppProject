using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRWebApplication.Models
{
    public class ApplicationCreateModel : Applications
    {
        public IFormFile CVFile { get; set; }
        public List<IFormFile> OtherAttachmentsFiles { get; set; }
    }
}
