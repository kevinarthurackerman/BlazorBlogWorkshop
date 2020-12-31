namespace BlazorBlogWorkshop.Server.Test
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;
    using System.Collections.Generic;

    public static class ConfigurationExtensions
    {
        public static void AddDefaultConnectionString(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new MemoryConfigurationSource
            {
                InitialData = new[]
                {
                    KeyValuePair.Create(
                        "ConnectionStrings:App",
                        "Data Source=localhost;Initial Catalog=BlazorBlogWorkshop;Integrated Security=True"
                    )
                }
            });
        }
    }
}
