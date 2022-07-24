using Microsoft.AspNetCore.Mvc;
using TestTask.Services;

namespace TestTask.Controllers
{
    public class TransactionController : Controller
    {
        public ITransactionService TransactionService;
        public TransactionController(IServiceProvider _serviceProvider)
        {
            TransactionService = _serviceProvider.GetService<ITransactionService>();
        }
        [HttpPost]
        [Route("api/task")]
        public async Task<IActionResult> CreateTransaction()
        {
            var response = new OkObjectResult(await TransactionService.CreateTransaction());
            response.StatusCode = 202;
            return response;
        }

        [HttpGet]
        [Route("api/task/{id}")]
        public async Task<IActionResult> GetTransaction(string id)
        {
            var response = new OkObjectResult(new object { });
            try
            {
                response.Value = await TransactionService.GetTransactionById(id);
                response.StatusCode = response.Value == null ? 404 : 200;
                if(response.Value == null)
                    response.Value = $"Not found transaction with Id = {id}";
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Value = ex.Message;
            }
            return response;
        }
    }
}
