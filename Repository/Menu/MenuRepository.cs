using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZetaSaasHRMSBackend.Data;
using ZetaSaasHRMSBackend.CustomModels;

namespace ZetaSaasHRMSBackend.Repository.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private readonly HrmsDbContext _context;

        public MenuRepository(HrmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuResponse>> GetUserMenusAsync(long userId)
        {
            long[] systemMenus = { 1, 2 }; // User Profile, My Profile

            // 1️⃣ User roles
            var roleIds = await _context.UserRole
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            // 2️⃣ RoleMenuRights (distinct by MenuId)
            var rights = await _context.RoleMenuRight
                .Where(r => roleIds.Contains(r.RoleId))
                .GroupBy(r => r.MenuId)
                .Select(g => new
                {
                    MenuId = g.Key,
                    CanView = g.Any(x => x.CanView),
                    CanCreate = g.Any(x => x.CanCreate),
                    CanEdit = g.Any(x => x.CanEdit),
                    CanDelete = g.Any(x => x.CanDelete)
                })
                .ToListAsync();

            // 3️⃣ MenuIds user can see + system menus
            var visibleMenuIds = rights
                .Where(r => r.CanView)
                .Select(r => r.MenuId)
                .Concat(systemMenus)
                .Distinct()
                .ToList();

            // 4️⃣ Load ALL active menus (important for parents)
            var menus = await _context.Menu
                .Where(m => m.IsActive)
                .Select(m => new MenuResponse
                {
                    MenuId = m.Id,
                    MenuName = m.MenuName,
                    ParentMenuId = m.ParentMenuId,
                    RouteUrl = m.RouteUrl,
                    Icon = m.Icon,
                    DisplayOrder = m.DisplayOrder
                })
                .ToListAsync();

            // 5️⃣ Apply permissions
            menus.ForEach(m =>
            {
                if (systemMenus.Contains(m.MenuId))
                {
                    m.CanView = m.CanCreate = m.CanEdit = m.CanDelete = true;
                    return;
                }

                var r = rights.FirstOrDefault(x => x.MenuId == m.MenuId);
                if (r != null)
                {
                    m.CanView = r.CanView;
                    m.CanCreate = r.CanCreate;
                    m.CanEdit = r.CanEdit;
                    m.CanDelete = r.CanDelete;
                }
            });

            // 6️⃣ Recursive visibility
            bool IsVisible(MenuResponse menu) =>
                menu.CanView ||
                menus.Any(c => c.ParentMenuId == menu.MenuId && IsVisible(c));

            return menus
                .Where(IsVisible)
                .OrderBy(m => m.DisplayOrder)
                .ToList();
        }
    }
}
