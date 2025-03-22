using System.ComponentModel.DataAnnotations;

namespace Web_Learning.Model
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        // New property for Image URL or file path
        [StringLength(500)]
        public string Image { get; set; }

        // New property for Course Description
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
