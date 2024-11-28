using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CarRentalSystemAssignment.Data;
using CarRentalSystemAssignment.Repositories;
using CarRentalSystemAssignment.Services;
using System.Text;
using CarRentalSystemAssignment.Filters;
using CarRentalSystemAssignment.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<SystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CarRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CarRentalService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // Add custom validation filter globally
});
builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Specify the default authentication scheme
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Role-based authorization setup
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication middleware first (before authorization)
app.UseAuthentication(); // This ensures authentication happens before authorization
app.UseMiddleware<JwtValidationMiddleware>(); // Custom JWT validation (optional if you need additional custom checks)

// Authorization middleware should be after authentication
app.UseAuthorization();

app.MapControllers(); // Map the controller endpoints

app.Run();
