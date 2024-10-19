using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using DemoPL.Helpers;
using DemoPL.Helpers.Interfaces;
using DemoPL.MappingProfiles;
using DemoPL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPL
{
    public class Program
    {
		// Entry Point
        public static void Main(string[] args)
        {

            var Bulider = WebApplication.CreateBuilder(args);
			#region Configure Services That Allow Dependancy Injection
			Bulider.Services.AddControllersWithViews();

			Bulider.Services.AddDbContext<MvcAppContext>(Options =>
			{
				//Options.UseSqlServer("Server= .; Database= MvcApp; Integrated Security=True; Trusted_Connection=True; Encrypt=false ");

				Options.UseSqlServer(Bulider.Configuration.GetConnectionString("DefaultConnection"));

			}
			  , ServiceLifetime.Scoped); // Allow Dependancy Injection


			Bulider.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependancy Injection Class Department
																			   //services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow Dependancy Injection Class Department

			Bulider.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Bulider.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>{ new DepartmentProfile(),
				new EmployeeProfile () , new UserProfile (), new RoleProfile () }));
			//services.AddAutoMapper(M=>M.AddProfile(new DepartmentProfile()));

			//services.AddScoped<UserManager<ApplicationUser>>();
			//services.AddScoped<SignInManager<ApplicationUser>>();

			Bulider.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
			{
				Options.Password.RequireNonAlphanumeric = true; // @ # $
				Options.Password.RequireLowercase = true; // sld
				Options.Password.RequireUppercase = true; //ADH
				Options.Password.RequireDigit = true; //12345
													  //P@ssw0rd
													  //Pa$$w0rd
			}).AddEntityFrameworkStores<MvcAppContext>()
			  .AddDefaultTokenProviders();

			Bulider.Services.AddTransient<IMailSettings , EmailSettings>();
			Bulider.Services.AddTransient<ISmsService , SmsService>();
			Bulider.Services.Configure<MailSettings>(Bulider.Configuration.GetSection("MailSettings"));
			Bulider.Services.Configure<TwilioSettings>(Bulider.Configuration.GetSection("Twilio"));


			//Bulider.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
			//{
			//	Options.LoginPath = "/Account/Login";
			//	Options.AccessDeniedPath = "/Home/Error";

			//});

		Bulider.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
		{
			options.LoginPath = "/Account/Login"; // Path for login
			options.AccessDeniedPath = "/Home/Error"; // Path for access denied
		})
		.AddGoogle(GoogleDefaults.AuthenticationScheme,options =>
		{
			IConfiguration GoogleAuthSection = Bulider.Configuration.GetSection("Authentication:Google");
			options.ClientId = GoogleAuthSection["ClientId"];
			options.ClientSecret = GoogleAuthSection["ClientSecret"];

			options.Events = new OAuthEvents
			{
				OnRemoteFailure = context =>
				{
					
					context.Response.Redirect("/Account/Login"); // Redirect to login on failure
					context.HandleResponse(); // Prevent the exception from being thrown
					return Task.CompletedTask;
				}
			};
		});



			#endregion

			var app = Bulider.Build();

			#region Configure Http Request Pipelines
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			#endregion
			app.Run ();
		}


	}
}
