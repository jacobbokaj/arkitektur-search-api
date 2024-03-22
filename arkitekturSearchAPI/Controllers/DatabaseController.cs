using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SearchProgamModul3.Server.Models;
using SearchProgamModul3.Server.SearchMehanics;
using SearchProgamModul3.Shared.Models;

namespace SearchProgamModul3.Server.Controllers
{
    [ApiController]
    [Route("database")]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseRepository Repository = new DatabaseDBContext();

        public DatabaseController(IDatabaseRepository databaseRepository)
        {
            if (Repository == null && databaseRepository != null)
            {
                Repository = databaseRepository;
                Console.WriteLine("Repository initialized");
            }
        }

        [HttpGet("words")]
        public IEnumerable<Word> GetAllWords()
        {
            return Repository.GetAllWords();
        }


        [HttpGet("words/{id}")]
        public Word GetWordById(int id)
        {
            return Repository.GetWordById(id);
        }

        [HttpGet("names")]
        public SearchResult GetWordWithCondition([FromQuery] string query, [FromQuery] string cs)
        {
            
            SearchLogic searchLogic = new SearchLogic(Repository);

            string[] queryList = query.Split("_");
            string csFlag = cs;

            if (csFlag == "true" || csFlag == "false")
            {
                return searchLogic.Search(queryList, 10, cs == "true");
            }
            else
            {
                return new SearchResult();
            }
        }

        [HttpGet("wordsWithFrequncies")]
        public List<WordWithFrequrency> GetWordsWithFrequncies()
        {
            return Repository.GetWordsFrequncies();
        }
    }
}
