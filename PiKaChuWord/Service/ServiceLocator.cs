using PiKaChuWord.ViewModel;

namespace PiKaChuWord.Service
{
    internal class ServiceLocator
    {
        private IServiceProvider _serviceProvider;
        public AddPageViewModel AddPageViewModel => _serviceProvider.GetService<AddPageViewModel>();
        public ListPageViewModel ListPageViewModel => _serviceProvider.GetService<ListPageViewModel>();
        public MemoryPageViewModel MemoryPageViewModel => _serviceProvider.GetService<MemoryPageViewModel>();
        public ExportPageViewModel ExportPageViewModel => _serviceProvider.GetService<ExportPageViewModel>();
        public WordPopupViewModel WordPopupViewModel => _serviceProvider.GetService<WordPopupViewModel>();

        public DataBaseService DataBaseService => _serviceProvider.GetService<DataBaseService>();
        public PopupService PopupService => _serviceProvider.GetService<PopupService>();
        public WordQueryService WordQueryService => _serviceProvider.GetService<WordQueryService>();


        public ServiceLocator()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AddPageViewModel>();
            serviceCollection.AddSingleton<ListPageViewModel>();
            serviceCollection.AddSingleton<MemoryPageViewModel>();
            serviceCollection.AddSingleton<ExportPageViewModel>();
            serviceCollection.AddSingleton<WordPopupViewModel>();

            serviceCollection.AddSingleton<DataBaseService>();
            serviceCollection.AddSingleton<PopupService>();
            serviceCollection.AddSingleton<WordQueryService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
