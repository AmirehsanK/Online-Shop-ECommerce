using Application.Security;
using Application.Services.Interfaces;
using Infra.Data.Context;
using IOC.DiContainer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Web.DemoData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.IOcContainer(builder.Configuration);

var databaseProvider = builder.Configuration["Database:Provider"] ?? DatabaseProvider.SqlServer;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    DatabaseProvider.Configure(options, databaseProvider, builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.Requirements.Add(new AdminRequirement()));
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database", tags: ["ready"]);

// Exported only when an OTLP endpoint is configured; docker-compose.yml points it at a local dashboard.
if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
{
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(builder.Configuration["OTEL_SERVICE_NAME"] ?? "online-shop"))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation(options =>
                options.Filter = context => !context.Request.Path.StartsWithSegments("/health"))
            .AddHttpClientInstrumentation())
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation())
        .WithLogging()
        .UseOtlpExporter();
}

var app = builder.Build();

// First, so the scheme and client address are corrected before anything reads them.
// It used to run after authentication and routing, where it had no effect.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

await PrepareDatabaseAsync(app);

app.Run();

// Migrations are applied on start only when Database:ApplyMigrations is set (the Docker demo).
// A real deployment should run them as a separate step so replicas never race each other.
static async Task PrepareDatabaseAsync(WebApplication app)
{
    var configuration = app.Configuration;
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (DatabaseProvider.IsSqlite(configuration["Database:Provider"]))
        await context.Database.EnsureCreatedAsync();
    else if (configuration.GetValue<bool>("Database:ApplyMigrations"))
        await context.Database.MigrateAsync();

    if (configuration.GetValue<bool>("Database:SeedDemoData"))
        await DemoDataSeeder.SeedAsync(context, scope.ServiceProvider.GetRequiredService<IPasswordHasher>(),
            configuration, app.Logger);
}

public partial class Program;
