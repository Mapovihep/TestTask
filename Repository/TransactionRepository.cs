using Microsoft.EntityFrameworkCore;
using TestTask.Context;
using TestTask.Models;

namespace TestTask.Repository
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private ApplicationContext Context;
        private static SemaphoreSlim semaphore;
        public TransactionRepository(IServiceProvider _serviceProvider)
        {
            Context = _serviceProvider.GetRequiredService<ApplicationContext>();
        }
        public async Task CheckSemaphore()
        {
            if (semaphore == null)
                semaphore = new SemaphoreSlim(0, 1);
            else if(semaphore.CurrentCount < 1)
                semaphore.Release();
            else
                semaphore.Wait();
        }
        public virtual async Task<Guid> Add(Transaction obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            await CheckSemaphore();
            await Context.Transaction.AddAsync(obj);
            await Context.SaveChangesAsync();
            semaphore = new SemaphoreSlim(0, 1);
            return obj.Id;
        }
        public virtual async Task<Transaction> GetById(Guid id)
        {
            await CheckSemaphore();
            var obj = await Context.Transaction.FirstOrDefaultAsync(x => x.Id == id);
            semaphore = new SemaphoreSlim(0,1);
            return obj;
        }
        public virtual async Task Update(Transaction obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            await CheckSemaphore();
            Context.Transaction.Update(obj);
            await Context.SaveChangesAsync();
            semaphore = new SemaphoreSlim(0,1);
        }
        public virtual async Task Delete(Transaction obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            await CheckSemaphore();
            Context.Entry(obj).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
            semaphore = new SemaphoreSlim(0, 1);
        }
    }
}
