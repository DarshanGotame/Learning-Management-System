using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Learning.Data;
using Web_Learning.Model;
using Microsoft.EntityFrameworkCore;

namespace Web_Learning.Pages
{
    public class ManageCoursesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageCoursesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task<IActionResult> OnGetAsync()
        {
            Courses = await _context.Course.ToListAsync();
            return Page();
        }

        // Add a new course
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newCourse = new Course
            {
                Title = Request.Form["NewCourse.Title"],
                Price = Convert.ToDecimal(Request.Form["NewCourse.Price"]),
                Description = Request.Form["NewCourse.Description"],
                Image = Request.Form["NewCourse.Image"]
            };

            _context.Course.Add(newCourse);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        // Delete an existing course
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            int courseId = Convert.ToInt32(Request.Form["DeleteCourseId"]);
            var courseToDelete = await _context.Course.FindAsync(courseId);

            if (courseToDelete != null)
            {
                _context.Course.Remove(courseToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // Edit an existing course
        public async Task<IActionResult> OnPostEditAsync()
        {
            int courseId = Convert.ToInt32(Request.Form["EditCourse.Id"]);
            var courseToUpdate = await _context.Course.FindAsync(courseId);

            if (courseToUpdate != null)
            {
                courseToUpdate.Title = Request.Form["EditCourse.Title"];
                courseToUpdate.Price = Convert.ToDecimal(Request.Form["EditCourse.Price"]);
                courseToUpdate.Description = Request.Form["EditCourse.Description"];
                courseToUpdate.Image = Request.Form["EditCourse.Image"];

                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
