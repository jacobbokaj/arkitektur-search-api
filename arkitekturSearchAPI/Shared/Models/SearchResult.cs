using SearchProgamModul3.Shared.Models;

namespace SearchProgamModul3.Shared.Models
{
    public class SearchResult
    {

        public String[] Query { get; set; } = Array.Empty<String>();

        public int Hits { get; set; }

        public List<DocumentHit> DocumentHits { get; set; } = new List<DocumentHit>();

        public List<string> Ignored { get; set; } = new List<string>();

        public TimeSpan TimeUsed { get; set; }
        public SearchResult(String[] query, int hits, List<DocumentHit> documents, List<string> ignored, TimeSpan timeUsed)
        {
            Query = query;
            Hits = hits;
            DocumentHits = documents;
            Ignored = ignored;
            TimeUsed = timeUsed;
        }
        public SearchResult()
        {
            
        }
    }
}
