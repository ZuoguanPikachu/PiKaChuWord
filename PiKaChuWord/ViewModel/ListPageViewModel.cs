using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PiKaChuWord.Model;
using PiKaChuWord.Service;
using System.Collections.ObjectModel;

namespace PiKaChuWord.ViewModel
{
    internal partial class ListPageViewModel : ObservableObject
    {
        private DataBaseService dataBaseService;

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

        async void Load()
        {
            List<Word> words = await dataBaseService.GetList();
            WordList = words.OrderByDescending(item => item.Date).ToObservableCollection();
        }

        public ListPageViewModel(DataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
            Load();
        }
    }
}
