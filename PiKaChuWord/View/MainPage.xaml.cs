namespace PiKaChuWord.View;

public partial class MainPage : TabbedPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		VocabularyEntry.Focus();
    }
}