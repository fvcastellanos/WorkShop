using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Connector.MySql.EFCore;
using Steeltoe.Management.Endpoint;
using WorkShop.Model;
using WorkShop.Repositories;
using WorkShop.Services;

namespace WorkShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAllActuators(Configuration);

            services.AddDbContext<WorkShopContext>(options => options.UseMySql(Configuration));

            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddHttpContextAccessor();

            services.AddScoped<InvoiceRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProviderRepository>();
            services.AddScoped<OperationTypeRepository>();
            services.AddScoped<DiscountTypeRepository>();

            services.AddScoped<ProductService>();
            services.AddScoped<OperationTypeService>();
            services.AddScoped<ProviderService>();
            services.AddScoped<DiscountTypeService>();
            services.AddScoped<ProviderInvoiceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapAllActuators();
                endpoints.MapControllers();
            });
        }
    }
}
