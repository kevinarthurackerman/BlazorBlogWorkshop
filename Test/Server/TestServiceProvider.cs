namespace BlazorBlogWorkshop.Server.Test
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class TestServiceProvider
    {
        private readonly IConfiguration _configuration;
        private IServiceProvider _serviceProvider;

        private TestServiceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static TestServiceProvider WithNoConfiguration() => WithConfiguration(null);

        public static TestServiceProvider WithConfiguration(Action<IConfigurationBuilder> configure)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configure?.Invoke(configurationBuilder);

            var configuration = configurationBuilder.Build();

            return new TestServiceProvider(configuration);
        }

        public TestServiceProvider WithServices(Action<IServiceCollection> configureServices) =>
            WithServices((sp, config) => configureServices(sp));

        public TestServiceProvider WithServices(Action<IServiceCollection, IConfiguration> configureServices)
        {
            var serviceCollection = new ServiceCollection();

            configureServices?.Invoke(serviceCollection, _configuration);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            return this;
        }

        public void ExecuteScope(Action<IServiceProvider> execute) =>
            execute(_serviceProvider.CreateScope().ServiceProvider);

        public Task ExecuteScopeAsync(Func<IServiceProvider, Task> execute) =>
            execute(_serviceProvider.CreateScope().ServiceProvider);
    }
}
