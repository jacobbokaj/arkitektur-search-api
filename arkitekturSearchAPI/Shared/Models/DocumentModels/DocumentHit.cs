namespace SearchProgamModul3.Shared.Models
{
    public class DocumentHit
    {
        public DocumentHit(BEDocument doc, int noOfHits, List<string> missing)
        {
            Document = doc;
            NoOfHits = noOfHits;
            Missing = missing;
        }
        public DocumentHit()
        {
            
        }

        public BEDocument Document { get; set; }

        public int NoOfHits { get; set; }

        public List<string> Missing { get; set; }
    }
}
