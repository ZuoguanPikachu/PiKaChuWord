using PiKaChuWord.View;
namespace PiKaChuWord;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage());
	}
}
