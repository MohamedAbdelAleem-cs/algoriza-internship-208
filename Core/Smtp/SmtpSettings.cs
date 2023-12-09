using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Smtp
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int port {  get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
    }
}
