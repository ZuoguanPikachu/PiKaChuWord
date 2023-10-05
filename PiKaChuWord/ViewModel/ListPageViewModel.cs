using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using PiKaChuWord.Model;
using PiKaChuWord.Service;
using System.Collections.ObjectModel;


namespace PiKaChuWord.ViewModel
{
    internal partial class ListPageViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<bool>>
    {
        private DataBaseService dataBaseService;
        private PopupService popupService;
        private List<Word> words;

        [ObservableProperty]
        ObservableCollection<Word> wordList = new();

        [ObservableProperty]
        bool isRefreshing = false;

        [ObservableProperty]
        string query = "";

        [RelayCommand]
        void Refresh()
        {
            Query = "";
            Task.Run(Load);
            IsRefreshing = false;
        }

        [RelayCommand]
        void Popup(Word word)
        {
            popupService.ShowPopup();
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Word>(word));
        }

        [RelayCommand]
        void Search()
        {
            if(words != null)
            {
                if (Query == "")
                {
                    if(words.Count != WordList.Count)
                    {
                        WordList = words.OrderByDescending(item => item.Date).ToObservableCollection();
                    }
                }
                else
                {
                    WordList = words
                        .Where(item => item.Vocabulary.Contains(Query) || item.Translation.Contains(Query))
                        .OrderByDescending(item => item.Date)
                        .ToObservableCollection();
                }
            }
        }

        async Task Load()
        {
            words = await dataBaseService.GetList();
            if (Query == "")
            {
                WordList = words.OrderByDescending(item => item.Date).ToObservableCollection();
            }
        }

        public async void Receive(ValueChangedMessage<bool> isChanged)
        {
            if (isChanged.Value)
            {
                await Load();
                Search();
            }
        }

        public ListPageViewModel(DataBaseService dataBaseService, PopupService popupService)
        {
            IsActive = true;
            this.dataBaseService = dataBaseService;
            this.popupService = popupService;
            Task.Run(Load);
        }
    }
}
