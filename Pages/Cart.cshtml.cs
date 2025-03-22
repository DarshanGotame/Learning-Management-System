using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Learning.Data;
using Web_Learning.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web_Learning.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Cart> CartItems { get; set; } = new List<Cart>();
        public decimal TotalPrice { get; set; } = 0;

        // This method will load all cart items
        public async Task OnGetAsync()
        {
            CartItems = await _context.Cart.ToListAsync(); // Get all items in the cart
            TotalPrice = CartItems.Sum(item => item.Price);
        }

        // This method will clear the entire cart
        public async Task<IActionResult> OnPostBuyAsync()
        {
            // Clear all cart items
            _context.Cart.RemoveRange(_context.Cart);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Purchase successful! Your cart has been cleared.";
            return RedirectToPage();
        }

        // Method for removing an individual item from the cart
        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
        {
            var cartItem = await _context.Cart.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
