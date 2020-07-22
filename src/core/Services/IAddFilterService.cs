namespace Mitheti.Core.Services
{
    //TODO: add ignore filter as well, or change filtering;
    public interface IAddFilterService
    {
        bool HavePassed(string app);
    }
}