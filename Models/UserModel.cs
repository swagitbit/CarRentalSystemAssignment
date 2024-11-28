using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAssignment.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage ="invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6,ErrorMessage ="Password must have at least 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("Admin|User",ErrorMessage ="Role must be admin or user.")]
        public string Role { get; set; } 
    }
}
