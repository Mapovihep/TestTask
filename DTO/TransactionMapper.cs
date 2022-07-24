using AutoMapper;
using TestTask.Models;

namespace TestTask.DTO
{
    public class TransactionMapper : ITransactionMapper
    {
        public IMapper GetMapperCfg()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionDto>();
            });

            return config.CreateMapper();
        }
    }
    public interface ITransactionMapper
    {
        public IMapper GetMapperCfg();
    }
}
