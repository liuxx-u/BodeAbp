
using Abp.EntityFramework;
using BodeAbp.Activity.Activities.Domain;

namespace BodeAbp.Activity.Activities.ModelConfigs
{
    public class ActivityConfiguration : EntityConfigurationBase<Domain.Activity, long>
    {
		public ActivityConfiguration()
        {
            //TODO：配置实体映射关系
        }
    }
}
