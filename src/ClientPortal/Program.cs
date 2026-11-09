using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClientPortal.Data;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (int.TryParse(port, out var listeningPort) && listeningPort is > 0 and <= 65535)
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{listeningPort}");
}
else if (!string.IsNullOrEmpty(port))
{
    throw new InvalidOperationException("PORT must be a valid TCP port.");
}

// Add services to the container.
var databaseProvider = builder.Configuration["Database:Provider"] ?? "Sqlite";
var connectionName = databaseProvider switch
{
    "Sqlite" => "DefaultConnection",
    "PostgreSql" => "PostgreSql",
    "Supabase" => "Supabase",
    _ => throw new InvalidOperationException($"Unsupported database provider '{databaseProvider}'.")
};
var connectionString = builder.Configuration.GetConnectionString(connectionName)
    ?? throw new InvalidOperationException($"Connection string '{connectionName}' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (databaseProvider is "PostgreSql" or "Supabase")
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages(o => o.Conventions.AuthorizeFolder("/Account"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

public partial class Program { }
