using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Dashboard.Contracts
{
    public class ForgetPasswordDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}