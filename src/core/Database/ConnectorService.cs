using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Database
{
    public class ConnectionService
    {
        public const string ConnectionStringKey = "database";
        private string _connectionString;

        public Context Context
        {
            get
            {
                var builder = new DbContextOptionsBuilder<Context>();

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
