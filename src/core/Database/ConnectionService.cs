using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Database
{
    public class ConnectionService
    {
        public const string ConnectionStringKey = "local";
        private string _connectionString;

        public Context Context
        {
            get
            {
                var builder = new DbContextOptionsBuilder();

                var options = builder
                    .UseMySQL(_connectionString)
                    .Options;

                return new Context(options);
            }
        }

        public ConnectionService(IConfiguration config)
        {
            #if DEBUG
                _connectionString = config.GetConnectionString(ConnectionStringKey + "debug");
            #else
                _connectionString = config.GetConnectionString(ConnectionStringName);
            #endif
        }
    }
}
