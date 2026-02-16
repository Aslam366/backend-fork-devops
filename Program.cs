using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ZetaSaasHRMSBackend.Data;
using ZetaSaasHRMSBackend.Middleware;
using ZetaSaasHRMSBackend.Repository.Auth;
using ZetaSaasHRMSBackend.Repository.Common;
using ZetaSaasHRMSBackend.Repository.Employee;
using ZetaSaasHRMSBackend.Repository.Menu;
using ZetaSaasHRMSBackend.Repository.Role;
using ZetaSaasHRMSBackend.Repository.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HrmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ZetaSaasHRMS")));

builder.Services.AddDbContext<ZetaSaasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ZetaSaas")));


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ZetaSaasHrms API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalhostTenants", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;

                // allow: http://localhost:4200 and http://abc.localhost:4200
                var isLocalhostTenant = uri.Host == "localhost" || uri.Host.EndsWith(".localhost");
                return uri.Scheme == "http" && uri.Port == 4200 && isLocalhostTenant;
            });
    });
});

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HrmsDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseCors("LocalhostTenants");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
