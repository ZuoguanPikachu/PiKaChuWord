using SQLite;

namespace PiKaChuWord.Model
{
    internal class Word
    {
        [PrimaryKey]
        public string Vocabulary { get; set; }
        public string Translation { get; set; }
        public int Date { get; set; }
    }
}
