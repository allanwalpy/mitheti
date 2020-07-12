namespace Mitheti.Core.Services
{
    public interface IDatabaseService
    {
        DatabaseContext GetContext();
    }
}