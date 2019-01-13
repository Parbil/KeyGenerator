using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitproKeyGen.Models
{
    public class KeyRequest
    {
        public int Id { get; set; }
        public String RequestedBy { get; set; }
        public String ShopName { get; set; }
        public String ShopContactNo { get; set; }
        public String ProductKey { get; set; }
        public String LicenseKey { get; set; }
        public int Duration { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public String Status { get; set; }

    }
}
