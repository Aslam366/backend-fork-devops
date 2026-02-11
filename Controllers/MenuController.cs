using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZetaSaasHRMSBackend.Repository.Menu;

namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpGet("GetMyMenus")]
        public async Task<IActionResult> GetMyMenus()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var menus = await _menuRepository.GetUserMenusAsync(userId);

            return Ok(menus);
        }
    }
}
