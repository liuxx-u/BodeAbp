using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Zero.Organizations.Domain;

namespace BodeAbp.Zero.Organizations.Dtos
{
    /// <summary>
    /// 组织机构Dto
    /// </summary>
    [AutoMap(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : EntityDto<long>, IDoubleWayDto
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
    }
}
