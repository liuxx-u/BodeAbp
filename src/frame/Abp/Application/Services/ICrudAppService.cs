using Abp.Application.Services.Dto;

namespace Abp.Application.Services
{
    public interface ICrudAppService<TEntityDto, TPrimaryKey> :
        ICrudAppService<TEntityDto, TPrimaryKey, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    { }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TOperableInput> :
        ICrudAppService<TEntityDto, TPrimaryKey, TOperableInput, TOperableInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TOperableInput : IEntityDto<TPrimaryKey>
    { }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        PagedResultDto<TEntityDto> GetPagedList(QueryListPagedRequestInput input);

        TEntityDto Create(TCreateInput input);

        TEntityDto Update(TUpdateInput input);

        void Delete(IdInput<TPrimaryKey> input);
    }
}
