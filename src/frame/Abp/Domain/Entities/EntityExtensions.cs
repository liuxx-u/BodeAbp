using System;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// Some useful extension methods for Entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Check if this Entity is null of marked as deleted.
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        /// Undeletes this entity by setting <see cref="ISoftDelete.IsDeleted"/> to false and
        /// <see cref="IDeletionAudited"/> properties to null.
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;
            if (entity is IDeletionAudited)
            {
                var deletionAuditedEntity = entity.As<IDeletionAudited>();
                deletionAuditedEntity.DeletionTime = null;
                deletionAuditedEntity.DeleterUserId = null;
            }
        }

        /// <summary>
        /// 判断指定类型是否为<see cref="IEntity{TKey}"/>实体类型
        /// </summary>
        /// <param name="type">要判断的类型</param>
        /// <returns></returns>
        public static bool IsEntityType(this Type type)
        {
            return typeof(IEntity<>).IsGenericAssignableFrom(type) && !type.IsAbstract;
        }
    }
}