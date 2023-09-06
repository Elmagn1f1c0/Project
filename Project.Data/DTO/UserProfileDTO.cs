using System.ComponentModel.DataAnnotations;

namespace Project.Data.DTO
{
    public class UserProfileDTO
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
    }

}
