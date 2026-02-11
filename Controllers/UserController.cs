using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.Authorization;
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
        public async Task<IActionResult> Create(User user)
        {
            await _userRepository.CreateAsync(user);
            return Ok();
        }

        [HttpPut("Update")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> Update(User user)
        {
            await _userRepository.UpdateAsync(user);
            return Ok();
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
