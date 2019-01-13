using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitproKeyGen.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using BitproKeyGen.Models;

namespace BitproKeyGen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            /* Not Used with Customized Identity
            //services.AddIdentity<ApplicationUser, ApplicationRole>(
            //    option => option.Stores.MaxLengthForKeys = 128)
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultUI()
            //    .AddDefaultTokenProviders();
            */
            //services.AddDefaultIdentity <ApplicationUser>().AddRoles<ApplicationRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultUI()
           .AddDefaultTokenProviders();


            services.AddMvc(config =>
            {
                // using Microsoft.AspNetCore.Mvc.Authorization;
                // using Microsoft.AspNetCore.Authorization;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddMvc().AddRazorPagesOptions(options =>
            //{
            //    options.Conventions.AuthorizeFolder("/");
            //    options.Conventions.AllowAnonymousToPage("/Account/Login");
            //});

            services.AddAuthorization(options =>
            {
                options.AddPolicy( "AdminOnly", policy => policy.RequireRole("Admin"));
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
            //, ApplicationDbContext context,
            //RoleManager<ApplicationRole> roleManager,
            //UserManager<ApplicationUser> userManager)

        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            //
            CreateRoles(serviceProvider).Wait();
            //
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=KeyRequests}/{action=Index}/{id?}");
                    //template: "{controller=Home}/{action=Index}/{id?}");
            });


           
        }


        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "User", "Partners" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new ApplicationRole(roleName));


                    if (!roleResult.Succeeded)
                    {

                        throw new Exception("Failed to create role");

                    }
                }
            }

            ApplicationUser user = await UserManager.FindByEmailAsync("sales@bitpro-tech.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "sales@bitpro-tech.com",
                    Email = "sales@bitpro-tech.com",
                };
                await UserManager.CreateAsync(user, "Test@123");
            }
            await UserManager.AddToRoleAsync(user, "Admin");


            //IdentityUser user1 = await UserManager.FindByEmailAsync("tejas@gmail.com");

            //if (user1 == null)
            //{
            //    user1 = new IdentityUser()
            //    {
            //        UserName = "tejas@gmail.com",
            //        Email = "tejas@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user1, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user1, "User");

            //IdentityUser user2 = await UserManager.FindByEmailAsync("rakesh@gmail.com");

            //if (user2 == null)
            //{
            //    user2 = new IdentityUser()
            //    {
            //        UserName = "rakesh@gmail.com",
            //        Email = "rakesh@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user2, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user2, "HR");

        }
    }
}
