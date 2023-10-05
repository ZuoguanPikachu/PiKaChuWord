using PiKaChuWord.Model;
using SQLite;

namespace PiKaChuWord.Service
{
    internal class DataBaseService
    {
        private SQLiteAsyncConnection dataBase;

        public DataBaseService()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wordsList.db");
            dataBase = new SQLiteAsyncConnection(path);
            dataBase.CreateTableAsync<Word>();
        }

        public async Task<List<Word>> GetList()
        {
            List<Word> words_list = await dataBase.Table<Word>().ToListAsync();

            return words_list;
        }

        public async Task AddWord(Word newWord)
        {
            await dataBase.InsertOrReplaceAsync(newWord);
        }

        public async Task DeleteWord(Word word)
        {
            await dataBase.DeleteAsync(word);
        }
    }
}
