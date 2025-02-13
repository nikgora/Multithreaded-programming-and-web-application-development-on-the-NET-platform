using System.ComponentModel.DataAnnotations;

namespace lab7.Models
{
    public class IntegrationModel
    {
        [Required]
        [Display(Name = "Lower Limit (A)")]
        public double LowerLimit { get; set; }

        [Required]
        [Display(Name = "Upper Limit (B)")]
        public double UpperLimit { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Subintervals must be at least 1")]
        [Display(Name = "Number of Subintervals (N)")]
        public int SubIntervals { get; set; }


        public double Result { get; set; }
    }
}