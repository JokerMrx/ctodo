using Ctodo.Data;
using CTodo.Factories;
using CTodo.Options;
using CTodo.Providers;
using CTodo.Repositories.Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<DataContext>();
builder.Services.Configure<XmlStorageOptions>(builder.Configuration.GetSection("XmlStorage"));
builder.Services.AddTransient<StorageTypeProvider>();

builder.Services.AddSingleton<IOptionsMonitor<StorageOptions>>(provider => new OptionsMonitor<StorageOptions>(
    provider.GetRequiredService<IOptionsFactory<StorageOptions>>(),
    provider.GetRequiredService<IEnumerable<IOptionsChangeTokenSource<StorageOptions>>>(),
    provider.GetRequiredService<IOptionsMonitorCache<StorageOptions>>()));

builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<ICategoryRepository>(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateCategoryRepository());
builder.Services.AddScoped<ITodoRepository>(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateTodoRepository());

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
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();