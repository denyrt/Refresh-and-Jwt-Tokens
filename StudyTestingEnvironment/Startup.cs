using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceStack.Redis;
using StudyTestingEnvironment.Data.Contexts;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Handlers;
using StudyTestingEnvironment.Models.Options;
using StudyTestingEnvironment.Requirements;
using StudyTestingEnvironment.Services.Identity;
using StudyTestingEnvironment.Services.Identity.Repositories;

namespace StudyTestingEnvironment
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
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudyTestingEnvironment", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                   new OpenApiSecurityScheme
                   {
                     Reference = new OpenApiReference
                     {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                     }
                    },
                    new string[] { }
                  }
                });
            });
            
            services.AddOptions().AddLogging();

            services.AddDbContext<IdentityContext>(options => 
            {
                options.UseSqlServer(Configuration.GetConnectionString("MicrosoftSqlServer"));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisCache");
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
            services.AddScoped<ISessionManager, SessionManager>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPwdRecoveryCacheService, PwdRecoveryCacheService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ISessionsRepository, SessionsRepository>();
            services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(Configuration.GetConnectionString("RedisCache")));

            #region Microsoft Identity services

            var builder = services.AddIdentityCore<User>(options =>
            {

            });

            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);            
            identityBuilder.AddRoles<Role>();
            identityBuilder.AddEntityFrameworkStores<IdentityContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();
            identityBuilder.AddUserManager<AspNetUserManager<User>>();
            identityBuilder.AddRoleManager<AspNetRoleManager<Role>>();
            identityBuilder.AddDefaultTokenProviders();
            

            services.Configure<PasswordHasherOptions>(option =>
            {
                option.IterationCount = 12000;
            });

            #endregion

            ConfigureOptions(services);
        }

        public void ConfigureOptions(IServiceCollection services)
        {
            var jwtOptionsConfig = Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtOptionsConfig);
            
            var options = jwtOptionsConfig.Get<JwtOptions>();
            
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configure =>
            {
                configure.RequireHttpsMetadata = false;
                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = options.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = options.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("secure", policy => 
                    policy.Requirements.Add(new SecureTokenRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, SecureTokenHandler>();

            var sendGridOptions = Configuration.GetSection("SendGridOptions");
            services.Configure<SendGridOptions>(sendGridOptions);
            
            var frontendRoutingOptions = Configuration.GetSection("FrontendRoutingOptions");
            services.Configure<FrontendRoutingOptions>(frontendRoutingOptions);
            
            var pwdRecoveryCachingOptions = Configuration.GetSection("PwdRecoveryCachingOptions");
            services.Configure<PwdRecoveryCachingOptions>(pwdRecoveryCachingOptions);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyTestingEnvironment v1"));
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();           
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}