using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris;
using E_ticket.Repostoris.IRepository;
using E_ticket.utiltiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace E_ticket
{
    public class Program
    {
        public static void Main(string[] args)
     
      {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(
            Option=>Option.UseSqlServer(builder.Configuration.GetConnectionString("detualtconnection"))
                );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
             .AddGoogle("google", opt =>
             {
         var googleAuth = builder.Configuration.GetSection("Authentication:Google");
         opt.ClientId = googleAuth["ClientId"] ?? " ";
         opt.ClientSecret = googleAuth["ClientSecret"] ?? " ";
         opt.SignInScheme = IdentityConstants.ExternalScheme;
         });

          builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId =builder.Configuration["Authentication:Facebook:AppId"] ?? " ";
                facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]?? "";
            });




            builder.Services.AddTransient<IEmailSender, EmailSend>();

            builder.Services.AddScoped<ICategotyRepository, CategoryRepository>();
            builder.Services.AddScoped<IRuserotprepostoity, RuserotpRepository>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=movies}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
