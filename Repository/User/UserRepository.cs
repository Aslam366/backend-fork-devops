using ZetaSaasHRMSBackend.Data;


namespace ZetaSaasHRMSBackend.Repository.User
{
    using Microsoft.EntityFrameworkCore;
    using ZetaSaasHRMSBackend.Models;
    public class UserRepository : IUserRepository
    {
        private readonly HrmsDbContext _context;

        public UserRepository(HrmsDbContext context)
        {
            _context = context;
        }

        // 🔹 READ 
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User
                .OrderBy(e => e.UserName)
                .ToListAsync();
        }

        // 🔹 READ BY ID
        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.User.Include(e => e.UserRole)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // 🔹 CREATE
        public async Task CreateAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        // 🔹 UPDATE
        public async Task UpdateAsync(User user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        // 🔹 DELETE
        public async Task DeleteAsync(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return;

            var user_roles = await _context.UserRole.Where(x => x.UserId == user.Id).ToListAsync();
            _context.UserRole.RemoveRange(user_roles);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
