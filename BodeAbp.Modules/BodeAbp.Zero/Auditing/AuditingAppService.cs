using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Auditing.Domain;
using BodeAbp.Zero.Auditing.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BodeAbp.Zero.Auditing
{
	/// <summary>
    ///  审计日志 服务
    /// </summary>
    public class AuditingAppService : IAuditingAppService
    {
	    private readonly IRepository<AuditLog,long> _auditLogRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditLogRepository"></param>
        public AuditingAppService(IRepository<AuditLog, long> auditLogRepository)
        {
		    _auditLogRepository = auditLogRepository;
        }

		 /// <summary>
        /// 获取 审计日志分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultOutput<GetAuditLogListOutput>> GetAuditLogPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _auditLogRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetAuditLogListOutput>(total, list.MapTo<List<GetAuditLogListOutput>>());
        }
        
		/// <summary>
        /// 删除 审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAuditLog(List<IdInput<long>> input)
        {
			var ids = input.Select(p => p.Id);
            await _auditLogRepository.DeleteAsync(p => ids.Contains(p.Id));
        }
    }
}