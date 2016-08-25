using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Organizations.Dtos;

namespace BodeAbp.Zero.Organizations
{
    /// <summary>
    /// 组织机构服务
    /// </summary>
    [Description("组织机构接口")]
    public interface IOrganizationAppService:IApplicationService
    {
        /// <summary>
        /// 获取 组织机构分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<OrganizationUnitDto>> GetOrganizationUnitPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取组织机构树数据
        /// </summary>
        /// <returns></returns>
        Task<List<TreeOutPut>> GetOrganizationUnitTreeData();

        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreatOrganizationUnit(OrganizationUnitDto input);

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateOrganizationUnit(OrganizationUnitDto input);

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <returns></returns>
        Task DeleteOrganizationUnit(List<IdInput> input);
    }
}
