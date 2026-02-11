namespace ZetaSaasHRMSBackend.Repository.Common
{
    public class CommonRepository : ICommonRepository
    {
        public async Task<string> EncriptPwd(string Password)
        {
            try
            {
                int i;
                int i_PwdLen = Password.Length;
                string sEncriptedPwd = "";
                for (i = 0; i < i_PwdLen; i++)
                {
                    string sAscVal = "000" + ((int)Convert.ToChar((Password.Substring(i, 1))) - 7).ToString();
                    sEncriptedPwd = sEncriptedPwd + sAscVal.Substring(sAscVal.Length - 3, 3);
                }
                return sEncriptedPwd;
            }
            catch
            {
                return "";
            }
        }
    }
}
