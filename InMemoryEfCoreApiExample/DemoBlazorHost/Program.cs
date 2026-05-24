using API.Controllers;
using API.Middleware;
using DemoBlazorHost.Components;
using Infrastructure;
using Infrastructure.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// UI (Blazor)
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Backend (DI + InMemory EF Core)
builder.Services.AddInfrastructure(builder.Configuration);

// Host API controllers (from your existing API project) in the same process
builder.Services.AddControllers().AddApplicationPart(typeof(CategoriesController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Seed the in-memory DB on startup (data exists only while this process runs)
await SeedData.Initialize(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Swagger (enabled by default in Development; can be overridden via config)
bool swaggerEnabled = app.Configuration.GetValue("Swagger:Enabled", app.Environment.IsDevelopment());

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply your JSON exception middleware only for API endpoints (keeps UI error handling intact)
app.UseWhen(
    ctx => ctx.Request.Path.StartsWithSegments("/api"),
    branch => branch.UseMiddleware<ExceptionHandlerMiddleware>());

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

// Map API + UI
app.MapControllers();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
