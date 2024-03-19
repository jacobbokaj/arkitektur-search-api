using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchProgamModul3.Shared.Models
{
    public class WordWithFrequrency
    {
        public Word Word { get; set; } = new Word();
        public int Frequncy { get; set; }

        public WordWithFrequrency()
        {

        }

        public WordWithFrequrency(Word word, int frequncy)
        {
            Word = word;
            Frequncy = frequncy;
        }

    }
}
