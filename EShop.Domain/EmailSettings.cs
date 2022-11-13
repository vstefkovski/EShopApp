using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Domain
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public int SmtpServerPort { get; set; }
        public bool EnableSsl { get; set; }
        public string DisplayName { get; set; }
        public string SenderName { get; set; }

        public EmailSettings()
        {

        }

        public EmailSettings(string SmtpServer, string SmtpUserName, string SmtpPassword, int SmtpServerPort)
        {
            this.SmtpServer = SmtpServer;
            this.SmtpUserName = SmtpUserName;
            this.SmtpPassword = SmtpPassword;
            this.SmtpServerPort = SmtpServerPort;
        }

    }
}
