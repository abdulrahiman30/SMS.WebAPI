using System;
using System.Collections.Generic;
using System.Text;

namespace HCP.SMS.DO
{
    public class DO_SMSVariable
    {
        public string Smsvariable { get; set; }
        public string Smscomponent { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
