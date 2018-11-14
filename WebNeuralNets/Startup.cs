using GroupProjectBackend.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets
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
            IConfigProvider config = new ConfigProvider(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //DB and identity
            services.AddDbContext<WebNeuralNetDbContext>(opt => opt.UseSqlServer(config.ConnectionString));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<WebNeuralNetDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.User.RequireUniqueEmail = true;
            });
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    }).AddJwtBearer(cfg =>
            //    {
            //        cfg.RequireHttpsMetadata = false;
            //        cfg.SaveToken = true;
            //        cfg.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
            //            ClockSkew = TimeSpan.Zero // remove delay of token when expire
            //        };
            //    });

            //Singletons
            services.AddSingleton<IConfigProvider>(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WebNeuralNetDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
