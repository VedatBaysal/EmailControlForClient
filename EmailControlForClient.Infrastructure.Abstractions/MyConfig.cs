using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailControlForClient.Infrastructure.Abstractions
{
    public class MyConfig
    {
        public string EmailServiceSourceMailUserName { get; set; }
        public string EmailServiceSourceMailPassword { get; set; }
        public string MailServiceHost { get; set; }
        public int MailServiceHostPort { get; set; }
        public bool MailServiceHostSSL { get; set; }
    }
}
