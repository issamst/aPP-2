using Asp_Net_Ticket.Context;
using Asp_Net_Ticket.Service.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Text;
using AutoMapper; // Add this using statement
using FluentValidation; // Add this using statement
using Asp_Net_Ticket.Dto.Ticket;
using Asp_Net_Ticket.Mappings;
using FluentValidation.AspNetCore;
using Asp_Net_Ticket.Validators;

var builder = WebApplication.CreateBuilder(args);

// Set the ExcelPackage license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container
builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateTicketValidators>());
builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateTicketValidators>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Ticket_Services>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Replace with "https://localhost:4200" if using HTTPS on Angular
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Enable this if you use cookies or authorization headers
    });
});



// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnStr"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); // Register your mapping profile

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy"); // Apply the CORS policy
app.UseAuthentication(); // Make sure you have authentication configured if needed
app.UseAuthorization();

app.MapControllers();

app.Run();
