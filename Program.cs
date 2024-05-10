using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.CustomDbContext;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Models;
using CourseProject_backend.Packages;
using MySql.Data.MySqlClient;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        /*builder.Services.AddSingleton<LanguagePackSingleton>((service) =>
        {

        });*/

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Start}/{action=Index}/{id?}");

        var lang = LanguagePackSingleton.GetInstance();

        var pack = lang.GetLanguagePack(AppLanguage.uz);

        using (var con = new CollectionDBContext(app.Configuration.GetValue<string>("DBConnections:mysql"),
                                                 DBSystem.MYSQL))
        {
            
        }

        app.Run();
    }
}