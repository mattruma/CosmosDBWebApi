using CosmosDBWebApi.Data;
using CosmosDBWebApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CosmosDBWebApi API", Version = "v1" });

                c.EnableAnnotations();

                c.IgnoreObsoleteActions();

                c.SchemaFilter<SwaggerExcludeSchemaFilter>(); // Hides model properties from Swagger display
            });

            // Note: Manual binding because sometimes I like to pull the settings from a configuration database and/or key vault
            services.Configure<AzureCosmosDbOptions>(
               options =>
               {
                   options.DatabaseId = Configuration["Azure:CosmosDb:DatabaseId"];
                   options.Key = Configuration["Azure:CosmosDb:Key"];
                   options.Endpoint = Configuration["Azure:CosmosDb:Endpoint"];
               });

            // services.AddTransient<IOrderRepository, OrderCosmosDbSdk2Repository>();
            services.AddTransient<IOrderRepository, OrderCosmosDbSdk3Repository>();
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

                c.DocExpansion(DocExpansion.List);

                c.EnableFilter();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
