using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Web_Learning.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public string Role { get; set; }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Role = User.FindFirstValue(ClaimTypes.Role);
            }
        }
    }
}
