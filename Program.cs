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

        for (int i = 0; i < 100; i++)
        {
            CollectionBuilder collectionBuilder = new CollectionBuilder(configuration);
            Category category = new Category($"Books{i+1}", "Books Category");
            UserBuilder userBuilder = new UserBuilder(configuration);
            userBuilder.SetParameters("Muhammad", "muh22@amail.com", "pass", UserRoles.User);
            User user = userBuilder.Build() as User;

            collectionBuilder.SetParameters($"MyCOL{i}", "dESC", user, category);

            MyCollection myCollection = collectionBuilder.Build() as MyCollection;

            ItemBuilder itemBuilder = new ItemBuilder(configuration);

            itemBuilder.SetParameters($"Kitob{i}", myCollection);

            Item item = itemBuilder.Build() as Item;

            dbContext.Users.Add(user);
            dbContext.Categories.Add(category);
            dbContext.Items.Add(item);
            dbContext.Collections.Add(myCollection);
        }

        dbContext.SaveChanges();

        await CategoriesPackage.Initialize(dbContext);

        IJwtTokenHelper jwtTokenHelper = new JwtTokenHelper();
        IPasswordHasher passwordHasher = new Sha3_256PasswordHasher();

        CollectionRepository collectionRepository = new CollectionRepository(dbContext);
        ItemRepository itemRepository = new ItemRepository(dbContext);
        UserRepository userRepository = new UserRepository(dbContext);
        TagRepository tagRepository = new TagRepository(dbContext);

        UserService userService = new UserService(repository: userRepository,
                                                  tokenHelper: jwtTokenHelper,
                                                  configuration: configuration,
                                                  passwordHasher: passwordHasher);
        ItemService itemService = new ItemService(repository: itemRepository,
                                                  configuration: configuration,
                                                  dbContext: dbContext,
                                                  tagRepository);
        CollectionService collectionService = new CollectionService(repository: collectionRepository,
                                                                    configuration: configuration,
                                                                    dbContext: dbContext,
                                                                    userService: userService);

        builder.Services.AddSingleton<UserService>((service) =>
        {
            return userService;
        });
        builder.Services.AddSingleton<ItemService>((service) =>
        {
            return itemService;
        });
        builder.Services.AddSingleton<CollectionService>((service) =>
        {
            return collectionService;
        });
        builder.Services.AddSingleton<CollectionDBContext>((service) =>
        {
            return dbContext;
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

        //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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