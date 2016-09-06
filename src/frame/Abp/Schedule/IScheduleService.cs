using Abp.Dependency;
using System;

namespace Abp.Schedule
{
    public interface IScheduleService : ITransientDependency
    {
        void StartTask(string name, Action action, int dueTime, int period);
        void StopTask(string name);
    }
}
