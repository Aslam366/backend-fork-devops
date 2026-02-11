using ZetaSaasHRMSBackend.CustomModels;

namespace ZetaSaasHRMSBackend.Repository.Menu
{
    public interface IMenuRepository
    {
        Task<List<MenuResponse>> GetUserMenusAsync(long userId);
    }
}
