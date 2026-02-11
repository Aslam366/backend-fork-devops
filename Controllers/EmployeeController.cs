using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using ZetaSaasHRMSBackend.Authorization;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.Employee;

namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private const long MENU_ID = 7;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Get")]
        [HasPermission(MENU_ID, PermissionType.View)]
        public async Task<IActionResult> Get()
        {
           return Ok(await _employeeRepository.GetAllAsync());
        }

        [HttpGet("GetById/{id}")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _employeeRepository.GetByIdAsync(id));
        }

        [HttpPost("Create")]
        [HasPermission(MENU_ID, PermissionType.Create)]
        public async Task<IActionResult> Create(Employee emp)
        {
            await _employeeRepository.CreateAsync(emp);
            return Ok();
        }

        [HttpPut("Update")]
        [HasPermission(MENU_ID, PermissionType.Edit)]
        public async Task<IActionResult> Update(Employee emp)
        {
            await _employeeRepository.UpdateAsync(emp);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission(MENU_ID, PermissionType.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _employeeRepository.DeleteAsync(id);
            return Ok();
        }

    }
}
