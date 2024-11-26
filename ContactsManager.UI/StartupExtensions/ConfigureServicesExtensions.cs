using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.Mapping;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.UI.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonData.PersonsContext;
using RepositoryProject.CountryRepository;
using RepositoryProject.PersonRepository;

namespace ContactsManager.UI.StartupExtensions
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddControllersWithViews(options => {
                // options.Filters.Add<ResponseHeaderActionFilter>();// this is for filters without parameter 
                //ILogger<ResponseHeaderActionFilter> logger = services.BuildServiceProvider()
                //.GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                options.Filters.Add(new ResponseHeaderActionFilter("X-CustomGlobal-Key", "Custom-Value", 1));
            });
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderServices>();
            services.AddScoped<IPersonsGetterService, PersonsGetterServices>();
            services.AddScoped<IPersonsSorterService, PersonsSorterServices>();
            services.AddScoped<ICountryRepository, CountriesRepository>();
            services.AddScoped<IPersonRepository, PersonsRepository>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILogoutService, LogoutService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("PersonsConnectionString"));
            });
            // Configuring Identity
            services.AddIdentity<ApplicationUser,ApplicationRole>(
                options=> {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 3;
                }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>() // where to store Identity details.
                .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
                //UserStore is thr builin Repository layer for User in Identity we are congfiguring here.
                // we are saying UserStore is repository layer and UserStore needs
                // ApplicationUser,ApplicationRole,ApplicationDbContext,Guid
                .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>()
                //RoleStore is thr builin Repository layer for Roles in Identity we are congfiguring here.
                // we are saying RoleStore is repository layer and RoleStore needs
                // ApplicationRole,ApplicationDbContext,Guid
                .AddDefaultTokenProviders()
                // for providing token in diffrent senarios like login, resetpassword 
                // we need to enable this.
                //------------------------------------------------------------
                // UserManager,RoleManager (BLL/Service Layer) we no need to add that it will generate automatically.
                ;

            services.AddAuthorization(options => {
                options.FallbackPolicy= new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                // This block ensure that unless logged in User cant access any action in this
                // Application
                // to avoid Account Controller from this polycy add attribute [AllowAnonymus]
                // in action COntroller
            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                // If the User is not loggedIn then application will redirect to 
                // this path.
            });


            //Services.AddHttpLogging(options =>
            //options.LoggingFields =HttpLoggingFields.RequestProperties|HttpLoggingFields.ResponsePropertiesAndHeaders);
        }
    }
}
 