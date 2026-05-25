using Microsoft.EntityFrameworkCore;
using PwnLearn.Data;
using PwnLearn.Services;

var builder = WebApplication.CreateBuilder(args);

var dataDirectory = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "data"));
Directory.CreateDirectory(dataDirectory);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?.Replace("|DataDirectory|", dataDirectory, StringComparison.Ordinal)
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// EF Core — connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register application services
builder.Services.AddScoped<AuthSession>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

// Apply EF Core migrations to the portable demo database on startup.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<PwnLearn.App>()
    .AddInteractiveServerRenderMode();

app.Run();
