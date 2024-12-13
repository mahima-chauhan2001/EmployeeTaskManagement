//0
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using System.Text;
using EmployeeTaskManagement.DbContext;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using EmployeeTaskManagement.CommonService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        // Allow specific frontend URLs
        builder.WithOrigins("http://localhost:8080", "http://localhost:8081") // Add both URLs here
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),         
    };
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = builder.Configuration["JWT:Issuer"],
//            ValidateAudience = true,
//            ValidAudience = builder.Configuration["JWT:Audience"],
//            ValidateLifetime = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
//            ValidateIssuerSigningKey = true
//        };
//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine($"Authentication failed: {context.Exception.Message}"); // Logging for debugging
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = context =>
//            {
//                Console.WriteLine("Token validated successfully"); // Debug log
//                return Task.CompletedTask;
//            }
//        };
//    });

//builder.Services.ConfigureCustomServices();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

//builder.Services.AddAuthorization();
builder.Services.AddScoped<SeedRoleService>();

 


var app = builder.Build();

 
 
using (var scope = app.Services.CreateScope())
{
    var seedRoleService = scope.ServiceProvider.GetRequiredService<SeedRoleService>();
    await seedRoleService.SeedRolesAsync();  // Ensure roles are seeded before other middleware
    await SeedAdmin(scope.ServiceProvider);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection(); 
app.UseCors("AllowLocalhost");
app.UseAuthentication();  
app.UseAuthorization();    
app.MapControllers();   

app.Run();

// Seed method to create the admin user
async Task SeedAdmin(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
     
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
     
    var adminUserModel = await dbContext.UserModels
        .Where(u => u.Role == "Admin")
        .FirstOrDefaultAsync();  

    if (adminUserModel != null)
    { 
        var existingUser = await userManager.FindByEmailAsync(adminUserModel.Email);
        if (existingUser == null)
        { 
            var applicationUser = new ApplicationUser
            {
                UserName = adminUserModel.Email,
                Email = adminUserModel.Email,
                FirstName = adminUserModel.FirstName,
                LastName = adminUserModel.LastName,
                Role = adminUserModel.Role ?? "Employee"
            };
             
            var result = await userManager.CreateAsync(applicationUser, "Admin@123");

            if (result.Succeeded)
            { 
                await userManager.AddToRoleAsync(applicationUser, "Admin");
            }
            else
            {
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
} 

//1
//using EmployeeTaskManagement.CommonService;
//using EmployeeTaskManagement.DbContext; 
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Filters; 

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register Identity configuration (before Build)
//builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

//// Register authorization services
//builder.Services.AddAuthorization();

//// Register SeedRoleService (before Build)
//builder.Services.AddScoped<SeedRoleService>();

//// Register controllers
//builder.Services.AddControllers();

//// Swagger setup
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//// Build the application
//var app = builder.Build();

//// Use IServiceScope to resolve Scoped services during application startup
//using (var scope = app.Services.CreateScope())
//{
//    var seedRoleService = scope.ServiceProvider.GetRequiredService<SeedRoleService>();
//    await seedRoleService.SeedRolesAsync();  // Ensure roles are seeded before other middleware
//}

//// Swagger and development environment configuration
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Configure the HTTP request pipeline.
//app.MapIdentityApi<IdentityUser>();
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();



//2
//using EmployeeTaskManagement.DbContext;
//using EmployeeTaskManagement.Models;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Filters;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Configure Identity with ApplicationUser and IdentityRole
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders(); // Necessary for UserManager and SignInManager

//// Add authorization services
//builder.Services.AddAuthorization();

//// Add JWT authentication (if needed)
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//        };
//    });

//// Add controllers
//builder.Services.AddControllers();

//// Configure Swagger for API security
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//// Map API controllers
//app.MapControllers();

//app.Run();


