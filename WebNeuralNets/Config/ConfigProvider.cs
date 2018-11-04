namespace GroupProjectBackend.Config
{
    using Microsoft.Extensions.Configuration;

    public class ConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _config;

        public ConfigProvider(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.GetSection("ConnectionString").Value;
    }
}
