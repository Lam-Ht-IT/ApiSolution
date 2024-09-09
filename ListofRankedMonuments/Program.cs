using Jose;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Repositories;
using QUANLYVANHOA.Utilities;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Cấu hình JWT từ appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


// Tự động đăng ký tất cả các repository trong assembly
// Assembly là một đơn vị mã đã được biên dịch (compiled unit)
// => contain: Types(classes, interfaces, enums), Methods, properties, events
builder.Services.AddRepositoriesAndServices(typeof(SysUserRepository).Assembly); // typeof(SysUserRepository) đại diện, object chứa metadata

////Register repository and service
//builder.Services.AddScoped<ISysUserRepository, SysUserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ICtgDiTichXepHangRepository, CtgDiTichXepHangRepository>();
//builder.Services.AddScoped<ICtgTieuChiRepository, CtgTieuChiRepository>();
//builder.Services.AddScoped<ICtgChiTieuRepository, CtgChiTieuRepository>();
//builder.Services.AddScoped<ICtgKyBaoCaoRepository, CtgKyBaoCaoRepository>();
//builder.Services.AddScoped<ISysGroupRepository, SysGroupRepository>();
//builder.Services.AddScoped<ISysFunctionRepository, SysFunctionRepository>();
//builder.Services.AddScoped<ISysFunctionInGroupRepository, SysFunctionInGroupRepository>();
//builder.Services.AddScoped<ISysUserInGroupRepository, SysUserInGroupRepository>();
//builder.Services.AddScoped<ICtgLoaiMauPhieuRepository, CtgLoaiMauPhieuRepository>();
//builder.Services.AddScoped<ICtgLoaiDiTichRepository, CtgLoaiDiTichRepository>();
//builder.Services.AddScoped<ICtgDonViTinhRepository, CtgDonViTinhRepository>();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Register PermissionService

// Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // Event handling for automatic token refresh
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                // Bỏ qua thử thách mặc định để tránh trả về 401 khi token hết hạn
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { error = "You are not authorized" });
                return context.Response.WriteAsync(result);
            }
        };
    });

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin@example.com"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("user1@example.com"));
});

// Thêm dịch vụ CORS vào DI container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Thay thế bằng địa chỉ frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowLocalhost3000");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
