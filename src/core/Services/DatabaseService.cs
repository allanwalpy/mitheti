namespace Mitheti.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        public DatabaseContext GetContext() => new DatabaseContext();
    }
}