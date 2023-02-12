using MailSender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<IActionResult> SendMail(string email,string header,string subject)
        {
            await _mailService.SendMailAsync(email, header, subject);
            return Ok();
        }
    }
}
