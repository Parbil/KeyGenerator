using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitproKeyGen.Models
{
    public class ApplicationRole :IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(String roleName) : base(roleName)
        {

        }
    }
}
