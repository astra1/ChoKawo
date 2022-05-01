using ChoKawo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChoKawo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController > _logger;
        private readonly IConfiguration _configuration;

        public MailController(ILogger<MailController> logger, IConfiguration config) {
            _logger = logger;
            _configuration = config;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MailBody mailBody)
        {
            var mailService = new EmailService(_configuration);
            _logger.Log(LogLevel.Information, "New message");

            try
            {
                await mailService.SendEmailAsync(mailBody.Email, mailBody.Name, mailBody.Text);
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Information, "Failed to send a message: " + ex.Message);
                return NotFound();
            }
            return NoContent();
        }

        public class MailBody
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Text { get; set; }
        }
    }
}
