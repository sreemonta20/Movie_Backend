using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Domain.AutoMapping;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.Services;
using Movie.BackEnd.Persistence.DBContext;
using Movie.BackEnd.Persistence.DBManager;

namespace Movie.BackEnd.API
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
            
            #region Implementation of .Net Core In Memory Cache
            
            services.AddMemoryCache();
            #endregion

            #region Cross Origin
            services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            #endregion

            #region Auto Mapper
            services.AddAutoMapper(
                typeof(AllModelsMapper)
                );
            //services.AddAutoMapper(typeof(AllModelsMapper),typeof(AllModelsMapper));

            #endregion

            services.AddControllers();

            #region For Database Connection Initialization
            
            Dictionary<string, string> dict = GetConnectionDetails("DefaultConnection", "DefaultProvider");

            services.AddDbContext<MovieDBContext>(options => options.UseSqlServer(dict["MainConnection"]));

            #endregion

            #region JWT Token Authentication Mechanism Registry
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AppSettings:Audience"],
                    ValidIssuer = Configuration["AppSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:SecretKey"]))
                };
            });

            #endregion

            #region Registry of DI in Dependency Injection Container

            services.AddScoped(typeof(IDBManager<>), typeof(SqlManager<>));
            services.AddScoped<ISecurityRepository, SecurityService>();
            services.AddScoped<IMovieRepository, MovieService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.UseHttpsRedirection();

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            #region Cross Origin
            app.UseCors("AllowOrigin");
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Dictionary<string, string> GetConnectionDetails(string defaultConnName, string mainprodvName)
        {
            Dictionary<string, string> oDic = new Dictionary<string, string>();
            ConnectionParameter.MainConnName = defaultConnName;
            ConnectionParameter.MainConnection = Configuration.GetSection("ConnectionStrings").GetSection(defaultConnName).Value;
            ConnectionParameter.MainProvider = Configuration.GetSection("ConnectionStrings").GetSection(mainprodvName).Value;
            

            oDic.Add("MainConnection", ConnectionParameter.MainConnection);
            oDic.Add("MainProvider", ConnectionParameter.MainProvider);



            return oDic;
        }
    }
}
