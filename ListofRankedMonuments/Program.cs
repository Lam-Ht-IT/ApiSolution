using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register repository and service
builder.Services.AddScoped<ISysUserRepository, SysUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICtgDiTichXepHangRepository, CtgDiTichXepHangRepository>();
builder.Services.AddScoped<ICtgTieuChiRepository, CtgTieuChiRepository>();
builder.Services.AddScoped<ICtgChiTieuRepository, CtgChiTieuRepository>();
builder.Services.AddScoped<ICtgKyBaoCaoRepository, CtgKyBaoCaoRepository>();
builder.Services.AddScoped<ISysGroupRepository, SysGroupRepository>();
builder.Services.AddScoped<ISysFunctionRepository, SysFunctionRepository>();
builder.Services.AddScoped<ISysFunctionInGroupRepository, SysFunctionInGroupRepository>();
builder.Services.AddScoped<ISysUserInGroupRepository, SysUserInGroupRepository>();
builder.Services.AddScoped<ICtgLoaiMauPhieuRepository, CtgLoaiMauPhieuRepository>();
builder.Services.AddScoped<ICtgLoaiDiTichRepository, CtgLoaiDiTichRepository>();
builder.Services.AddScoped<ICtgDonViTinhRepository, CtgDonViTinhRepository>();


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
    });

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin@example.com"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("3"));
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
