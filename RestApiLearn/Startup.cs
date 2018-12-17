using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Helpers;
using RestApiLearn.Services;

namespace RestApiLearn
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
            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<ILibraryRepository, LibraryRepository>();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(a => $"{a.FirstName} {a.LastName}"))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(a => a.DateOfBirth.GetCurrentAge()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext)
        {
            libraryContext.EnsureSeedDataForContext();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
