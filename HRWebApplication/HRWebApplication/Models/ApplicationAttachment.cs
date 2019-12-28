using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class ApplicationAttachment
    {
        public int ApplicationAttachmentId { get; set; }
        public int ApplicationId { get; set; }
        public int AttachmentId { get; set; }

        public Applications Application { get; set; }
        public Attachments Attachment { get; set; }
    }
}
