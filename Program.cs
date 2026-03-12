using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Policies;
using NetCoreSeguridadEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication
    (options =>
    {
        options.DefaultAuthenticateScheme=
            CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme=
            CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme=
            CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    config =>
    {
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
    });

builder.Services.AddControllersWithViews
    (options=> options.EnableEndpointRouting=false).AddSessionStateTempDataProvider();

// Add services to the container.
builder.Services.AddControllersWithViews();

//LAS POLITICAS SE AGREGAN CON AUTHORIZATION
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<RepositoryHospital>();

builder.Services.AddSingleton<IAuthorizationHandler, SubordinadoHandler>();
builder.Services.AddAuthorization(options =>
{
    //DEBEMOS CREAR LAS POLICIES QUE NECESITEMOS PARA LOS ROLES
    
    options.AddPolicy("SOLOJEFES",
        policy=>policy.RequireRole("PRESIDENTE","DIRECTOR","ANALISTA"));
    options.AddPolicy("AdminOnly",
        policy=>policy.RequireClaim("Admin"));
    options.AddPolicy("SoloRicos",
        policy=>policy.Requirements.Add(new OverSalarioRequirement()));
    options.AddPolicy("SoloSub",
        policy => policy.Requirements.Add(new SubordinadoRequirement()));
    
});


string connectionString = builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddDbContext<DataContext>(options =>options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMvc(routes =>
    routes.MapRoute(name : "default",
        template: "{controller=Home}/{action=Index}/{id?}"));

// app.MapStaticAssets();

app.Run();
