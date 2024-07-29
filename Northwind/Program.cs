using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Business.Services;
using Northwind.Business.Response;
using Northwind.Models;
using Microsoft.AspNetCore.Session; // Oturum için gerekli kütüphane

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64; // Optional: Increase MaxDepth for deeper object graphs
});

// Configure DbContext
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Register SupplierDetailService
builder.Services.AddScoped<SupplierDetailService>(); // Eklenen kýsým

// Register MessageDetailService
builder.Services.AddScoped<MessageDetailService>();

// Register UserService
builder.Services.AddScoped<IGenericService<User>, UserService>(); // UserService'i buraya ekleyin

// Register ComprehensiveOrderDetailService
builder.Services.AddScoped<ComprehensiveOrderDetailService>();

// Add memory cache for session management
builder.Services.AddDistributedMemoryCache(); // Eklenen kýsým

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþým süresi
    options.Cookie.HttpOnly = true; // Çerezlerin sadece HTTP istekleriyle eriþilebilir olmasýný saðla
    options.Cookie.IsEssential = true; // Çerezlerin zorunlu olduðunu belirt
});

// Add Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind API", Version = "v1" });
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

app.UseSession(); // Oturum middleware'ini ekle
app.UseAuthorization();

app.MapControllers();

app.Run();
