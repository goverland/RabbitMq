using System.Text;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RabbitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public RabbitController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            _messageService.SendMessage(message: value);

            return Ok();
        }
    }
}
