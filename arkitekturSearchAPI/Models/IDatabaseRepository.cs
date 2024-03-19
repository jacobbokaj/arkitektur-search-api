using SearchProgamModul3.Server.SearchMehanics;
using SearchProgamModul3.Shared.Models;

namespace SearchProgamModul3.Server.Models
{
    public interface IDatabaseRepository
    {
        List<Word> GetAllWords();
        List<int> GetWordIds(string[] query, out List<string> outIgnored, bool caseSentive);

        List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds);
        /// <summary>
        /// if the paramenter is 1 it means true else 0 it is false <br></br>
        /// The reason is you can't send a request with a bool, but can with 1 and 0 because it is bytes
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        List<Word> GetAllWordsWithCondition(int flag);

        Word GetWordById(int id);

        List<BEDocument> GetDocDetails(List<int> docIds);
        List<int> getMissing(int docId, List<int> wordIds);
        List<string> WordsFromIds(List<int> wordIds);

        SearchLogic SearchLogic { get; }

       List<WordWithFrequrency> GetWordsFrequncies();
    }
}
