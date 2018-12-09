using GroupProjectBackend.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebNeuralNets.BusinessLogic;
using WebNeuralNets.BusinessLogic.ActivationFunctions;
using WebNeuralNets.BusinessLogic.BackgroundServices;
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
            services.AddHttpContextAccessor();

            //DB and identity
            services.AddDbContext<WebNeuralNetDbContext>(opt => opt.UseSqlServer(config.ConnectionString));

            services.AddSession();

            services.AddScoped<INeuralNetCreator, NeuralNetCreator>();

            //Singletons
            services.AddSingleton<IConfigProvider>(config);
            services.AddSingleton<ITranslationHelper, TranslationHelper>();
            services.AddSingleton<IActivationFunction, Sigmoid>();
            services.AddSingleton<INeuralNetTrainer, NeuralNetTrainer>();


            services.AddHostedService<NeuralNetTrainService>();
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

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc();
        }
    }
}
