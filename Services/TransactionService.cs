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
        public TransactionService(IServiceProvider _serviceProvider)
        {
            mapper = _serviceProvider.GetService<ITransactionMapper>();
            Context = _serviceProvider.GetService<ApplicationContext>();
        }
        public async Task<Guid> CreateTransaction()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;  
            try
            {
                var newTransaction = new Transaction()
                {
                    TransactionTime = DateTime.Now,
                    Status = TransactionStatus.Created
                };
                await Context.Transaction.AddAsync(newTransaction);
                await Context.SaveChangesAsync();
                newTransaction = await Context.Transaction.FirstOrDefaultAsync();
                Task task = Task.Run(async () =>
                {
                    await StatusChangingOn(TransactionStatus.Running, newTransaction.Id, token);
                });
                task.ContinueWith(task=>
                {
                    StatusChangingOn(TransactionStatus.Finished, newTransaction.Id, token);
                });
                return newTransaction.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task StatusChangingOn(TransactionStatus status, Guid id, CancellationToken token)
        {
            var currentTransaction = await Context.Transaction.FirstOrDefaultAsync(x => x.Id == id);
            var delay = 120000;
            try
            {
                if (status == TransactionStatus.Running)
                {
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"Transaction with id = {id} is created and will be running");
                }
                else
                {
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"Transaction with id = {id} is running and will be finished in {delay}ms");
                    Task.Delay(delay).Wait();
                    Console.WriteLine(DateTime.Now);
                }
                currentTransaction.Status = status;
                Context.Update(currentTransaction);
                await Context.SaveChangesAsync();
            }catch (Exception ex)
            {
                new CancellationTokenSource().Cancel();
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
