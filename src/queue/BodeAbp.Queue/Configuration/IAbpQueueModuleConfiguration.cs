namespace BodeAbp.Queue.Configuration
{
    public interface IAbpQueueModuleConfiguration
    {
        IAbpQueueModuleConfiguration InitQueue();

        IAbpQueueModuleConfiguration RegisterUnhandledExceptionHandler();
    }
}
