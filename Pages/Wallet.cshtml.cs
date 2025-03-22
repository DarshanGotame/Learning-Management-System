using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace Web_Learning.Pages
{
    public class WalletModel : PageModel
    {
        public decimal AvailableBalance { get; set; } = 0;

        [BindProperty]
        public decimal AmountToAdd { get; set; }

        public void OnGet()
        {
            var balance = HttpContext.Session.GetString("AvailableBalance");
            AvailableBalance = string.IsNullOrEmpty(balance) ? 0 : decimal.Parse(balance);
        }

        public IActionResult OnPostAddFunds()
        {
            var balance = HttpContext.Session.GetString("AvailableBalance");
            AvailableBalance = string.IsNullOrEmpty(balance) ? 0 : decimal.Parse(balance);

            if (AmountToAdd > 0)
            {
                AvailableBalance += AmountToAdd;
                HttpContext.Session.SetString("AvailableBalance", AvailableBalance.ToString());
            }

            return RedirectToPage(); // Refresh the wallet page
        }
    }
}
