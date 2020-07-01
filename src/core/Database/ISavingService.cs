namespace Mitheti.Core.Database
{
    public interface ISavingService
    {
        /// <summary>
        /// register delay milliseconds of <paramref name="info" /> process;
        /// </summary>
        public void AddRecordedTime(AppTimeModel info);
    }
}
