using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.Authorization;
using ZetaSaasHRMSBackend.CustomModels;
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
        public async Task<IActionResult> Create(RoleRequest role)
        {
            var userRole = new Role
            {
                RoleName = role.RoleName,
                Description = role.Description

            };
            userRole.RoleMenuRight = role.MenuRights.Select(x => new RoleMenuRight
            {
                MenuId = x.MenuId,
                CanView = x.CanView,
                CanCreate = x.CanCreate,
                CanEdit = x.CanEdit,
                CanDelete = x.CanDelete
            }).ToList();
            await _roleRepository.CreateAsync(userRole);
            return Ok();
        }

        [HttpPut("Update")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> Update(RoleRequest role)
        {
            if (role.roleId <= 0)
                return BadRequest("Role Id is required");

            var userRoles = new Role    
            {
                Id = role.roleId,
                RoleName = role.RoleName,
                Description = role.Description,
                RoleMenuRight = role.MenuRights.Select(x => new RoleMenuRight
                {
                    RoleId = role.roleId,
                    MenuId = x.MenuId,
                    CanView = x.CanView,
                    CanCreate = x.CanCreate,
                    CanEdit = x.CanEdit,
                    CanDelete = x.CanDelete
                }).ToList()
            };

            await _roleRepository.UpdateAsync(userRoles);

            return Ok(new { message = "Role updated successfully" });
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
