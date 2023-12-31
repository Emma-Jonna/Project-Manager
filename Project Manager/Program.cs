using Microsoft.AspNetCore.Authentication.Cookies;
using Project_Manager.App;

namespace Project_Manager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppHelper.GetImageFolder();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options =>
                {
                    options.LoginPath = "/User/SignIn";
                    options.Cookie.Name = "ProjectManagerCookie";
                }
                );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }*/

            if (app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/Errors", "?statusCode={0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}