using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Learning.Data;
using Web_Learning.Model;

namespace Web_Learning.Pages
{
    public class SignupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SignupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Initialize page
        }

        public IActionResult OnPost()
        {
            // Validate if passwords match
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            // Check if email already exists in the database
            var existingUser = _context.User.FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                ErrorMessage = "Email is already registered.";
                return Page();
            }

            // Create a new user (no password hashing here)
            var newUser = new User
            {
                Username = Username,
                Email = Email,
                Password = Password // Store the password as plain text (not recommended for production)
            };

            // Add new user to the database
            _context.User.Add(newUser);
            _context.SaveChanges();

            // Optionally, assign a default role (User role)
            var role = _context.Role.FirstOrDefault(r => r.Name == "User"); // Assuming "User" role exists
            if (role != null)
            {
                var userRole = new User_Role
                {
                    UserId = newUser.Id,
                    RoleId = role.Id
                };

                _context.UserRole.Add(userRole);
                _context.SaveChanges();
            }

            // Use TempData to send a success message to the Login page
            TempData["SuccessMessage"] = "Signup successful! Please login to your account.";

            // Redirect to the login page after successful signup
            return RedirectToPage("/Login");
        }
    }
}
