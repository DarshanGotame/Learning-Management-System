using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Learning.Data;
using Web_Learning.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace Web_Learning.Pages
{
    public class CoursesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CoursesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task OnGetAsync()
        {
            // Retrieve courses from the database
            Courses = await _context.Course.ToListAsync();
        }

        // Add a course to the cart
        public async Task<IActionResult> OnPostAddToCartAsync(int courseId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // If not authenticated, redirect to the login page
                return RedirectToPage("/Login"); // Update the redirect page as per your login path
            }

            var course = await _context.Course.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Check if the course is already in the cart
            var existingCartItem = await _context.Cart.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (existingCartItem == null)
            {
                var cartItem = new Cart
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Price = course.Price,
                    Image = course.Image
                };

                _context.Cart.Add(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Courses");
        }
    }
}
