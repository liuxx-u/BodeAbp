using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.Chloe.Uow
{
    public class ChUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        //protected IDictionary<string, DbContext> ActiveDbContexts { get; private set; }

        protected IIocResolver IocResolver { get; private set; }

        public ChUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions) : base(defaultOptions)
        {
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void BeginUow()
        {
            throw new NotImplementedException();
        }

        protected override void CompleteUow()
        {
            throw new NotImplementedException();
        }

        protected override Task CompleteUowAsync()
        {
            throw new NotImplementedException();
        }

        protected override void DisposeUow()
        {
            throw new NotImplementedException();
        }
    }
}
