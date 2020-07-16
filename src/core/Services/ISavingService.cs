using System;

namespace Mitheti.Core.Services
{
    public interface ISavingService
    {
        void Save(string info, int duration, DateTime timestamp);
    }
}