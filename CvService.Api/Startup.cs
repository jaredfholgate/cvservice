using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
      services.AddDbContext<CvContext>(options => options.UseSqlServer(connection));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }
  }
}
