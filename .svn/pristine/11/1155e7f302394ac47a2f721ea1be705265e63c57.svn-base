using System;
using System.Collections.Generic;

namespace HCP.SMS.DL.Entities
{
    public partial class GtEcsmsh
    {
        public GtEcsmsh()
        {
            GtEcsmsd = new HashSet<GtEcsmsd>();
            GtEcsmsr = new HashSet<GtEcsmsr>();
        }

        public string Smsid { get; set; }
        public int FormId { get; set; }
        public string Smsdescription { get; set; }
        public bool IsVariable { get; set; }
        public int TeventId { get; set; }
        public string Smsstatement { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcsmsd> GtEcsmsd { get; set; }
        public virtual ICollection<GtEcsmsr> GtEcsmsr { get; set; }
    }
}
