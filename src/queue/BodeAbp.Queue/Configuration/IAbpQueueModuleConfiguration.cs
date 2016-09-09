namespace BodeAbp.Queue.Configuration
{
    public interface IAbpQueueModuleConfiguration
    {
        IAbpQueueModuleConfiguration InitQueue();

        IAbpQueueModuleConfiguration UseDeleteMessageByTimeStrategy(int maxStorageHours = 24 * 30);

        IAbpQueueModuleConfiguration UseDeleteMessageByCountStrategy(int maxChunkCount = 100);

        IAbpQueueModuleConfiguration UseQueueHashSelector();

        IAbpQueueModuleConfiguration UseQueueAverageSelector();
    }
}
