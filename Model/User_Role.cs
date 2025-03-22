using System.ComponentModel.DataAnnotations;
using Web_Learning.Model;

namespace Web_Learning.Model
{
    public class User_Role
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

}
