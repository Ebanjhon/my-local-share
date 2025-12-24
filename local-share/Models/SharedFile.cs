using System;
using System.Collections.Generic;
using System.Text;

namespace local_share.Models
{
    public class SharedFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public long Size { get; set; }
    }
}
