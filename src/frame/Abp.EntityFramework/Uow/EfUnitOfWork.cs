using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFramework.Utils;
using Abp.Reflection;
using Castle.Core.Internal;
using System.Collections.Immutable;
using EntityFramework.DynamicFilters;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        protected IDictionary<string, DbContext> ActiveDbContexts { get; private set; }

        protected IIocResolver IocResolver { get; private set; }

        protected TransactionScope CurrentTransaction;
        private readonly IDbContextResolver _dbContextResolver;

        /// <summary>
        /// Creates a new <see cref="EfUnitOfWork"/>.
        /// </summary>
        public EfUnitOfWork(
            IIocResolver iocResolver,
            IDbContextResolver dbContextResolver,
            IEfUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions,
            IDbContextTypeMatcher dbContextTypeMatcher)
            : base(
                  defaultOptions,
                  filterExecuter)
        {
            IocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = Options.IsolationLevel.GetValueOrDefault(IsolationLevel.ReadUncommitted),
                };

                if (Options.Timeout.HasValue)
                {
                    transactionOptions.Timeout = Options.Timeout.Value;
                }

                CurrentTransaction = new TransactionScope(
                    Options.Scope.GetValueOrDefault(TransactionScopeOption.Required),
                    transactionOptions,
                    Options.AsyncFlowOption.GetValueOrDefault(TransactionScopeAsyncFlowOption.Enabled)
                    );
            }
        }

        public override void SaveChanges()
        {
            ActiveDbContexts.Values.ForEach(SaveChangesInDbContext);
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }
        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Complete();
            }

            DisposeUow();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Complete();
            }

            DisposeUow();
        }

        protected override void ApplyDisableFilter(string filterName)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                activeDbContext.DisableFilter(filterName);
            }
        }

        protected override void ApplyEnableFilter(string filterName)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                activeDbContext.EnableFilter(filterName);
            }
        }

        protected override void ApplyFilterParameterValue(string filterName, string parameterName, object value)
        {
            foreach (var activeDbContext in ActiveDbContexts.Values)
            {
                if (TypeHelper.IsFunc<object>(value))
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, (Func<object>)value);
                }
                else
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, value);
                }
            }
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            var dbContextKey = typeof (TDbContext).FullName;

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {

                dbContext = _dbContextResolver.Resolve<TDbContext>();
                ((IObjectContextAdapter)dbContext).ObjectContext.ObjectMaterialized += ObjectContext_ObjectMaterialized;

                foreach (var filter in Filters)
                {
                    if (filter.IsEnabled)
                    {
                        dbContext.EnableFilter(filter.FilterName);
                    }
                    else
                    {
                        dbContext.DisableFilter(filter.FilterName);
                    }

                    foreach (var filterParameter in filter.FilterParameters)
                    {
                        if (TypeHelper.IsFunc<object>(filterParameter.Value))
                        {
                            dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, (Func<object>)filterParameter.Value);
                        }
                        else
                        {
                            dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, filterParameter.Value);
                        }
                    }
                }

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }

        protected override void DisposeUow()
        {
            ActiveDbContexts.Values.ForEach(Release);
            ActiveDbContexts.Clear();

            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }

        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }
        
        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
            IocResolver.Release(dbContext);
        }

        private static void ObjectContext_ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var entityType = ObjectContext.GetObjectType(e.Entity.GetType());
            DateTimePropertyInfoHelper.NormalizeDatePropertyKinds(e.Entity,entityType);
        }
    }
}