using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Business.Services;
using Northwind.Models;
using System.Text;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuration nesnesine eriþim
var configuration = builder.Configuration;

// JWT settings
var key = configuration["Jwt:Key"];
var issuer = configuration["Jwt:Issuer"];
var audience = configuration["Jwt:Audience"];

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.Headers.Add("Authentication-Failed", "true");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Additional custom validation logic
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { error = "You are not authorized" });
                return context.Response.WriteAsync(result);
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64; // Optional: Increase MaxDepth for deeper object graphs
});

// Configure DbContext
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IGenericService<EmployeeRequestDTO>, EmployeeService>();
builder.Services.AddScoped<IGenericService<CategoryRequestDTO>, CategoryService>();
builder.Services.AddScoped<IGenericService<CustomerRequestDTO>, CustomerService>();
builder.Services.AddScoped<IGenericService<MessageRequestDTO>, MessageService>();
builder.Services.AddScoped<IGenericService<OrderRequestDTO>, OrderService>();
builder.Services.AddScoped<IGenericService<OrderDetailRequestDTO>, OrderDetailService>();
builder.Services.AddScoped<IGenericService<ProductRequestDTO>, ProductService>();
builder.Services.AddScoped<IGenericService<ShipperRequestDTO>, ShipperService>();
builder.Services.AddScoped<IGenericService<SupplierRequestDTO>, SupplierService>();
builder.Services.AddScoped<IGenericService<ProductDetailRequestDTO>, ProductDetailService>();
builder.Services.AddScoped<IGenericService<AdminCategoryRequestDTO>, AdminCategoryService>();
builder.Services.AddScoped<IGenericService<SupplierDetailRequestDTO>, SupplierDetailService>();

// Register UserService
builder.Services.AddScoped<IGenericService<User>, UserService>();

// Add memory cache for session management
builder.Services.AddDistributedMemoryCache();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
    options.Cookie.HttpOnly = true; // Make cookies accessible only via HTTP
    options.Cookie.IsEssential = true; // Indicate that cookies are essential
});

// Add Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind API", Version = "v1" });

    // JWT Bearer Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please enter token in the format **'Bearer {your token}'**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use CORS policy
app.UseCors();

app.UseSession(); // Add session middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
