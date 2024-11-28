using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAssignment.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="car Make is required.")]
        public string Make { get; set; }
        [Required(ErrorMessage ="Car Model is required.")]
        public string Model { get; set; }
        [Required(ErrorMessage ="Year is required.")]
        [Range(1900,2100, ErrorMessage ="Year must be in the range 1900 to 2100")]
        public int Year { get; set; }
        [Required(ErrorMessage ="Price is required.")]
        [Range(0.01, 2000000000, ErrorMessage ="Price must be greater than zero.")]
        public decimal PricePerDay { get; set; }
        [Required(ErrorMessage ="availability must be added.")]
        public bool IsAvailable { get; set; } 
    }
}
