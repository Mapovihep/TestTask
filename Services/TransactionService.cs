using Microsoft.EntityFrameworkCore;
using TestTask.Context;
using TestTask.DTO;
using TestTask.Models;

namespace TestTask.Services
{
    public class TransactionService : ITransactionService
    {
        ApplicationContext Context;
        ITransactionMapper mapper;
        static SemaphoreSlim semaphore;
        public TransactionService(IServiceProvider _serviceProvider)
        {
            mapper = _serviceProvider.GetService<ITransactionMapper>();
            Context = _serviceProvider.GetService<ApplicationContext>();
        }
        public async Task<Guid> CreateTransaction()
        {
            if (semaphore==null || semaphore.CurrentCount < 1)
                semaphore = new SemaphoreSlim(0, 1);
            else
                semaphore.Wait();
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;  
            try
            {
                var newTransaction = new Transaction()
                {
                    TransactionTime = DateTime.Now,
                    Status = TransactionStatus.Created
                };
                semaphore.Release();
                await Context.Transaction.AddAsync(newTransaction);
                await Context.SaveChangesAsync();
                semaphore = new SemaphoreSlim(0, 1);
                Task task = Task.Run(async () =>
                {
                    await StatusChangingOn(TransactionStatus.Running, newTransaction, token, semaphore);
                });
                task.ContinueWith(task=>
                {
                    StatusChangingOn(TransactionStatus.Finished, newTransaction, token, semaphore);
                });
                return newTransaction.Id;
            }
            catch (AggregateException ex)
            {
                tokenSource.Cancel();
                foreach(var e in ex.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        throw e;
                }
                throw ex;
            }
        }

        public async Task StatusChangingOn(TransactionStatus status, Transaction newTransaction, CancellationToken token, SemaphoreSlim semaphore)
        {
            /*var currentTransaction = await Context.Transaction.FirstOrDefaultAsync(x => x.Id == id);*/
            var delay = 120000;
            try
            {
                if (status == TransactionStatus.Running)
                {
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"Transaction with id = {newTransaction.Id} is created and will be running");
                }
                else
                {
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"Transaction with id = {newTransaction.Id} is running and will be finished in {delay}ms");
                    
                    Task.Delay(delay).Wait();
                    Console.WriteLine(DateTime.Now);
                }
                newTransaction.Status = status;
                if (semaphore.CurrentCount == 1)
                    semaphore.Wait();
                semaphore.Release();
                Context.Update(newTransaction);
                await Context.SaveChangesAsync();
                semaphore = new SemaphoreSlim(0, 1);
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        throw e;
                }
                throw ex;
            }
        }
        public async Task<TransactionDto> GetTransactionById(string id)
        {
            try{
                var guidId = Guid.Parse(id);
                return mapper.GetMapperCfg().Map<Transaction, TransactionDto>(await Context.Transaction.FirstOrDefaultAsync(x => x.Id == guidId));
            }
            catch (FormatException ex)
            {
                throw ex;
            }
        }
    }
}
