using Microsoft.EntityFrameworkCore;
using studentAPI.Data;
using studentAPI.Service;
using Azure.Data.Tables;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Register your StudentServiceAzure as the implementation of IStudentService
builder.Services.AddScoped<IStudentService, StudentServiceAzure>();

// Register the controllers
builder.Services.AddControllers();


// builder.Services.AddDbContext<AppDbContext>(options =>
//     {
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//                      new MySqlServerVersion(new Version(8, 0, 36)));
//     options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//     });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
