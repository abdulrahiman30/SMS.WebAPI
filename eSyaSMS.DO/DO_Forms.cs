using System;
using System.Collections.Generic;
using System.Text;

namespace HCP.SMS.DO
{
    public class DO_Forms
    {
        public int FormID { get; set; }
        public string FormName { get; set; }
    }

    public class DO_BusinessLocation
    {
        public string LocationDescription { get; set; }

        public int BusinessKey { get; set; }
    }
}
