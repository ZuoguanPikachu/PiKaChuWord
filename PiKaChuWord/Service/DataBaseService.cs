using PiKaChuWord.Model;
using PiKaChuWord.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiKaChuWord.Service
{
    internal class DataBaseService
    {
        private SQLiteAsyncConnection dataBase;
        private SogoTranslation translator;

        public DataBaseService()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wordsList.db");
            dataBase = new SQLiteAsyncConnection(path);
            dataBase.CreateTableAsync<Word>();

            translator = new();
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

        public async Task<Dictionary<string, string>> QueryWord(string query)
        {
            List<Word> words_list = await GetList();
            foreach (Word item in words_list)
            {
                if (item.Vocabulary == query)
                {
                    return new() { {"is_word", "√" }, { "translation", item.Translation } };
                }
            }

            return translator.Query(query);
        }
    }
}
