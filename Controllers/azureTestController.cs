using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.Authorization;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.Employee;

namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class azureTestController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private const long MENU_ID = 7;

        public azureTestController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Get")]
        [HasPermission(MENU_ID, PermissionType.View)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _employeeRepository.GetAllAsync());
        }

        [HttpPost("Create")]
        [HasPermission(MENU_ID, PermissionType.Create)]
        public async Task<IActionResult> Create(Employee emp)
        {
            await _employeeRepository.CreateAsync(emp);
            return Ok();
        }
    }
}
