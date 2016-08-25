using Abp.Domain.Entities;
using Abp.EntityFramework.Initialize;
using Abp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.EntityFramework.Repositories;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 数据上下文管理器
    /// </summary>
    public sealed class DbContextManager
    {
        private static readonly Lazy<DbContextManager> InstanceLazy = new Lazy<DbContextManager>(() => new DbContextManager());

        /// <summary>
        /// 上下文类型-上下文初始化类型
        /// </summary>
        private readonly IDictionary<Type, DbContextInitializerBase> _contextInitializerDict
            = new Dictionary<Type, DbContextInitializerBase>();
        private readonly ConcurrentDictionary<Type, Type> _entityContextCache = new ConcurrentDictionary<Type, Type>();
        
        /// <summary>
        /// 获取 上下文管理器的唯一实例
        /// </summary>
        public static DbContextManager Instance { get { return InstanceLazy.Value; } }
        
        /// <summary>
        /// 注册上下文初始化器
        /// </summary>
        /// <param name="contextType">上下文类型</param>
        /// <param name="initializer">上下文初始化器</param>
        public void RegisterInitializer(Type contextType, DbContextInitializerBase initializer)
        {
            if (_contextInitializerDict.ContainsKey(contextType))
            {
                return;
            }
            _contextInitializerDict[contextType] = initializer;
        }
        
        /// <summary>
        /// 获取实体类型对应的上下文类型
        /// </summary>
        public Type GetDbContexType(Type entityType)
        {
            if (!typeof(IEntity<>).IsGenericAssignableFrom(entityType))
            {
                throw new InvalidOperationException(string.Format("类型“{0}”不是实体类型", entityType.FullName));
            }
            Type contextType = null;
            if (_entityContextCache.ContainsKey(entityType))
            {
                contextType = _entityContextCache[entityType];
            }
            else
            {
                foreach (var item in _contextInitializerDict.Where(item => item.Value.EntityMappers.ContainsKey(entityType)))
                {
                    contextType = item.Key;
                    _entityContextCache[entityType] = contextType;
                    break;
                }
            }
            if (contextType == null)
            {
                throw new InvalidOperationException(string.Format("实体类型“{0}”未映射到上下文中，请为实体派生 EntityConfigurationBase<> 类型，使之能被数据上下文加载。",entityType.FullName));
            }
            return contextType;
        }

        /// <summary>
        /// 获取 实体映射集合
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns>实体集合</returns>
        public IEnumerable<IEntityMapper> GetEntityMappers(Type dbContextType)
        {
            if (!typeof(AbpDbContext).IsAssignableFrom(dbContextType))
            {
                throw new InvalidOperationException(string.Format("类型“{0}”不是 AbpDbContext 类型的派生类", dbContextType.FullName));
            }

            DbContextInitializerBase initializer;
            if (_contextInitializerDict.TryGetValue(dbContextType, out initializer))
            {
                return initializer.EntityMappers.Values;
            }
            return new List<IEntityMapper>();
        }

        /// <summary>
        /// 初始化所有DbContext
        /// </summary>
        public void Initialize()
        {
            foreach (var item in _contextInitializerDict)
            {
                item.Value.Initialize();
                RegisterForDbContext(item);
            }
        }

        /// <summary>
        /// 注入所有的IRepository
        /// </summary>
        /// <param name="item">DbContext类型与对应的初始化类</param>
        private void RegisterForDbContext(KeyValuePair<Type, DbContextInitializerBase> item)
        {
            var iocManager = IocManager.Instance;
            var autoRepositoryAttr = item.Key.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ?? AutoRepositoryTypesAttribute.Default;
            foreach (var entityMapper in item.Value.EntityMappers)
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityMapper.Key);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityMapper.Key);
                    if (!iocManager.IsRegistered(genericRepositoryType))
                    {
                        var implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                                ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityMapper.Key)
                                : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(item.Key, entityMapper.Key);

                        iocManager.Register(genericRepositoryType, implType, DependencyLifeStyle.Transient);
                    }
                }

                var genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityMapper.Key, primaryKeyType);
                if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                {
                    var implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                                ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityMapper.Key, primaryKeyType)
                                : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(item.Key, entityMapper.Key, primaryKeyType);

                    iocManager.Register(genericRepositoryTypeWithPrimaryKey, implType, DependencyLifeStyle.Transient);
                }
            }
        }
    }
}