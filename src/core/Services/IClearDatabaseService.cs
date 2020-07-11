using System;

namespace Mitheti.Core.Services
{
    public interface IClearDatabaseService
    {
        /// <summary>
        /// Clear records with timestamp between <paramref name="laterThen"/> and <paramref name="beforeThen"/>
        /// </summary>
        /// <param name="laterThen">lower boundary</param>
        /// <param name="beforeThen">upper boundary</param>
        public void Clear(DateTime laterThen, DateTime beforeThen);

        /// <summary>
        /// Clear records with timestamp between <see cref="DateTime.MinValue"/> and <paramref name="beforeThen"/>
        /// </summary>
        /// <param name="beforeThen">upper boundary</param>
        public void Clear(DateTime beforeThen);
    }
}