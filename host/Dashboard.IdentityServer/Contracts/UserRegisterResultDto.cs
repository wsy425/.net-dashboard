using System;

namespace Dashboard.Contracts
{
    public class UserRegisterResultDto : BaseUserResultDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null;
        public string Email { get; set; } = null;
    }
}