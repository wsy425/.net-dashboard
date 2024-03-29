﻿using System;
using System.Threading.Tasks;
using Dashboard.Contracts;
using Dashboard.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Dashboard.Controllers
{
    [Route("api/app")]
    public class AppController : AbpController
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AppController(
            IIdentityUserRepository userRepository, 
            IdentityUserManager userManager, 
            IGuidGenerator guidGenerator)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(DashboardResource);
        }

        [HttpPost]
        [Route("register")]
        public async Task<UserRegisterResultDto> UserRegisterAsync([FromBody]UserRegisterDto register)
        {
            var nameResult = await _userManager.FindByNameAsync(register.Username);
            if (nameResult != null)
            {
                return new UserRegisterResultDto
                {
                    StatusCode = 500,
                    Message = L["User:Existed"]
                };
            }

            var newUser = new IdentityUser(_guidGenerator.Create(), register.Username, register.Email);

            var userResult = await _userManager.CreateAsync(newUser,register.Password);
            if (!userResult.Succeeded)
            {
                return new UserRegisterResultDto
                {
                    StatusCode = 500,
                    Message = L["User:CreateFailed",userResult.ToString().Split(":")[1].TrimStart()]
                };
            }
            await _userManager.AddDefaultRolesAsync(newUser);
            return new UserRegisterResultDto
            {
                Id = newUser.Id,
                Username = newUser.UserName,
                Email = newUser.Email,
                StatusCode = 200,
                Message = L["User:CreateSuccess"]
            };
        }
        
        [HttpPost("reset-password")]
        public async Task<BaseUserResultDto> ResetPasswordAsync([FromBody] ForgetPasswordDto input)
        {
            ValidateLoginInfo(input);
            var resultByName = await _userManager.FindByNameAsync(input.Username);
            if (resultByName ==null)
            {
                return new BaseUserResultDto
                {
                    StatusCode = 500,
                    Message = L["User:UserNameInvalid", input.Username]
                };
            }
            var resultByEmail = await _userManager.FindByEmailAsync(input.UserEmail);
            if (resultByEmail ==null)
            {
                return new BaseUserResultDto
                {
                    StatusCode = 500,
                    Message = L["User:UserEmailInvalid", input.UserEmail]
                };
            }

            if (!resultByName.Id.Equals(resultByEmail.Id))
            {
                return new BaseUserResultDto
                {
                    StatusCode = 500,
                    Message = L["User:UserNameNotMatchEmail", input.Username,input.UserEmail]
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(resultByEmail);
            var resetResult = await _userManager.ResetPasswordAsync(resultByEmail, token, input.NewPassword);
            if (!resetResult.Succeeded)
            {
                return new BaseUserResultDto
                {
                    StatusCode = 500,
                    Message = L["User:ResetPasswordFail",resetResult.ToString().Split(":")[1].TrimStart()]
                };
            }
            return new BaseUserResultDto
            {
                StatusCode = 200,
                Message = L["User:ResetPasswordSuccess"]
            };
        }
        
        protected virtual void ValidateLoginInfo(ForgetPasswordDto login)
        {
            if (login == null)
            {
                throw new ArgumentException(nameof(login));
            }

            if (login.Username.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.Username));
            }
            
            if (login.UserEmail.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.UserEmail));
            }

            if (login.NewPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.NewPassword));
            }
        }
    }
}