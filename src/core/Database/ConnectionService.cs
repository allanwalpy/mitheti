using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core.Database
{
    public class ConnectionService : IConnectionService
    {
        public const string ConnectionStringKey = "local";
        private readonly ILogger _logger;
        private string _connectionString;

        public DatabaseContext Context
        {
            get
            {
                _logger.LogTrace("getting database context with connection string is {0}", _connectionString);

                var builder = new DbContextOptionsBuilder();

                var options = builder
                    .UseMySQL(_connectionString)
                    .Options;

                return new DatabaseContext(options);
            }
        }

        public ConnectionService(ILogger logger, IConfiguration config)
        {
            #if DEBUG
                _connectionString = config.GetConnectionString(ConnectionStringKey + "debug");
            #else
                _connectionString = config.GetConnectionString(ConnectionStringName);
            #endif

            _logger = logger;
        }
    }
}
