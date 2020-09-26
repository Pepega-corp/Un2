using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Unicon.Backend.Common.Modularity;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Entities;
using Unicon.Backend.WebApi.Module;

namespace Unicon.Backend.WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			Console.WriteLine();
			services.AddSingleton<ITypesContainer, TypesContainer>();
			services.AddSingleton<RegistriesInitializer>();
	
		
			services.AddDbContext<ApplicationContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IServiceCollection>(services);
			services.AddSingleton<ITypesProvider, TypesProvider>();
			InitializeModules(services, new MainModule());
			services.AddControllers()
				.AddNewtonsoftJson(options => options.SerializerSettings.TypeNameHandling = TypeNameHandling.All);
		}

		private void InitializeModules(IServiceCollection services, params IModule[] modules)
		{
			foreach (var module in modules)
			{
				module.InitializeTypes(new TypesContainer(services));
			}
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
			RegistriesInitializer registriesInitializer)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			registriesInitializer.Init(new MainRegistry());
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action}/{id?}");
				endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Unicon backend. Welcome. Again."); });
			});
			app.EnsureMigrationOfContext<ApplicationContext>();

		}
	}


	public static class EnsureMigration
	{
		public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetService<T>();
				dbContext.Database.Migrate();
			}
		}
	}
}
