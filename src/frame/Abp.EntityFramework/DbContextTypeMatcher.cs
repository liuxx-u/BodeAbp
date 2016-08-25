using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.EntityFramework
{
    public class DbContextTypeMatcher : IDbContextTypeMatcher, ISingletonDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        public Dictionary<Type, List<Type>> _types;

        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _types = new Dictionary<Type, List<Type>>();
        }

        public void Add(Type sourceType, Type targetType)
        {
            if (!_types.ContainsKey(sourceType))
            {
                _types[sourceType] = new List<Type>();
            }

            _types[sourceType].Add(targetType);
        }

        public Type GetConcreteType(Type dbContextType)
        {
            //Get possible concrete types for given DbContext type
            var targetList = _types.GetOrDefault(dbContextType);

            if (targetList.IsNullOrEmpty())
            {
                //Not found any target type, return the given type if it's not abstract
                if (dbContextType.IsAbstract)
                {
                    throw new AbpException("Could not find a concrete implementation of given DbContext type: " + dbContextType.AssemblyQualifiedName);
                }

                return dbContextType;
            }

            if (targetList.Count == 1)
            {
                //Only one type does exists, return it
                return targetList[0];
            }

            //Will decide the target type with current UOW, so it should be in a UOW.
            if (_currentUnitOfWorkProvider.Current == null)
            {
                throw new AbpException("GetConcreteType method should be called in a UOW.");
            }

            if (targetList.Count < 1)
            {
                throw new AbpException(string.Format(
                    "Found more than one concrete type for given DbContext Type ({0})",
                    dbContextType
                    ));
            }
            if (targetList.Count > 1)
            {
                throw new AbpException(string.Format(
                    "Found more than one concrete type for given DbContext Type ({0}) ",
                    dbContextType
                    ));
            }

            return targetList[0];
        }
    }
}