namespace Mitheti.Core.Database
{
    public interface ISavingService
    {
        /// <summary>
        /// register delay milliseconds of <paramref name="info" /> process;
        /// </summary>
        void Add(AppTimeModel info);
    }
}
