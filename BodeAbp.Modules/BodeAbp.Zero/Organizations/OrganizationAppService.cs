using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Organizations.Domain;
using BodeAbp.Zero.Organizations.Dtos;

namespace BodeAbp.Zero.Organizations
{
    /// <summary>
    /// 组织机构服务
    /// </summary>
    public class OrganizationAppService : ApplicationService, IOrganizationAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _ouRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrganizationAppService(OrganizationUnitManager organizationUnitManager, IRepository<OrganizationUnit, long> ouRepository)
        {
            _ouRepository = ouRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        /// <inheritdoc/>
        public async Task<PagedResultOutput<OrganizationUnitDto>> GetOrganizationUnitPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _ouRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<OrganizationUnitDto>(total, list.MapTo<List<OrganizationUnitDto>>());
        }

        /// <inheritdoc/>
        public async Task<List<TreeOutPut>> GetOrganizationUnitTreeData()
        {
            var ous = await _ouRepository.QueryWithNoTracking().Select(p => new OrganizationUnitDto
            {
                Id = p.Id,
                DisplayName = p.DisplayName,
                ParentId = p.ParentId
            }).ToListAsync();

            var result = ous.Where(p => p.ParentId == null).Select(p => new TreeOutPut
            {
                Value = p.Id + "",
                Text = p.DisplayName.ToString(),
                Children = getChildOus(p.Id, ous)
            }).ToList();

            return result;
        }

        /// <inheritdoc/>
        public async Task CreatOrganizationUnit(OrganizationUnitDto input)
        {
            await _organizationUnitManager.CreateAsync(input.MapTo<OrganizationUnit>());
        }

        /// <inheritdoc/>
        public async Task UpdateOrganizationUnit(OrganizationUnitDto input)
        {
            var ou = await _ouRepository.GetAsync(input.Id);
            input.MapTo(ou);
            await _organizationUnitManager.UpdateAsync(ou);
        }

        /// <inheritdoc/>
        public async Task DeleteOrganizationUnit(List<IdInput> input)
        {
            var ids = input.Select(p => p.Id);
            foreach (var id in ids)
            {
                await _organizationUnitManager.DeleteAsync(id);
            }
        }

        private List<TreeOutPut> getChildOus(long id, List<OrganizationUnitDto> ous)
        {
            if (ous.Any(p => p.Id == id))
            {
                var children = ous.Where(p => p.ParentId == id).Select(p => new TreeOutPut
                {
                    Value = p.Id + "",
                    Text = p.DisplayName.ToString(),
                    Children = getChildOus(p.Id, ous)
                }).ToList();
                return children;
            }
            return new List<TreeOutPut>();
        }
    }
}
