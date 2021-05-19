using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterface;
using CabsBooking.API.Middleware;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CabsBooking.API
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
            services.AddHttpContextAccessor();
            services.AddDbContext<CabsBookingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CabsBookingDB")));

            //************* Repository *****************
            services.AddTransient<IBookingsRepository, BookingsRepository>();
            services.AddTransient<IBookingsHistoryRepository, BookingsHistoryRepository>();
            services.AddTransient<IPlacesRepository, PlaceRepository>();
            services.AddTransient<ICabTypesRepository, CabTypesRepository>();

            //************* service *****************
            services.AddTransient<IBookingsService, BookingsService>();
            services.AddTransient<IBookingsHistoryService, BookingsHisotryService>();
            services.AddTransient<IPlacesService, PlacesService>();
            services.AddTransient<ICabTypesService, CabTypesService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CabsBooking.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseCabsBookingExecptionMiddleware();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CabsBooking.API v1"));
            }
            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration.GetValue<string>("clientSPAUrl")).AllowAnyHeader()
                    .AllowAnyMethod();
            });
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
