using PiKaChuWord.ViewModel;

namespace PiKaChuWord.Service
{
    internal class ServiceLocator
    {
        private IServiceProvider _serviceProvider;
        public AddPageViewModel AddPageViewModel => _serviceProvider.GetService<AddPageViewModel>();
        public ListPageViewModel ListPageViewModel => _serviceProvider.GetService<ListPageViewModel>();
        public MemoryPageViewModel MemoryPageViewModel => _serviceProvider.GetService<MemoryPageViewModel>();
        public DataBaseService DataBaseService => _serviceProvider.GetService<DataBaseService>();

        public ServiceLocator()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AddPageViewModel>();
            serviceCollection.AddSingleton<ListPageViewModel>();
            serviceCollection.AddSingleton<MemoryPageViewModel>();
            serviceCollection.AddSingleton<DataBaseService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
