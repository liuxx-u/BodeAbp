using System;
using System.Data.Entity;
using Abp.Domain.Uow;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="EfUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <param name="unitOfWork">Current (active) unit of work</param>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is EfUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfUnitOfWork).FullName, "unitOfWork");
            }

            return (unitOfWork as EfUnitOfWork).GetOrCreateDbContext<TDbContext>();
        }
    }
}