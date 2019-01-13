using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BitproKeyGen.Areas.Identity.Pages.Account
{
    public class RedirectLoginModel : PageModel
    {
        public LocalRedirectResult OnGet(string returnUrl = null)
        {

            if (User.IsInRole("Admin"))
            {
                return LocalRedirect("~/KeyRequests/AdminIndex");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }
    }
}
