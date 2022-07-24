using TestTask.DTO;

namespace TestTask.Services
{
    public interface ITransactionService
    {
        public Task<Guid> CreateTransaction();
        public Task<TransactionDto> GetTransactionById(string id);
    }
}
