namespace Mitheti.Core.Services
{
    public interface ISizeLimitDatabaseService
    {
        void LimitDatabase();

        long GetSizeMb();
    }
}