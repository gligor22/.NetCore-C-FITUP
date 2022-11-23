using FitUp.Repository.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitUp.Domain.Identity;
using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Repository.Implementation;
using FitUp.Service.Interfaces;
using FitUp.Service.Implementations;
using FitUp.Domain;
using FitUp.Service;
using Quartz.Spi;

using Quartz;
using Quartz.Impl;
using FitUp.Service.Jobs;
using FitUp.Service.Scheduler;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Rewrite;

namespace FitUp.Web
{
    public class Startup
    {
        private EmailSettings emailSettings;
        public Startup(IConfiguration configuration)
        {
            emailSettings = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<FitUpApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
                

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IRepository<Coach>, Repository<Coach>>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReservationRepository,ReservationRepository>();
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddScoped<IRepository<EmailMessage>, Repository<EmailMessage>>();

            services.AddScoped<EmailSettings>(es => emailSettings);
            services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailSettings));
            services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
            services.AddHostedService<EmailScopedHostedService>();

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICoachService, CoachService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ITrainingService, TrainingService>();


            //Scheduler
            
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, SingeltonJobFactory>();
            services.AddScoped<DayJob>();
            services.AddScoped<MonthJob>();
            services.AddScoped<WeekJob>();
            services.AddScoped<YearJob>();
            //they will be set specificly in deploy phase
           /* services.AddSingleton(new MyJob(jobType:typeof(DayJob),expression:"0/30 0/1 * 1/1 * ? *"));
            services.AddSingleton(new MyJob(jobType: typeof(WeekJob), expression: "0/30 0/1 * 1/1 * ? *"));
            services.AddSingleton(new MyJob(jobType: typeof(MonthJob), expression: "0/30 0/1 * 1/1 * ? *"));
            services.AddSingleton(new MyJob(jobType: typeof(YearJob), expression: "0/30 0/1 * 1/1 * ? *"));*/
            services.AddHostedService<MySchedluler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
