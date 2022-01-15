using System.ComponentModel.DataAnnotations;

namespace Dashboard.Parameters
{
    public class Config
    {
        [Required]
        public string ConfigName { get; set; }
        [Required]
        public string Content { get; set; }
    }
}