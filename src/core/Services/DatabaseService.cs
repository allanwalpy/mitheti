namespace Mitheti.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        // опять же, думаю код работающий с базой нужно обьединить
        public DatabaseContext GetContext() => new DatabaseContext();
    }
}
