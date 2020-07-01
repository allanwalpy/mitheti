namespace Mitheti.Core.Database
{
    public interface ISavingService
    {
        /// <summary>
        /// register delay milliseconds of <paramref name="info" /> process;
        /// <para> delay fetched from watcher configs;</para>
        /// </summary>
        public void AddRecordedTime(AppTimeModel info);
    }
}
