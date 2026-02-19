using ZetaSaasHRMSBackend.Data;

namespace ZetaSaasHRMSBackend.Repository.Role
{
    using Microsoft.EntityFrameworkCore;
    using ZetaSaasHRMSBackend.Models;
    public class RoleRepository : IRoleRepository
    {
        private readonly HrmsDbContext _context;

        public RoleRepository(HrmsDbContext context)
        {
            _context = context;
        }

        // 🔹 READ 
        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Role
                .OrderBy(e => e.RoleName)
                .ToListAsync();
        }

        // 🔹 READ BY ID
        public async Task<Role?> GetByIdAsync(long id)
        {
            return await _context.Role
                .Include(e => e.RoleMenuRight)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // 🔹 CREATE
        public async Task CreateAsync(Role role)
        {
            _context.Role.Add(role);
            await _context.SaveChangesAsync();
        }

        // 🔹 UPDATE
        public async Task UpdateAsync(Role role)
        {
            var existing = await _context.Role
                 .Include(x => x.RoleMenuRight)
                 .FirstOrDefaultAsync(x => x.Id == role.Id);

            if (existing == null)
                throw new Exception("Role not found");


            existing.RoleName = role.RoleName;
            existing.Description = role.Description;


            _context.RoleMenuRight.RemoveRange(existing.RoleMenuRight);

            existing.RoleMenuRight = role.RoleMenuRight;

            await _context.SaveChangesAsync();
        }

        // 🔹 DELETE
        public async Task DeleteAsync(long id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null) return;

            var rights = await _context.RoleMenuRight.Where(x => x.RoleId == role.Id).ToListAsync();

            _context.RoleMenuRight.RemoveRange(rights);
            _context.Role.Remove(role);

            await _context.SaveChangesAsync();
        }
    }
}
