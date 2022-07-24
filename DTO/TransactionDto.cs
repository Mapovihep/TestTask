using AutoMapper;
using TestTask.Models;

namespace TestTask.DTO
{
    public class TransactionDto
    {
        public string Status { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
