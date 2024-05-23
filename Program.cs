using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.CustomDbContext;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Helpers;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Models;
using CourseProject_backend.Packages;
using CourseProject_backend.Repositories;
using CourseProject_backend.Services;
using MySql.Data.MySqlClient;
using System.Globalization;

internal class Program
{
    public static async Task Main(string[] args)
    {

        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        AppSecrets appSecrets = new AppSecrets();

        if(!await appSecrets.InitData(configuration.GetValue<string>("secretsDataLink")))
        {
            throw new Exception("Secrets undefined");
        }

        CollectionDBContext context = new CollectionDBContext(appSecrets.NpgSql_connection, DBSystem.POSTGRES);

        await CategoriesPackage.Initialize(context);

        IJwtTokenHelper jwtTokenHelper = new JwtTokenHelper();
        IPasswordHasher passwordHasher = new Sha3_256PasswordHasher();

        CollectionAdapter collectionAdapter = new CollectionAdapter();
        CSVHepler csvHelper = new CSVHepler();

        builder.Services.AddSingleton<AppSecrets>((service) =>
        {
            return appSecrets;
        });

        builder.Services.AddSingleton<CollectionDBContext>((service) =>
        {
            CollectionDBContext dbContext = new CollectionDBContext(appSecrets.NpgSql_connection, DBSystem.POSTGRES);

            return dbContext;
        });

        builder.Services.AddSingleton<UserService>((service) =>
        {
            var dbContext = service.GetService<CollectionDBContext>();

            UserService? userService = new UserService(tokenHelper: jwtTokenHelper,
                                                  configuration: configuration,
                                                  passwordHasher: passwordHasher,
                                                  appSecrets: appSecrets);


            return userService;
        });
        builder.Services.AddSingleton<ItemService>((service) =>
        {
            var dbContext = service.GetService<CollectionDBContext>();

            ItemService itemService = new ItemService(configuration);

            return itemService;
        });
        builder.Services.AddSingleton<CollectionService>((service) =>
        {
            CollectionService collectionService = new CollectionService(configuration,
                csvHelper,
                collectionAdapter);

            return collectionService;
        });
        builder.Services.AddSingleton<ReactionService>((service) =>
        {
            var dbContext = service.GetService<CollectionDBContext>();

            ReactionService reactionService = new ReactionService(configuration);

            return reactionService;
        });
        builder.Services.AddSingleton<IConfiguration>((service) =>
        {
            return configuration;
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Start}/{action=Index}/{lang?}/{id?}");

        app.Run();
    }
}