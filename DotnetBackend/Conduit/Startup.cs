
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

namespace Conduit
{
    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Conduit Api", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "authority"; // not exactly sure what do this
                    options.Audience = "audience"; // not exactly sure what do this
                    options.RequireHttpsMetadata = false;
                    options.Configuration = new OpenIdConnectConfiguration(); // why the actual eff is this line needed for swagger to work....
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = failed =>
                        {
                            failed.NoResult();

                            failed.Response.StatusCode = 500;
                            failed.Response.ContentType = "text/plain";
                            if (HostingEnvironment.IsDevelopment())
                                return failed.Response.WriteAsync(failed.Exception.ToString());

                            return failed.Response.WriteAsync("An error occured processing your authentication");
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "localhost",
                        ValidAudience = "all",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superlongandawesomesecuritykeynoonewillknow"))

                    };
                });

            services.AddMvc();
            services.AddMediatR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
            app.UseSwagger(o => o.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conduit API V1"));
        }
    }
}
