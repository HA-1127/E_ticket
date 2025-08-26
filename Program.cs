using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris;
using E_ticket.Repostoris.IRepository;
using E_ticket.utiltiy;
using E_ticket.utiltiy.Bbinitializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Stripe;
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
            //add identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
                
            }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            // add login google
            builder.Services.AddAuthentication()
             .AddGoogle("google", opt =>
             {
         var googleAuth = builder.Configuration.GetSection("Authentication:Google");
         opt.ClientId = googleAuth["ClientId"] ?? " ";
         opt.ClientSecret = googleAuth["ClientSecret"] ?? " ";
         opt.SignInScheme = IdentityConstants.ExternalScheme;
         });
            // login facebook
          builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId =builder.Configuration["Authentication:Facebook:AppId"] ?? " ";
                facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]?? "";
            });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/identity/account/Login";
                options.AccessDeniedPath = "/Identity/account/AccessDenied";
            });
            // email sender
            builder.Services.AddTransient<IEmailSender, EmailSend>();
            //reopseitory

            builder.Services.AddScoped<ICategotyRepository, CategoryRepository>();
            builder.Services.AddScoped<Imovierepository, Movierepository>();
            builder.Services.AddScoped<IRuserotprepostoity, RuserotpRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<ITicketItemRepository, TicketItemRepository>();
            builder.Services.AddScoped<Iactoryrepository, ActoryRepository>();
            builder.Services.AddScoped<IcinemaRepository, CineamRepository>();

            builder.Services.AddScoped<IMoviesUserTicketRepositiriy, MoviesUserTicketReopsitoriy>();
            builder.Services.AddScoped<IDBinitiaitizer, DBinitializer>();
           // builder.Services.AddScoped<IUntiOfWorkeRepositery, UnitOfWorkRepository>();
          


          // Stripe
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            var app = builder.Build();
          
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //initializer

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBinitiaitizer>();
                dbInitializer.Initialize();
            }
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
