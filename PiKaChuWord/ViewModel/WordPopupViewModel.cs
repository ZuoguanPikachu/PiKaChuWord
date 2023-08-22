using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using PiKaChuWord.Model;
using PiKaChuWord.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiKaChuWord.ViewModel
{
    internal partial class WordPopupViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<Word>>
    {
        private PopupService popupService;
        private DataBaseService dataBaseService;

        [ObservableProperty]
        public Word word;

        public WordPopupViewModel(DataBaseService dataBaseService, PopupService popupService) 
        {
            this.dataBaseService = dataBaseService;
            this.popupService = popupService;
            IsActive = true;
        }

        public void Receive(ValueChangedMessage<Word> word)
        {
            Word = word.Value;
        }

        [RelayCommand]
        async Task Update()
        {
            await dataBaseService.AddWord(Word);
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<bool>(true));
            popupService.ClosePopup();
        }

        [RelayCommand]
        void Cancel()
        {
            popupService.ClosePopup();
        }

        [RelayCommand]
        async Task Delete() 
        {
            await dataBaseService.DeleteWord(Word);
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<bool>(true));
            popupService.ClosePopup();
        }
    }
}
