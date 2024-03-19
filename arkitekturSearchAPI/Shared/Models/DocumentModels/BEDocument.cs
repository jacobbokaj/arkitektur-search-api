using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchProgamModul3.Shared.Models
{
    public class BEDocument
    {
        public int MId { get; set; }
        public string MUrl { get; set; } = "Default";
        public string MIdxTime { get; set; } = "Default";
        public string MCreationTime { get; set; } = "Default ";
    }
}
