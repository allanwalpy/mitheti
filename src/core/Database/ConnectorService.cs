using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                    .UseMySql(this._connectionString)
                    .Options;

                return new Context(options);
            }
        }

        public ConnectionService(IConfiguration config)
        {
            #if DEBUG
                this._connectionString = config.GetConnectionString(ConnectionStringKey + "debug");
            #else
                this._connectionString = config.GetConnectionString(ConnectionStringName);
            #endif
        }
    }
}
