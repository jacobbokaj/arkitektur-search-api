using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchProgamModul3.Shared.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";

        public Word()
        {

        }
        public Word(int id)
        {
            Id = id;
        }
        public Word(int id, string name) { 
            Id = id;
            Name = name;       
        }
    }
}
