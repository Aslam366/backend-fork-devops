namespace ZetaSaasHRMSBackend.Repository.User
{
    using ZetaSaasHRMSBackend.Models;
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(long id);
    }
}
