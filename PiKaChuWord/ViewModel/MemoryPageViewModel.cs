using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PiKaChuWord.Model;
using PiKaChuWord.Service;

namespace PiKaChuWord.ViewModel
{
    partial class MemoryPageViewModel : ObservableObject
    {
        private DataBaseService dataBaseService;
        private List<Word> words;
        private List<Word> quizWords;

        [ObservableProperty]
        DateTime earlyDate;

        [ObservableProperty]
        DateTime lateDate;

        [ObservableProperty]
        int index = 0;

        [ObservableProperty]
        int count = 0;

        [ObservableProperty]
        string vocabulary;

        [ObservableProperty]
        string translation;

        [ObservableProperty]
        bool ansHidden = true;

        [ObservableProperty]
        List<string> filterModes = new (){"全部", "近1周", "近2周", "近1月"};

        [ObservableProperty]
        string filterMode;

        [RelayCommand]
        void Filter()
        {
            if (words.Count == 0) return;

            LateDate = DateTime.ParseExact(words[0].Date.ToString(), "yyyyMMdd", null);
            switch (FilterMode)
            {
                case "全部":
                    EarlyDate = DateTime.ParseExact(words[words.Count - 1].Date.ToString(), "yyyyMMdd", null);
                    LoadQuizWords(words);
                    break;
                case "近1周":
                    EarlyDate = LateDate.AddDays(-7);
                    ChangeDate();
                    break;
                case "近2周":
                    EarlyDate = LateDate.AddDays(-14);
                    ChangeDate();
                    break;
                case "近1月":
                    EarlyDate = LateDate.AddMonths(-1);
                    ChangeDate();
                    break;
                default:
                    break;
            }
        }


        [RelayCommand]
        async Task Load()
        {
            words = await dataBaseService.GetList();
            FilterMode = "全部";
            if (words.Count == 0) return;

            words = words.OrderByDescending(item => item.Date).ToList();
            LateDate = DateTime.ParseExact(words[0].Date.ToString(), "yyyyMMdd", null);
            EarlyDate = DateTime.ParseExact(words[words.Count - 1].Date.ToString(), "yyyyMMdd", null);
            LoadQuizWords(words);
            
        }

        [RelayCommand]
        void Next(int step)
        {
            if (quizWords.Count == 0) return;

            Index += Convert.ToInt32(step);
            if (Index > Count)
            {
                Index = 1;
            }
            else if(Index <= 0)
            {
                Index = Count;
            }
            DisplayNewWord();
        }

        [RelayCommand]
        void ShowAns()
        {
            if (AnsHidden)
            {
                AnsHidden = false;
            }
            else
            {
                Next(1);
            }
        }

        [RelayCommand]
        void ChangeDate()
        {
            if (words.Count == 0) return;

            List<Word> selectedWords = words.Where(
                item => item.Date >= Convert.ToInt32(EarlyDate.ToString("yyyyMMdd")) && item.Date <= Convert.ToInt32(LateDate.ToString("yyyyMMdd"))
            ).ToList();

            if (selectedWords.Count == 0) return;
            LoadQuizWords(selectedWords);
        }

        void LoadQuizWords(List<Word> quizWords)
        {
            this.quizWords = ShuffleList(quizWords);
            Count = quizWords.Count;
            Index = 1;
            DisplayNewWord();
        }

        List<T> ShuffleList<T>(List<T> list)
        {
            Random random = new();
            List<T> shuffledList = list.ToList();
            int n = shuffledList.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (shuffledList[n], shuffledList[k]) = (shuffledList[k], shuffledList[n]);
            }
            return shuffledList;
        }

        void DisplayNewWord()
        {
            Vocabulary = quizWords[Index - 1].Vocabulary;
            Translation = quizWords[Index - 1].Translation;
            AnsHidden = true;
        }

        public MemoryPageViewModel(DataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
            Task.Run(Load);
        }
    }
}
