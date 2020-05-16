using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieRentalAdminApi.DataAccess;
using MovieRentalAdminApi.DataAccess.Repositories;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using MovieRentalAdminApi.Handlers;
using MovieRentalAdminApi.Middleware;
using MovieRentalAdminApi.Utils;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;

namespace MovieRentalAdminApi
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
            // Database Configure
            var connectionString = Configuration.GetConnectionString("MovieRentalConnection");
            if (!string.IsNullOrEmpty(connectionString))
                services.AddDbContext<MovieRentalDbContext>(opt => opt.UseSqlServer(connectionString));
            else
                services.AddDbContext<MovieRentalDbContext>(opt => opt.UseInMemoryDatabase("MovieRentalDb"));

            services.AddScoped<IRepository<MovieUpdateLogEntity>, Repository<MovieUpdateLogEntity>>();
            services.AddScoped<IRepository<RentedTrackingEntity>, Repository<RentedTrackingEntity>>();
            services.AddScoped<IRepository<MovieActionLogEntity>, Repository<MovieActionLogEntity>>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IMovieLikesRepository, MovieLikesRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IRentalSettingsRepository, RentalSettingsRepository>();
            services.AddScoped<IDomainHandler<MovieUpdated>, MovieUpdatedHandler>();
            services.AddScoped<IDomainHandler<MovieAction>, MovieActionHandler>();
            services.AddScoped<IEventDispatcher, EventContainer>();
            services.AddHttpContextAccessor();
            services.AddSingleton<ITokenFactory, JwtFactory>();
            services.AddMvc();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info { Title = "Movie Rental API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Type = "apiKey",
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = "header",
                });
                option.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
                option.DescribeAllEnumsAsStrings();
                option.OperationFilter<SwaggerFileOperationFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var signingKey = Convert.FromBase64String(Configuration["AuthJwt:SigningSecret"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Rental API v1");
            });
        }
    }
}
