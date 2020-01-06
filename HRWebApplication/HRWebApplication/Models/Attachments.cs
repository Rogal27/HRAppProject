using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class Attachments
    {
        public Attachments()
        {
            ApplicationAttachment = new HashSet<ApplicationAttachment>();
        }

        public int AttachmentId { get; set; }
        public string Path { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationAttachment> ApplicationAttachment { get; set; }
    }
}
