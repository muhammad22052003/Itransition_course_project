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

        CollectionDBContext dbContext = new CollectionDBContext
                                        (
                                        builder.Configuration.GetValue<string>("DBConnections:npgsql"),
                                        DBSystem.POSTGRES
                                        );

        await CategoriesPackage.Initialize(dbContext);

        IJwtTokenHelper jwtTokenHelper = new JwtTokenHelper();
        IPasswordHasher passwordHasher = new Sha3_256PasswordHasher();

        CollectionRepository collectionRepository = new CollectionRepository(dbContext);
        ItemRepository itemRepository = new ItemRepository(dbContext);
        UserRepository userRepository = new UserRepository(dbContext);
        TagRepository tagRepository = new TagRepository(dbContext);
        CategoryRepository categoryRepository = new CategoryRepository(dbContext);
        CommentRepository commentRepository = new CommentRepository(dbContext);
        ReactionRepository reactionRepository = new ReactionRepository(dbContext);

        UserService userService = new UserService(repository: userRepository,
                                                  tokenHelper: jwtTokenHelper,
                                                  configuration: configuration,
                                                  passwordHasher: passwordHasher);
        ItemService itemService = new ItemService(repository: itemRepository,
                                                  configuration: configuration,
                                                  dbContext: dbContext,
                                                  tagRepository: tagRepository,
                                                  commentRepository: commentRepository);
        CollectionService collectionService = new CollectionService(repository: collectionRepository,
                                                                    configuration: configuration,
                                                                    dbContext: dbContext,
                                                                    userService: userService,
                                                                    categoryRepository: categoryRepository);

        ReactionService reactionService = new ReactionService(reactionRepository);

        builder.Services.AddScoped<UserService>((service) =>
        {
            return userService;
        });
        builder.Services.AddScoped<ItemService>((service) =>
        {
            return itemService;
        });
        builder.Services.AddScoped<CollectionService>((service) =>
        {
            return collectionService;
        });
        builder.Services.AddScoped<CollectionDBContext>((service) =>
        {
            return dbContext;
        });
        builder.Services.AddScoped<ReactionService>((service) =>
        {
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