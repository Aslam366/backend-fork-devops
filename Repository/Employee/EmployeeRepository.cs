using Microsoft.EntityFrameworkCore;
using ZetaSaasHRMSBackend.Data;


namespace ZetaSaasHRMSBackend.Repository.Employee
{
    using ZetaSaasHRMSBackend.Models;
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HrmsDbContext _context;

        public EmployeeRepository(HrmsDbContext context)
        {
            _context = context;
        }

        // 🔹 READ (Tenant filter applied automatically)
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employee
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        // 🔹 READ BY ID
        public async Task<Employee?> GetByIdAsync(long id)
        {
            return await _context.Employee
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // 🔹 CREATE
        public async Task CreateAsync(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
        }

        // 🔹 UPDATE
        public async Task UpdateAsync(Employee employee)
        {
            _context.Employee.Update(employee);
            await _context.SaveChangesAsync();
        }

        // 🔹 DELETE
        public async Task DeleteAsync(long id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null) return;

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
        }

    }
}
