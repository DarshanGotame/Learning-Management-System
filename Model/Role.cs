using System.ComponentModel.DataAnnotations;
using Web_Learning.Model;

namespace Web_Learning.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<User_Role> UserRole { get; set; }
    }

}

