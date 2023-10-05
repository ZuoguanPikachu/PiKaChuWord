using PiKaChuWord.Model;
using PiKaChuWord.Utils;

namespace PiKaChuWord.Service
{
    internal class WordQueryService
    {
        private DataBaseService dataBaseService;
        private SogoTranslation translator = new();

        public async Task<Dictionary<string, string>> QueryWord(string query)
        {
            List<Word> words_list = await dataBaseService.GetList();
            foreach (Word item in words_list)
            {
                if (item.Vocabulary == query)
                {
                    return new() { { "word_status", "o" }, { "translation", item.Translation } };
                }
            }

            return translator.Query(query);
        }

        public WordQueryService(DataBaseService dataBaseService) 
        {
            this.dataBaseService = dataBaseService;
        }

    }
}
