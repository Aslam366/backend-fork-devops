using Microsoft.EntityFrameworkCore;
using ZetaSaasHRMSBackend.Data;

namespace ZetaSaasHRMSBackend.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TenantContext tenantContext, ZetaSaasDbContext dbContext)
        {
            // 1️⃣ Read host
            var host = context.Request.Host.Host; // abc.zaas.com

            // 2️⃣ Extract subdomain
            var parts = host.Split('.');
            //if (parts.Length < 3)
            //{
            //    context.Response.StatusCode = 400;
            //    await context.Response.WriteAsync("Invalid tenant URL");
            //    return;
            //}

            var tenantName = parts[0]; // abc

            // 3️⃣ Validate tenant
            var tenant = await dbContext.Tenant
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TenantName == tenantName && t.IsActive);

            if (tenant == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Tenant not found");
                return;
            }

            // 4️⃣ Store tenant info
            tenantContext.TenantId = tenant.Id;
            tenantContext.TenantName = tenantName;

            // 5️⃣ Continue pipeline
            await _next(context);
        }
    }
}
