using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TATask.AssetApi;
using TATask.AssetApi.Blocktap;
using TATask.Assets;
using TATask.Assets.Implementation;
using TATask.Assets.Interface;
using TATask.Configuration;
using TATask.Contracts;
using TATask.File;
using TATask.StringTools;
using TATask.Threading;

namespace TATask
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TATask", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<DefaultSettings>(cs => Configuration.GetSection("DefaultSettings").Bind(cs));
            services.Configure<ApiSettings>(cs => Configuration.GetSection("Blocktap").Bind(cs));
            services.Configure<PageQuerySettings>(cs => Configuration.GetSection("PageQuerySettings").Bind(cs));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IStringTool, AlgorithmicTool>();
            services.AddScoped<IThreadTask, CommunicationThreadTask>();
            services.AddScoped<IRemoteFile, RemoteFile>();
            services.AddScoped<IAssetQuery, AssetQuery>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IPricedAssetService, AssetPriceServiceAggregate>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TATask v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
