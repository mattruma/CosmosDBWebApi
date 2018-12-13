using CosmosDBWebApi.Data;
using CosmosDBWebApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Linq;

namespace CosmosDBWebApi
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
            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddMvc();

            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(2, 0); // Specify the default api version
                    options.AssumeDefaultVersionWhenUnspecified = true; // Assume that the caller wants the default version if they don't specify
                });

            services.AddRouting(
                options =>
                {
                    options.LowercaseUrls = true;
                });

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "CosmosDBWebApi API", Version = "v1", Description = "Uses the CosmosDB 2.0 SDK." });

                    options.SwaggerDoc("v2", new Info { Title = "CosmosDBWebApi API", Version = "v2", Description = "Uses the CosmosDB 3.0 SDK." });

                    options.EnableAnnotations();

                    options.IgnoreObsoleteActions();

                    options.SchemaFilter<SwaggerExcludeSchemaFilter>(); // Hides model properties from Swagger display
                });

            // Note: Manual binding because sometimes I like to pull the settings from a configuration database and/or key vault
            services.Configure<AzureCosmosDbOptions>(
               options =>
               {
                   options.DatabaseId = Configuration["Azure:CosmosDb:DatabaseId"]; // Could read from key vault if you wanted to  
                   options.Key = Configuration["Azure:CosmosDb:Key"];
                   options.Endpoint = Configuration["Azure:CosmosDb:Endpoint"];
               });

            services.AddTransient<IOrderCosmosDbSdk2Repository, OrderCosmosDbSdk2Repository>();
            services.AddTransient<IOrderCosmosDbSdk3Repository, OrderCosmosDbSdk3Repository>();
            services.AddTransient<IOrderItemCosmosDbSdk3Repository, OrderItemCosmosDbSdk3Repository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "CosmosDBWebApi Api Explorer";

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CosmosDBWebApi V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "CosmosDBWebApi V2");

                c.InjectStylesheet("/swagger-ui/custom.css");

                c.DocExpansion(DocExpansion.List);

                c.EnableFilter();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
