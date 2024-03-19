using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchProgamModul3.Shared.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string Url { get; set; } = "Default";
        public DateTime IdexTime { get; set; }
        public DateTime CreationTime { get; set; }
        public Document()
        {
            
        }
    }
}
