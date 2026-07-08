
using AdvertisementApp.Business.DependencyResolvers.Microsoft;
using AdvertisementApp.Business.Helpers;
using AdvertisementApp.UI.Mappings.AutoMapper;
using AdvertisementApp.UI.Models;
using AdvertisementApp.UI.ValidationRules;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddDependencies(builder.Configuration);

// FluentValidation
builder.Services.AddTransient<IValidator<UserCreateModel>, UserCreateModelValidator>();

// Authentication
builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "Cookie";
		options.Cookie.HttpOnly = true;
		options.Cookie.SameSite = SameSiteMode.Strict;
		options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
		options.ExpireTimeSpan = TimeSpan.FromDays(20);
	});

// MVC
builder.Services.AddControllersWithViews();

//AutoMapper
var profiles = ProfileHelper.GetProfiles();
profiles.Add(new UserCreateModelProfile());

var mapperConfiguration = new AutoMapper.MapperConfiguration(cfg =>
{
	cfg.AddProfiles(profiles);
});

IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();