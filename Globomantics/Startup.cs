using System.IdentityModel.Tokens.Jwt;
using Globomantics.Binders;
using Globomantics.Constraints;
using Globomantics.Conventions;
using Globomantics.Filters;
using Globomantics.Middleware;
using Globomantics.Services;
using Globomantics.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using System;
using Globomantics.HttpMessageHandler;
using Globomantics.Authorization;

namespace Globomantics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ModelValidationFilter));
                options.Filters.Add(typeof(RateExceptionFilter));
                options.ModelBinderProviders.Insert(0, new SurveyBinderProvider());
                
                 
                //Model Convention are applied only once when the request starts up and not with every request.
                options.Conventions.Add(new APIConvention());
            });
            services.AddAuthorization(authOptions =>
            {
                authOptions.AddPolicy(
                    "isOldEnough",
                    policyReq =>
                    {
                        policyReq.RequireAuthenticatedUser()
                        .RequireClaim("age", "29");
                    });
            });
            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(opt => opt.AccessDeniedPath = "/Account/AccessDenied"
                )
            .AddOpenIdConnect(opt =>
            {
                opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.Authority = "https://localhost:5001/";
                opt.ClientId = "globomanticsclient";
                opt.ResponseType = "code";
                opt.Scope.Add("address");
                opt.Scope.Add("roles");
                opt.Scope.Add("courselibraryapi");
                opt.Scope.Add("age");
                opt.SaveTokens = true;
                opt.ClientSecret = "secret";
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.ClaimActions.MapUniqueJsonKey("role", "role");
                opt.ClaimActions.MapUniqueJsonKey("age", "age");
                //opt.ClaimActions.Remove("nbf");
                opt.ClaimActions.DeleteClaim("sid");
                opt.ClaimActions.DeleteClaim("idp");
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });
                //.AddOpenIdConnect(opt => {
                //    opt.Authority =""
                //})
                //.AddCookie("External")
                //.AddGoogle(options=> {
                //    options.SignInScheme = "External";
                //    options.ClientId = Configuration["Google:ClientId"];
                //    options.ClientSecret = Configuration["Google:ClientSecret"];
                //});
            services.AddSingleton<ILoanService, LoanService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddTransient<IQuoteService, QuoteService>();
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddTransient<IRateService, RateService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddTransient<BearerTokenHandler>();
            services.AddHttpClient("AuthorClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:61237");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<BearerTokenHandler>();

            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.ViewLocationExpanders.Add(new ThemeExpander());
            //});

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("tokenCheck", typeof(TokenConstraint));
                options.ConstraintMap.Add("versionCheck", typeof(VersioningConstraint));
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseMiddleware<BasicAuthMiddleware>();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.MapWhen(context => context.Request.Path.ToString().EndsWith(".timetable"),
                appbuilder =>
                {
                    appbuilder.UseMyHandlerMiddleware();
                });
            app.UseStaticFiles();

            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
