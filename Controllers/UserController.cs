using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.Authorization;
using ZetaSaasHRMSBackend.CustomModels;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.User;

namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private const long MENU_ID = 3;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("Get")]
        [HasPermission(MENU_ID, PermissionType.View)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        [HttpGet("GetById/{id}")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _userRepository.GetByIdAsync(id));
        }

        [HttpPost("Create")]
        [HasPermission(MENU_ID, PermissionType.Create)]
        public async Task<IActionResult> Create(UserRequest request)
        {
            if (request.PasswordHash != request.ConfirmPassword)
                throw new Exception("Password and Confirm Password do not match");

            var adminUsers = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                IsActive = true

            };
            adminUsers.UserRole = request.RoleIds.Select(roleId => new UserRole
            {
                RoleId = roleId
            }).ToList();
            await _userRepository.CreateAsync(adminUsers);
            return Ok();
        }

        [HttpPut("Update")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> Update(UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.PasswordHash != request.ConfirmPassword)
                return BadRequest("Password and Confirm Password do not match");

            var adminUser = new User
            {
                Id = request.userId,
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                IsActive = true
            };

            // assign roles
            adminUser.UserRole = request.RoleIds.Select(roleId => new UserRole
            {
                UserId = request.userId,
                RoleId = roleId
            }).ToList();

            await _userRepository.UpdateAsync(adminUser);

            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission(MENU_ID, PermissionType.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _userRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
