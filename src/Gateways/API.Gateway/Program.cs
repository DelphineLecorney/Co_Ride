using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    // Limiete de 100 requêtes par minute
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));

    // Limite par endpoint
    options.AddFixedWindowLimiter("api", options =>
    {
        options.PermitLimit = 50;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Trop de requêtes",
            message = "Limite de débit dépassée. Veuillez réessayer plus tard. ",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? (double?)retryAfter.TotalSeconds
                : null
        }, cancellationToken: token);
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "",
        Version = "v1",
        Description = "Point d'entrée unique pour tous les microservices de la plateforme de covoiturage"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRateLimiter();

app.MapGet("/", () => Results.Ok(new
{
    service = "Passerelle API pour plateforme de covoiturage",
    version = "1.0.0",
    status = "Running",
    endpoints = new
    {
        trips = "/api/trips",
        bookins = "/api/bookings",
        health = "/health",
        swagger = "/swagger"
    }

})).WithName("Gateway Info");

app.MapHealthChecks("/health");

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseRateLimiter();
});

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("API Gateway démarré avec succès!");
    logger.LogInformation("Trip Service: {TripUrl}", 
        builder.Configuration["ReverseProxy:Clusters:trip-cluster:Destinations:trip-destination:Address"]);
    logger.LogInformation("Booking Service: {BookingUrl}", 
        builder.Configuration["ReverseProxy:Clusters:booking-cluster:Destinations:booking-destination:Address"]);
});

app.Run();
