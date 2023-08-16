using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PiKaChuWord.Service;

namespace PiKaChuWord.ViewModel
{
    internal partial class AddPageViewModel : ObservableObject
    {
        private DataBaseService dataBaseService;

        [ObservableProperty]
        string vocabulary;

        [ObservableProperty]
        string translation;

        [ObservableProperty]
        string date = DateTime.Now.ToString("yyyyMMdd");

        [ObservableProperty]
        Color textcolor = Colors.Black;

        [ObservableProperty]
        string isWord = "?";

        [RelayCommand]
        void Query()
        {
            Task.Run(() => {
                Translation = "";
                Dictionary<string, string> result = dataBaseService.QueryWord(Vocabulary).Result;
                IsWord = result["is_word"];
                Translation = result["translation"];
            });
        }

        [RelayCommand]
        async Task AddAsync()
        {
            await dataBaseService.AddWord(new() { Vocabulary = Vocabulary, Translation = Translation, Date = Convert.ToInt32(Date) });
            Vocabulary = "";
            Translation = "";
        }

        [RelayCommand]
        void VocabularyEntryFocused()
        {
            IsWord = "?";
        }

        public AddPageViewModel(DataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
        }
    }
}
