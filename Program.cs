using Microsoft.EntityFrameworkCore;
using studentAPI.Service;
using Azure.Data.Tables;
using OpenTelemetry.Logs;


var builder = WebApplication.CreateBuilder(args);

// adding loggers
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());

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

// Register the TableServiceClient with your Azure Storage connection string
builder.Services.AddSingleton(new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=apidatastorage1234;AccountKey=9MqbI1u5LZNBQPCNRdOc+LimDv6rGAzdPCDWj/8/grgeSTefVRrrjLY0NoM+BIIYuz7MnUt7yOw7+AStcWw9lA==;EndpointSuffix=core.windows.net"));

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
