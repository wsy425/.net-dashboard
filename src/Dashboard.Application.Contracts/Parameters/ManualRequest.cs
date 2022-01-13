using System.ComponentModel.DataAnnotations;
using Dashboard.ManualShared;

namespace Dashboard.Parameters
{
    public class ManualRequest
    {
        [Required]
        public Status State { get; set; }
        [Required]
        public Algorithm Name { get; set; }
    }
}