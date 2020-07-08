using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Database
{
    public class ConnectionService : IConnectionService
    {
        public const string ConnectionStringKey = "local";

        public DatabaseContext Context
        {
            get
            {
                var builder = new DbContextOptionsBuilder();

                return new DatabaseContext(builder.Options);
            }
        }

        public ConnectionService()
        {   }
    }
}
