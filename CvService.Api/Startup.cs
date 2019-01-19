using AutoMapper;
using CvService.Repositories.Contexts;
using CvService.Repositories.Repositories;
using CvService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace CvService.Api
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
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddScoped<ICvService, Services.CvService>();
      services.AddScoped<ICompanyService, CompanyService>();
      services.AddScoped<ISkillService, SkillService>();
      services.AddScoped<ICvRepository, CvRepository>();
      services.AddScoped<ICompanyRepository, CompanyRepository>();
      services.AddScoped<ISkillRepository, SkillRepository>();
      var mapper = new Services.Mapper().GetMapper();
      services.AddScoped<IMapper>(o => mapper);

      var connection = Configuration.GetConnectionString("CvContext");
      var environmentConnectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection");
      if (!string.IsNullOrWhiteSpace(environmentConnectionString))
      {
        connection = environmentConnectionString;
      }
      services.AddDbContext<CvContext>(options => options.UseSqlServer(connection));

      var sp = services.BuildServiceProvider();

      using (var scope = sp.CreateScope())
      {
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CvContext>();
        db.Database.EnsureCreated();
      }

      var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Cv Service", Version = version });
        c.IncludeXmlComments(xmlPath);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cv Service API");
      });

      app.UseMvc();
    }
  }
}
