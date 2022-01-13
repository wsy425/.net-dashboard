using System.ComponentModel.DataAnnotations;
using Dashboard.Manual;

namespace Dashboard.Parameters
{
    public class ManualRequest
    {
        [Required]
        public Status State { get; set; }
        [Required]
        public string Name { get; set; }
    }
}