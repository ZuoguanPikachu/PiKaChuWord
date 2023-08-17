using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PiKaChuWord.Model;
using PiKaChuWord.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiKaChuWord.ViewModel
{
    internal partial class ExportPageViewModel : ObservableObject
    {
        private DataBaseService dataBaseService;

        [RelayCommand]
        async void ExportToClipBoard()
        {
            string content = "";
            foreach (Word word in await dataBaseService.GetList())
            {
                content += $"{word.Vocabulary}\t{word.Translation}\n";
            }
            await Clipboard.Default.SetTextAsync(content);
        }

        public ExportPageViewModel(DataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
        }
    }
}
