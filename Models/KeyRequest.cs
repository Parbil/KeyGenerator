using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitproKeyGen.Models
{
    public class KeyRequest
    {
        public int Id { get; set; }
        public String ProductKey { get; set; }
        public String LicenseKey { get; set; }
        public int Duration { get; set; }
    }
}
