


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("no conection string was found");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoriesService,CategoriesService>();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<IGemesService, GamesService>();
builder.Services.AddScoped<IMailingService, MailingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
