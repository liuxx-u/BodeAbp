namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public interface IEntityChangeEventHelper
    {
        void TriggerEntityCreatingEvent(object entity);

        void TriggerEntityCreatedEventOnUowCompleted(object entity);

        void TriggerEntityUpdatingEvent(object entity);
        
        void TriggerEntityUpdatedEventOnUowCompleted(object entity);

        void TriggerEntityDeletingEvent(object entity);
        
        void TriggerEntityDeletedEventOnUowCompleted(object entity);
    }
}