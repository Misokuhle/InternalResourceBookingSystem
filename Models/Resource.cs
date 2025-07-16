using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalResourceBookingSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Resource Name")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity {  get; set; }

        public bool IsAvailable { get; set; } = true;

        //Navigation property: One resource can have many bookings
        public ICollection<Booking>? Bookings { get; set; }

    }
}
