﻿using System;
using System.Collections.Generic;

namespace HCP.SMS.DL.Entities
{
    public partial class GtEcsmsd
    {
        public string Smsid { get; set; }
        public int ParameterId { get; set; }
        public bool ParmAction { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcsmsh Sms { get; set; }
    }
}
