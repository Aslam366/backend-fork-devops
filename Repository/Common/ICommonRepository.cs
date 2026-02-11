namespace ZetaSaasHRMSBackend.Repository.Common
{
    public interface ICommonRepository
    {
        Task<string> EncriptPwd(string Password);
    }
}
