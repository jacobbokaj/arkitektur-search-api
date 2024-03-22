using Microsoft.AspNetCore.Mvc;
using SearchProgamModul3.Server.Models;
using SearchProgamModul3.Shared.Models;
using System.Reflection;

namespace SearchProgamModul3.Server.SearchMehanics
{
    public class SearchLogic
    {
        IDatabaseRepository mDatabase;


        public SearchLogic(IDatabaseRepository database)
        {
            mDatabase = database;
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about amost maxAmount of documents.
         */
        public SearchResult Search(string[] query, int maxAmount,bool caseSentiveFlag)
        {
            List<string> ignored;

            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = mDatabase.GetWordIds(query, out ignored,caseSentiveFlag);

            // perform the search - get all docIds
            var docIds = mDatabase.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            List<DocumentHit> docresult = new List<DocumentHit>();
            int idx = 0;
            foreach (var doc in mDatabase.GetDocDetails(top))
            {
                var missing = mDatabase.WordsFromIds(mDatabase.getMissing(doc.MId, wordIds));

                docresult.Add(new DocumentHit(doc, docIds[idx++].Value, missing));
            }

            return new SearchResult(query, docIds.Count, docresult, ignored, DateTime.Now - start);
        }
    }
}
