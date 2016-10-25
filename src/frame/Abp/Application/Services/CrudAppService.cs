using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Application.Services.Query;

namespace Abp.Application.Services
{
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TEntityDto, TEntityDto>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TOperableInput>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TOperableInput, TOperableInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
            where TOperableInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>,
        ICrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
            where TCreateInput : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }

        public virtual PagedResultDto<TEntityDto> GetPagedList(QueryListPagedRequestInput input)
        {
            int total;
            QueryInputHandler(input);
            var list = _repository.GetAll().Where(input, out total).ToList();
            return new PagedResultDto<TEntityDto>(total, list.Select(MapToEntityDto).ToList());
        }

        public virtual TEntityDto Create(TCreateInput input)
        {
            var entity = MapToEntity(input);

            _repository.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public virtual TEntityDto Update(TUpdateInput input)
        {
            var entity = _repository.Get(input.Id);

            MapToEntity(input, entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public virtual void Delete(IdInput<TPrimaryKey> input)
        {
            _repository.Delete(input.Id);
        }
    }
}
