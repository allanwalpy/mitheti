using System;

namespace Mitheti.Core.Services
{
    //TODO:add stop and start;
    public interface ISavingService
    {
        void Save(string info, int duration, DateTime timestamp);
    }
}