using System.ComponentModel.DataAnnotations;
using Web_Learning.Model;

public class User
{
    // Constructor to initialize UserRole collection to avoid null reference
    public User()
    {
        UserRole = new List<User_Role>(); // Initialize to an empty list
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<User_Role> UserRole { get; set; }

    // Method to check if the user has the "Admin" role
    public bool IsAdmin() => UserRole?.Any(ur => ur.Role.Name == "Admin") ?? false;
}