using API.Data;
using API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<User>(options =>
{
    //Password Configurations
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    //For Email Configurations
    options.SignIn.RequireConfirmedEmail = true;
}).AddRoles<IdentityRole>() //able to add role
  .AddRoleManager<RoleManager<IdentityRole>>()//able to make use of RoleManager
  .AddEntityFrameworkStores<Context>() // provide our context
  .AddSignInManager<SignInManager<User>>() // make of use of signin manager
  .AddUserManager<UserManager<User>>() // to create user
  .AddDefaultTokenProviders(); // create token for the email confirmation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
