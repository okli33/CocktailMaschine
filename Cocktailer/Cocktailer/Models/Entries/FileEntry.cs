using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.Entries
{
    public class FileEntry : IEntry
    {
        public string Name { get; set; }
        public IFile File { get; set; }
        public bool Selected { get; set; }
    }
}
