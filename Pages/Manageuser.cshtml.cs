using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Learning.Data;
using Web_Learning.Model;

namespace Web_Learning.Pages
{
    public class ManageuserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ManageuserModel> _logger;

        public ManageuserModel(ApplicationDbContext context, ILogger<ManageuserModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<User> Users { get; set; } = new List<User>();
        public IList<Role> Roles { get; set; } = new List<Role>();

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _context.User
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            Roles = await _context.Role.ToListAsync();
            return Page();
        }

        // ? Improved Update Role method
        public async Task<IActionResult> OnPostUpdateRoleAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserId == userId);

            if (userRole != null)
            {
                // Delete the existing role
                _context.UserRole.Remove(userRole);
                await _context.SaveChangesAsync(); // Commit the deletion
            }

            // Add the new role
            _context.UserRole.Add(new User_Role { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync(); // Commit the addition

            return RedirectToPage();
        }


        // ? Delete User
        public async Task<IActionResult> OnPostDeleteUserAsync(int userId)
        {
            var userRoles = _context.UserRole.Where(ur => ur.UserId == userId);
            _context.UserRole.RemoveRange(userRoles);

            var user = await _context.User.FindAsync(userId);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
