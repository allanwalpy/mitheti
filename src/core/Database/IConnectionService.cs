namespace Mitheti.Core.Database
{
    public interface IConnectionService
    {
        DatabaseContext Context { get; }
    }
}
