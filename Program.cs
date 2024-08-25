using studentAPI.Service;
using Azure.Data.Tables;
using OpenTelemetry.Logs;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// adding loggers
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());

// Adding Application Insights for logging
builder.Logging.AddApplicationInsights(
    configureTelemetryConfiguration: (config) => 
        config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"],
    configureApplicationInsightsLoggerOptions: (options) => { }
);
// adding application insights for logging
// builder.Logging.AddApplicationInsights(
//         configureTelemetryConfiguration: (config) => 
//             config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
//             configureApplicationInsightsLoggerOptions: (options) => { }
//     );

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

// Add Swagger and configure it to use the XML documentation file
builder.Services.AddSwaggerGen(options =>
{
    // Specify the OpenAPI info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Student API",
        Description = "An API to manage students",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com",
        }
    });

    // Include XML comments if generated
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

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
