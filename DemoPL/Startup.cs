//using AutoMapper;
//using Demo.BLL.Interfaces;
//using Demo.BLL.Repositories;
//using Demo.DAL.Contexts;
//using Demo.DAL.Models;
//using DemoPL.MappingProfiles;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DemoPL
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//       /* public void ConfigureServices(IServiceCollection services)
//        {
//            //services.AddControllersWithViews();

//            //services.AddDbContext<MvcAppContext>(Options =>
//            //{
//            //    //Options.UseSqlServer("Server= .; Database= MvcApp; Integrated Security=True; Trusted_Connection=True; Encrypt=false ");

//            //    Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            
//            //}
//            //  ,ServiceLifetime.Scoped  ); // Allow Dependancy Injection


//            //services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow Dependancy Injection Class Department
//            ////services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow Dependancy Injection Class Department

//            //services.AddScoped<IUnitOfWork , UnitOfWork>();
//            //services.AddAutoMapper(M=>M.AddProfiles(new List<Profile>{ new DepartmentProfile(), 
//            //    new EmployeeProfile () , new UserProfile (), new RoleProfile () }));
//            ////services.AddAutoMapper(M=>M.AddProfile(new DepartmentProfile()));

//            ////services.AddScoped<UserManager<ApplicationUser>>();
//            ////services.AddScoped<SignInManager<ApplicationUser>>();

//            //services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
//            //{
//            //    Options.Password.RequireNonAlphanumeric = true; // @ # $
//            //    Options.Password.RequireLowercase = true; // sld
//            //    Options.Password.RequireUppercase = true; //ADH
//            //    Options.Password.RequireDigit = true; //12345
//            //    //P@ssw0rd
//            //    //Pa$$w0rd
//            //}).AddEntityFrameworkStores<MvcAppContext>()
//            //  .AddDefaultTokenProviders();
//            //    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
//            //    {
//            //        Options.LoginPath = "Account/Login";
//            //        Options.AccessDeniedPath = "Home/Error";
//            //    });


//        }*/

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        /*public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            //if (env.IsDevelopment())
//            //{
//            //    app.UseDeveloperExceptionPage();
//            //}
//            //else
//            //{
//            //    app.UseExceptionHandler("/Home/Error");
//            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//            //    app.UseHsts();
//            //}
//            //app.UseHttpsRedirection();
//            //app.UseStaticFiles();

//            //app.UseRouting();

//            //app.UseAuthentication();

//            //app.UseAuthorization();

//            //app.UseEndpoints(endpoints =>
//            //{
//            //    endpoints.MapControllerRoute(
//            //        name: "default",
//            //        pattern: "{controller=Home}/{action=Index}/{id?}");
//            //});
//        }*/
//    }
//}
