using Ctodo.Data;
using CTodo.Factories;
using CTodo.GraphQL.GraphQLMutations;
using CTodo.GraphQL.GraphQLQueries;
using CTodo.GraphQL.GraphQLSchema;
using CTodo.GraphQL.GraphQLTypes;
using CTodo.Middlewares;
using CTodo.Options;
using CTodo.Providers;
using CTodo.Repositories.Infrastructure;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        b =>
        {
            b.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<DataContext>();
builder.Services.Configure<XmlStorageOptions>(builder.Configuration.GetSection("XmlStorage"));
builder.Services.AddTransient<StorageTypeProvider>();
builder.Services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

builder.Services.AddSingleton<IOptionsMonitor<StorageOptions>>(provider => new OptionsMonitor<StorageOptions>(
    provider.GetRequiredService<IOptionsFactory<StorageOptions>>(),
    provider.GetRequiredService<IEnumerable<IOptionsChangeTokenSource<StorageOptions>>>(),
    provider.GetRequiredService<IOptionsMonitorCache<StorageOptions>>()));

builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<ICategoryRepository>(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateCategoryRepository());
builder.Services.AddScoped<ITodoRepository>(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateTodoRepository());

builder.Services.AddScoped<TodoMutation>();
builder.Services.AddScoped<TodoQuery>();
builder.Services.AddScoped<TodoType>();
builder.Services.AddScoped<CategoryQuery>();
builder.Services.AddScoped<CategoryType>();

builder.Services.AddScoped<ISchema, AppSchema>();

builder.Services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
builder.Services.AddGraphQL(b => b
    .AddSystemTextJson()
    .AddGraphTypes(typeof(AppSchema).Assembly)
    .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = builder.Environment.IsDevelopment())
);

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

app.UseMiddleware<StorageTypeMiddleware>();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();