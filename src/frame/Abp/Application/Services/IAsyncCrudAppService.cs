using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Abp.Application.Services
{
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey> :
    IAsyncCrudAppService<TEntityDto, TPrimaryKey, TEntityDto, TEntityDto>
    where TEntityDto : IEntityDto<TPrimaryKey>
    { }

    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TOperableInput> :
        IAsyncCrudAppService<TEntityDto, TPrimaryKey, TOperableInput, TOperableInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TOperableInput : IEntityDto<TPrimaryKey>
    { }

    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        Task<PagedResultDto<TEntityDto>> GetPagedList(QueryListPagedRequestInput input);

        Task<TEntityDto> Create(TCreateInput input);

        Task<TEntityDto> Update(TUpdateInput input);

        Task Delete(IdInput<TPrimaryKey> input);
    }
}
