using Castle.Core.Smtp;
using Core.Authorization;
using Core.Interfaces;
using Core.Models;
using Core.Smtp;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Service;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(
    options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    ));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccountPolicy", policy =>
    {
        policy.Requirements.Add(new AccountTypeRequirement(Core.Const.AccountType.Admin));
        
    });
    options.AddPolicy("PatientAccountPolicy", policy =>
    {
        policy.Requirements.Add(new AccountTypeRequirement(Core.Const.AccountType.Patient));
    });
    options.AddPolicy("DoctorAccountPolicy", policy =>
    {
        policy.Requirements.Add(new AccountTypeRequirement(Core.Const.AccountType.Doctor));
    });
});


//For Dependency Injection
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPatientService,PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddTransient<IEmailService, EmailService>();


builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthorizationHandler, AccountTypeHandler>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
