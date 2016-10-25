using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Application.Services.Query;
using System.Threading.Tasks;
using Abp.Linq;

namespace Abp.Application.Services
{
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TEntityDto, TEntityDto>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TOperableInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TOperableInput, TOperableInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
            where TOperableInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
            where TCreateInput : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetPagedList(QueryListPagedRequestInput input)
        {
            int total;
            QueryInputHandler(input);

            var query = _repository.GetAll().Where(input, out total);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<TEntityDto>(total, entities.Select(MapToEntityDto).ToList());
        }

        public virtual async Task<TEntityDto> Create(TCreateInput input)
        {
            var entity = MapToEntity(input);

            await _repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> Update(TUpdateInput input)
        {
            var entity = await _repository.GetAsync(input.Id);

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task Delete(IdInput<TPrimaryKey> input)
        {
            await _repository.DeleteAsync(input.Id);
        }
    }
}
