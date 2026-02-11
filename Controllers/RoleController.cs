using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.Authorization;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.Role;

namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private const long MENU_ID = 5;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("Get")]
        [HasPermission(MENU_ID, PermissionType.View)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleRepository.GetAllAsync());
        }

        [HttpGet("GetById/{id}")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _roleRepository.GetByIdAsync(id));
        }

        [HttpPost("Create")]
        [HasPermission(MENU_ID, PermissionType.Create)]
        public async Task<IActionResult> Create(Role role)
        {
            await _roleRepository.CreateAsync(role);
            return Ok();
        }

        [HttpPut("Update")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> Update(Role role)
        {
            await _roleRepository.UpdateAsync(role);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission(MENU_ID, PermissionType.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _roleRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
