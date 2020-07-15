using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Core.Interfaces.Infra.Repositories;
using Api.Infra;
using Api.Web.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Api.Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FluentValidation.AspNetCore;
using System.Text;
using Api.Web.Middlewares;
using Api.Core.Tasks.Commands.Failure;
using Api.Core.Entities.Failure;
using Api.Infra.Repositories.Security;
using Failure_Microservice.Web.Queues;

namespace Api.Web
{
    public class Startup
    {
    
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json",false,true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json" ,true)
                            .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DI
            //->DI

            //Core
            //CrossCutting
            
            //Infra
            services.AddScoped<AppDbContext>();
            services.AddScoped(typeof(IGenerericRepository<>), typeof(Api.Infra.Repositories.GenericRepository<>));
            services.AddScoped<IFailureRepository<Failure>, FailureRepository>();
            #endregion

            //connection String
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //Config the database
            services.AddDbContext<AppDbContext>(opt => 
            {
                opt.UseSqlServer(connectionString, dbOpt => 
                {
                    dbOpt.MigrationsAssembly("Failure-Microservice.Web");
                    dbOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            //reponse compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<Configuration.BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            //swagger setup
            services.AddSwaggerSetup();

            //mediaR setup
            services.AddMediatR(typeof(CreateFailureCommand).GetTypeInfo().Assembly);

            //Validation on DTOs
            //remove null from json response
            services.AddControllers()
                .AddFluentValidation(opt => 
                {
                    opt.RegisterValidatorsFromAssembly(typeof(CreateFailureCommand).GetTypeInfo().Assembly);
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });

            // config the queue
            FailureQueueConfig.FailureQueueConfiguration(services, Configuration);

            //Configure global exception middleware 
            services.AddGlobalExceptionHandlerMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerSetup();
            }

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseGlobalExceptionHandlerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
