namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// to pass an Id value to an application service method.
    /// </summary>
    /// <typeparam name="TId">Type of the Id</typeparam>
    public class IdInput<TId>
    {
        public TId Id { get; set; }

        public IdInput()
        {

        }

        public IdInput(TId id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// A shortcut of <see cref="IdInput{TPrimaryKey}"/> for <see cref="int"/>.
    /// </summary>
    public class IdInput : IdInput<int>
    {
        public IdInput()
        {

        }

        public IdInput(int id)
            : base(id)
        {

        }
    }
}