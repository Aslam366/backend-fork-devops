using Microsoft.AspNetCore.Mvc;

namespace ZetaSaasHRMSBackend.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(long menuId, PermissionType permission) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { menuId, permission };
        }
    }
}
