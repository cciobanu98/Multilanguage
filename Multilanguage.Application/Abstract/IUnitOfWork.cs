using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IUnitOfWork
    {
       int SaveChanges(bool acceptAllChangesOnSuccess);
       int SaveChanges();
       Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
       Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

       DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
