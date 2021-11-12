using EmailControlForClient.Infrastructure.Abstractions;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailControlForClient.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IOptions<MyConfig> _config;

        public MailController(IOptions<MyConfig> config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> LastClientMailAsync(DateTime dateAfter)
        {
            ImapClient client = new();
            await client.ConnectAsync(_config.Value.MailServiceHost, _config.Value.MailServiceHostPort, _config.Value.MailServiceHostSSL);
            await client.AuthenticateAsync(_config.Value.EmailServiceSourceMailUserName, _config.Value.EmailServiceSourceMailPassword);
            await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
            var search = SearchQuery.DeliveredAfter(dateAfter).And(SearchQuery.FromContains("@bebka.org.tr")).And(SearchQuery.NotSeen);
            var mailsFromClient = client.Inbox.Search(search);

            List<ResponseModel> result = new();
            foreach (var uniqueId in mailsFromClient)
            {
                ResponseModel res = new();
                var mail = client.Inbox.GetMessage(uid: uniqueId);
                res.By = mail.From.Select(x=>x.Name).ToArray();
                res.When = mail.Date.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
                res.Title = mail.Subject;
                result.Add(res);
            }
            return Ok(result);
        }
    }
    public class ResponseModel
    {
        public string Title { get; set; }
        public string[] By { get; set; }
        public string When { get; set; }
    }
}
