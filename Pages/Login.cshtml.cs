using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Learning.Data;
using Web_Learning.Model;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Learning.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the user exists
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == Username);

            if (user == null)
            {
                // Username not found
                ErrorMessage = "Username not found.";
                return Page();
            }

            // Compare the entered password with the stored plain text password
            if (user.Password != Password)
            {
                ErrorMessage = "Invalid password.";
                return Page();
            }

            // Get the user's role from the User_Role table
            var userRole = await _context.UserRole
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == user.Id);

            // If userRole is null, it means the user has no assigned role
            if (userRole == null)
            {
                ErrorMessage = "User has no role assigned.";
                return Page();
            }

            // Create claims for user roles
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, userRole.Role.Name)  // Set the user's role as a claim
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Remember the user (based on the "Remember Me" functionality)
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect based on role
            if (userRole.Role.Name == "Admin" || userRole.Role.Name == "Tutor")
            {
                return RedirectToPage("/Dashboard"); // Redirect Admin and Tutor to Dashboard
            }
            else
            {
                return RedirectToPage("/Courses"); // Redirect other users to Courses
            }

        }

    }

}

