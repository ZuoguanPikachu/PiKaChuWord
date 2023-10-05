using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using PiKaChuWord.Service;

namespace PiKaChuWord.ViewModel
{
    internal partial class AddPageViewModel : ObservableObject
    {
        private DataBaseService dataBaseService;
        private WordQueryService wordQueryService;

        [ObservableProperty]
        string vocabulary;

        [ObservableProperty]
        string translation;

        [ObservableProperty]
        string date = DateTime.Now.ToString("yyyyMMdd");

        [ObservableProperty]
        Color textcolor = Colors.Black;

        [ObservableProperty]
        string wordStatus = "-";

        [RelayCommand]
        void Query()
        {
            Task.Run(() => {
                Translation = "";
                Dictionary<string, string> result = wordQueryService.QueryWord(Vocabulary).Result;
                WordStatus = result["word_status"];
                Translation = result["translation"];
            });
        }

        [RelayCommand]
        async Task AddAsync()
        {
            await dataBaseService.AddWord(new() { Vocabulary = Vocabulary, Translation = Translation, Date = Convert.ToInt32(Date) });
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<bool>(true));
            Vocabulary = "";
            Translation = "";
        }

        [RelayCommand]
        void VocabularyEntryFocused()
        {
            WordStatus = "-";
        }

        public AddPageViewModel(DataBaseService dataBaseService, WordQueryService wordQueryService)
        {
            this.dataBaseService = dataBaseService;
            this.wordQueryService = wordQueryService;
        }
    }
}
