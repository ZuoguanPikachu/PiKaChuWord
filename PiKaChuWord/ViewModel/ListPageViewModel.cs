using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using PiKaChuWord.Model;
using PiKaChuWord.Service;
using System.Collections.ObjectModel;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Xml.Linq;

namespace PiKaChuWord.ViewModel
{
    internal partial class ListPageViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<bool>>
    {
        private DataBaseService dataBaseService;
        private PopupService popupService;

        [ObservableProperty]
        ObservableCollection<Word> wordList = new();

        [ObservableProperty]
        bool isRefreshing = false;

        [RelayCommand]
        void Refresh()
        {
            Load();
            IsRefreshing = false;
        }

        [RelayCommand]
        void Popup(Word word)
        {
            popupService.ShowPopup();
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Word>(word));
        }

        async void Load()
        {
            List<Word> words = await dataBaseService.GetList();
            WordList = words.OrderByDescending(item => item.Date).ToObservableCollection();
        }

        public void Receive(ValueChangedMessage<bool> isChanged)
        {
            if (isChanged.Value)
            {
                Load();
            }
        }

        public ListPageViewModel(DataBaseService dataBaseService, PopupService popupService)
        {
            IsActive = true;
            this.dataBaseService = dataBaseService;
            this.popupService = popupService;
            Load();
        }
    }
}
