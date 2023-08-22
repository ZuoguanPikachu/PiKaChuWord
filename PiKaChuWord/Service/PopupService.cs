using CommunityToolkit.Maui.Views;
using PiKaChuWord.View;

namespace PiKaChuWord.Service
{
    internal class PopupService
    {
        WordPopup wordPopup;
        public void ShowPopup()
        {
            wordPopup = new();
            Application.Current.MainPage.ShowPopup(wordPopup);
        }

        public void ClosePopup()
        {
            wordPopup.Close();
        }
    }
}
