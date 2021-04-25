using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RepositoryPattern.Database.Context;
using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Database.Repository;
using RepositoryPattern.Models;
using RepositoryPattern.Services;

namespace RepositoryPattern
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers()
                .AddJsonOptions(ops =>
                {
                    ops.JsonSerializerOptions.WriteIndented = true;
                    ops.JsonSerializerOptions.IgnoreNullValues = true;
                    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );


            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "RepositoryPattern", Version = "v1"}); });

            var corsBuilder = new CorsPolicyBuilder();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsBuilder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().Build());
            });

            // DB stuff
            services.AddDbContext<AppDbContext>(ServiceLifetime.Singleton);
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDataSource, AppDbContext>();

            // register services
            services.AddSingleton<IDeveloperService, DeveloperService>();
            services.AddSingleton<IProjectService, ProjectService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            InitDb(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepositoryPattern v1"));
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void InitDb(IServiceProvider serviceProvider)
        {
            var projectService = serviceProvider.GetService<IProjectService>();
            var developerService = serviceProvider.GetService<IDeveloperService>();

            var project = projectService!.FindAll().First();

            var developer1 = developerService!.FindById(Guid.Parse("15c011fe-428b-45c7-ae41-02f54bf35eb1"));
            var developer2 = developerService!.FindById(Guid.Parse("15c011fe-428b-45c7-ae41-02f54bf35eb2"));

            developer1.DeveloperProjectRelations = new List<DeveloperProjectRelation>()
                {new DeveloperProjectRelation() {Project = project, ProjectId = project.Id, Developer = developer1, DeveloperId = developer1.Id}};
            developer2.DeveloperProjectRelations = new List<DeveloperProjectRelation>()
                {new DeveloperProjectRelation() {Project = project, ProjectId = project.Id, Developer = developer2, DeveloperId = developer2.Id}};

            // updating DeveloperProjectRelations of a project would lead to same result
            developerService.Update(developer1);
            developerService.Update(developer2);
        }
    }
}