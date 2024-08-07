using CodeAPI.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CodeAPI.Constants;

// OpenTelemetry
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// OpenTelemetry Service
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: OpenTelemetryConstants.ServiceName,
        serviceVersion: OpenTelemetryConstants.ServiceVersion))
    .WithTracing(tracing => tracing
        .AddSource(OpenTelemetryConstants.ServiceName)
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddMeter(OpenTelemetryConstants.ServiceName)
        .AddConsoleExporter());

builder.Logging.AddOpenTelemetry(options => options
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
        serviceName: OpenTelemetryConstants.ServiceName,
        serviceVersion: OpenTelemetryConstants.ServiceVersion))
    .AddConsoleExporter());

// Register the Instrumentation class as a singleton in the DI container.
builder.Services.AddSingleton<Instrumentation>();

builder.Services.AddControllers();

// Add mongodb service
// Configure MongoDB connection
var mongoConnectionString = "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoConnectionString);
// Register the custom serializer for JsonElement
BsonSerializer.RegisterSerializer(new JsonElementSerializer());

var mongoDatabase = mongoClient.GetDatabase("mongo-db");

// Register MongoDB database instance for dependency injection
builder.Services.AddSingleton(mongoDatabase);

// Add ICodeContentFactory dependency injection
builder.Services.AddScoped<ICodeContentFactory, CodeContentFactory>();

// Adding Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.Run();
