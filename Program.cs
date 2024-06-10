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
using CustomJiraTicketClient.Jira;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Globalization;
using System.Text.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        AppSecrets appSecrets = new AppSecrets();

        if (!await appSecrets.InitData(configuration.GetValue<string>("secretsDataLink")))
        {
            throw new Exception("Secrets undefined");
        }

        CollectionDBContext context = new CollectionDBContext(appSecrets.NpgSql_connection, DBSystem.POSTGRES);

        await CategoriesPackage.Initialize(context);

        IJwtTokenHelper jwtTokenHelper = new JwtTokenHelper();
        IPasswordHasher passwordHasher = new Sha3_256PasswordHasher();
        IPasswordGenerator passwordGenerator = new RNGCryptoPasswordGenerator();
        JiraTicketClient ticketClient = new JiraTicketClient(baseUrl: appSecrets.JiraBaseUrl,
                                                             userName: appSecrets.JiraUserName,
                                                             tokenOrPassword: appSecrets.JiraApiToken);

        CollectionAdapter collectionAdapter = new CollectionAdapter();
        CSVHepler csvHelper = new CSVHepler();

        // Application secrets
        builder.Services.AddSingleton<AppSecrets>((service) =>
        {
            return appSecrets;
        });

        // DbContext Service
        // Can be done as "AddSingleton" or as "AddScopped"
        // For this Service
        builder.Services.AddScoped<CollectionDBContext>((service) =>
        {
            CollectionDBContext dbContext = new CollectionDBContext(appSecrets.NpgSql_connection, DBSystem.POSTGRES);

            return dbContext;
        });

        // User Service
        builder.Services.AddSingleton<UserService>((service) =>
        {
            UserService? userService = new UserService(tokenHelper: jwtTokenHelper,
                                                  configuration: configuration,
                                                  passwordHasher: passwordHasher,
                                                  passwordGenerator : passwordGenerator,
                                                  appSecrets: appSecrets);
            return userService;
        });

        // Item Service
        builder.Services.AddSingleton<ItemService>((service) =>
        {
            ItemService itemService = new ItemService(configuration);

            return itemService;
        });

        // Collection Service
        builder.Services.AddSingleton<CollectionService>((service) =>
        {
            CollectionService collectionService = new CollectionService(configuration,
                csvHelper,
                collectionAdapter);

            return collectionService;
        });

        // Reaction(Like) Service
        builder.Services.AddSingleton<ReactionService>((service) =>
        {
            ReactionService reactionService = new ReactionService(configuration);

            return reactionService;
        });

        // Tag Service
        builder.Services.AddSingleton<TagService>((service) =>
        {
            TagService tagService = new TagService();

            return tagService;
        });

        builder.Services.AddScoped<JiraTicketService>((service) =>
        {
            JiraTicketService jiraTicketService = new JiraTicketService(ticketClient, configuration.GetValue<string>("jiraProjectKey"));

            return jiraTicketService;
        });

        //  Application Language Pack service
        builder.Services.AddSingleton<LanguagePackService>((service) =>
        {
            LanguagePackService languagePackService = LanguagePackService.GetInstance();
            
            return languagePackService;
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