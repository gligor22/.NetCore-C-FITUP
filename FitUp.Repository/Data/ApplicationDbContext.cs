using FitUp.Domain.Identity;
using FitUp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<FitUpApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>().Property(z => z.id).ValueGeneratedOnAdd();
            builder.Entity<Coach>().Property(z => z.id).ValueGeneratedOnAdd();
            builder.Entity<Payment>().Property(z => z.id).ValueGeneratedOnAdd();
            builder.Entity<Reservation>().Property(z => z.id).ValueGeneratedOnAdd();
            builder.Entity<Training>().Property(z => z.id).ValueGeneratedOnAdd();
            builder.Entity<EmailMessage>().Property(z => z.id).ValueGeneratedOnAdd();


            builder.Entity<Reservation>().HasOne(z => z.Client).WithMany(z => z.Reservations).HasForeignKey(z => z.ClientID);
            builder.Entity<Reservation>().HasOne(z => z.Training).WithMany(z => z.Reservations).HasForeignKey(z => z.TrainingID);
            builder.Entity<Payment>().HasOne(z => z.Client).WithMany(z => z.Payments).HasForeignKey(z => z.ClientID);
            builder.Entity<Training>().HasOne(z => z.Coach).WithMany(z => z.Trainings).HasForeignKey(z => z.CoachID);
        }
    }
}
