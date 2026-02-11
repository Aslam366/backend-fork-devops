


namespace ZetaSaasHRMSBackend.Repository.Employee
{
    using ZetaSaasHRMSBackend.Models;
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(long id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(long id);
    }
}
