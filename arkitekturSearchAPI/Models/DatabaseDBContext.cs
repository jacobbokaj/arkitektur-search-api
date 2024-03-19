using Microsoft.Data.Sqlite;
using SearchProgamModul3.Server.SearchMehanics;
using SearchProgamModul3.Shared;
using SearchProgamModul3.Shared.Models;

namespace SearchProgamModul3.Server.Models
{
    public class DatabaseDBContext : IDatabaseRepository
    {
        private SqliteConnection _connection;

        public SearchLogic SearchLogic { get; private set; }


        public DatabaseDBContext()
        {

            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            connectionStringBuilder.DataSource = Paths.DATABASE;

            SQLitePCL.Batteries.Init();
            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _connection.Open();
        }

        public List<Word> GetAllWords()
        {
            List<Word> words = new List<Word>();
            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM word";

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var idd = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    words.Add(new Word() { Id = idd, Name = w });
                }
            }
            return words;
        }


        /// <summary>
        /// if the paramenter is 1 it means true else 0 it is false <br></br>
        /// The reason is you can't send a request with a bool, but can with 1 and 0 because it is bytes
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<Word> GetAllWordsWithCondition(int flag)
        {
            List<Word> words = new List<Word>();
            var selectCmd = _connection.CreateCommand();

            string condition = String.Empty;
            if (flag == 1)
            {
             condition = flag == 1 ? "'^[A-Z].*$'" : "^[a-z].*$";

            }
            selectCmd.CommandText = "SELECT * FROM word WHERE name REGEXP " + condition;

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var idd = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    words.Add(new Word() { Id = idd, Name = w });
                }
            }
            return words;
        }

        public List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds)
        {
            var res = new List<KeyValuePair<int, int>>();

            /* Example sql statement looking for doc id's that
               contain words with id 2 and 3
            
               SELECT docId, COUNT(wordId) as count
                 FROM Occ
                WHERE wordId in (2,3)
             GROUP BY docId
             ORDER BY COUNT(wordId) DESC 
             */

            var sql = "SELECT docId, COUNT(wordId) as count FROM Occ where ";
            sql += "wordId in " + AsString(wordIds) + " GROUP BY docId ";
            sql += "ORDER BY count DESC;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var docId = reader.GetInt32(0);
                    var count = reader.GetInt32(1);

                    res.Add(new KeyValuePair<int, int>(docId, count));
                }
            }

            return res;
        }

        private string AsString(List<int> x) => $"({string.Join(',', x)})";

        public Word GetWordById(int id)
        {
            Word word = new Word();
            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM word WHERE ID = '{id}'";

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var idd = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    word = new Word() { Id = idd, Name = w };
                }
            }
            return word;
        }

        public List<int> GetWordIds(string[] query, out List<string> outIgnored,bool caseSensitiveFlag)
        {
          List<Word>  mWords = GetAllWords();


            var res = new List<int>();
            var ignored = new List<string>();


            foreach (var item in mWords)
            {
                bool wordAdd = false;
                for (int i = 0; i < query.Length; i++)
                {
                    bool wordMatchFlag = caseSensitiveFlag ? item.Name == query[i] : string.Equals(item.Name, query[i], StringComparison.OrdinalIgnoreCase);

                    if (wordMatchFlag)
                    {
                        wordAdd = true;
                        break;
                    }
                }
                if (wordAdd)
                {
                    res.Add(item.Id);
                }
                else
                {
                    ignored.Add(item.Name);
                }
            }
            outIgnored = ignored;
            return res;
        }


        public List<BEDocument> GetDocDetails(List<int> docIds)
        {
            List<BEDocument> res = new List<BEDocument>();

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM document where id in " + AsString(docIds);

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var url = reader.GetString(1);
                    var idxTime = reader.GetString(2);
                    var creationTime = reader.GetString(3);

                    res.Add(new BEDocument { MId = id, MUrl = url, MIdxTime = idxTime, MCreationTime = creationTime });
                }
            }
            return res;
        }

        public List<int> getMissing(int docId, List<int> wordIds)
        {
            var sql = "SELECT wordId FROM Occ where ";
            sql += "wordId in " + AsString(wordIds) + " AND docId = " + docId;
            sql += " ORDER BY wordId;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            List<int> present = new List<int>();

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var wordId = reader.GetInt32(0);
                    present.Add(wordId);
                }
            }
            var result = new List<int>(wordIds);
            foreach (var w in present)
                result.Remove(w);


            return result;
        }

        public List<string> WordsFromIds(List<int> wordIds)
        {
            var sql = "SELECT name FROM Word where ";
            sql += "id in " + AsString(wordIds);

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            List<string> result = new List<string>();

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var wordId = reader.GetString(0);
                    result.Add(wordId);
                }
            }
            return result;
        }

        public List<WordWithFrequrency> GetWordsFrequncies()
        {
            /*
              SELECT w.id AS wordID, w.Name AS WordName, COUNT(o.WordId) AS Occurrences
               FROM Word w
               JOIN Occ o ON w.Id = o.WordId
               GROUP BY w.Name
               ORDER BY Occurrences DESC      
            */


            var sql = "SELECT w.id AS wordID, w.Name AS WordName, COUNT(o.WordId) AS Occurrences\r\n" +
                "FROM Word w\r\n" +
                "JOIN Occ o ON w.Id = o.WordId\r\n" +
                "GROUP BY w.Name\r\n" +
                "ORDER BY Occurrences DESC";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            List<WordWithFrequrency> wordWithFrequrencies = new List<WordWithFrequrency>();

            using (var reader = selectCmd.ExecuteReader()){
                while (reader.Read())
                {
                    var wordId = reader.GetInt32(0);
                    var wordName = reader.GetString(1);
                    var wordFrequrency = reader.GetInt32(2);
                    wordWithFrequrencies.Add(new WordWithFrequrency(new Word(wordId, wordName), wordFrequrency));
                }
            }
            return wordWithFrequrencies;
        }
    }
}
