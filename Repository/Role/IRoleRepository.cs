namespace ZetaSaasHRMSBackend.Repository.Role
{
    using ZetaSaasHRMSBackend.Models;
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(long id);
        Task CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(long id);
    }
}
