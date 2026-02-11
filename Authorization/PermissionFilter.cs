using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZetaSaasHRMSBackend.Data;

namespace ZetaSaasHRMSBackend.Authorization
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly HrmsDbContext _context;
        private readonly long _menuId;
        private readonly PermissionType _permission;

        public PermissionFilter(HrmsDbContext context, long menuId, PermissionType permission)
        {
            _context = context;
            _menuId = menuId;
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = long.Parse(userIdClaim.Value);

            var hasAccess = await _context.RoleMenuRight
                .Where(r =>
                    r.MenuId == _menuId &&
                    r.Role.UserRole.Any(u => u.UserId == userId))
                .AnyAsync(r =>
                    _permission == PermissionType.View && r.CanView ||
                    _permission == PermissionType.Create && r.CanCreate ||
                    _permission == PermissionType.Edit && r.CanEdit ||
                    _permission == PermissionType.Delete && r.CanDelete
                );

            if (!hasAccess)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
