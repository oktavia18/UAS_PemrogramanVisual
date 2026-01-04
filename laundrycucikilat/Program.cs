using laundrycucikilat.Models;
using laundrycucikilat.Services;
using laundrycucikilat.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout 30 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "LaundrySession";
});

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<AuthService>();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Seed initial data including default users
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedDataAsync();
    
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    await authService.SeedDefaultUsersAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Remove HTTPS redirection for local development
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Session middleware
app.UseSession();

// Enable Auth Middleware with improved logic
app.UseAuthMiddleware();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Configure to run on port 5042
app.Urls.Add("http://localhost:5042");

app.Run();
