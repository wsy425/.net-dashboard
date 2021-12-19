using System.ComponentModel.DataAnnotations;
using Dashboard.Consts;
using Volo.Abp.Validation;

namespace Dashboard.Contracts
{
    public class UserRegisterDto
    {
        [Required]
        [DynamicStringLength(typeof(UserConst),nameof(UserConst.MaxNameLength))]
        public string Username { get; set; }
        [Required]
        [DynamicStringLength(typeof(UserConst),nameof(UserConst.MaxPasswordLength))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DynamicStringLength(typeof(UserConst),nameof(UserConst.MaxEmailLength))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}