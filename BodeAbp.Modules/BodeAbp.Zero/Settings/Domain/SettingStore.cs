using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace BodeAbp.Zero.Settings.Domain
{
    /// <summary>
    /// Implements <see cref="ISettingStore"/>.
    /// </summary>
    public class SettingStore : ISettingStore, ITransientDependency
    {
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingStore(
            IRepository<Setting, long> settingRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _settingRepository = settingRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task<List<SettingInfo>> GetAllListAsync(long? userId)
        {
            /* Combined SetTenantId and DisableFilter for backward compatibility.
             * SetTenantId switches database (for tenant) if needed.
             * DisableFilter and Where condition ensures to work even if tenantId is null for single db approach.
             */
            return (await _settingRepository.GetAllListAsync(s => s.UserId == userId))
                        .Select(s => s.ToSettingInfo())
                        .ToList();
        }

        [UnitOfWork]
        public virtual async Task<SettingInfo> GetSettingOrNullAsync(long? userId, string name)
        {
            return (await _settingRepository.FirstOrDefaultAsync(s => s.UserId == userId && s.Name == name)).ToSettingInfo();
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(SettingInfo settingInfo)
        {
            await _settingRepository.DeleteAsync(s => s.UserId == settingInfo.UserId && s.Name == settingInfo.Name);
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(SettingInfo settingInfo)
        {
            await _settingRepository.InsertAsync(settingInfo.ToSetting());
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task UpdateAsync(SettingInfo settingInfo)
        {
            var setting = await _settingRepository.FirstOrDefaultAsync(
                s => s.UserId == settingInfo.UserId &&
                     s.Name == settingInfo.Name
                );

            if (setting != null)
            {
                setting.Value = settingInfo.Value;
            }

            await _unitOfWorkManager.Current.SaveChangesAsync();
        }
    }
}