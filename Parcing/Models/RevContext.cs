using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parcing.Models
{
    class RevContext:DbContext
    {
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Reviewers { get; set; }
        public DbSet<Rating> Rates { get; set; }
        public DbSet<TargetRating> TargetRates { get; set; }
        public DbSet<Area> Areas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Rev.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            modelBuilder.Entity<Review>().HasOne(rev => rev.Reviever).WithOne(us => us.Rev).HasForeignKey<Review>(r=>r.UserId);
            modelBuilder.Entity<Review>().HasOne(rev => rev.Rate).WithOne(r => r.Rev).HasForeignKey<Review>(r=>r.RateId);
            //modelBuilder.Entity<Review>().ToTable("Reviews");
            //modelBuilder.Entity<User>().ToTable("Reviews");
            //modelBuilder.Entity<Rating>().ToTable("Reviews");
            modelBuilder.Entity<TargetRating>().HasOne(tr => tr.NavRate).WithMany(r => r.Ratings).HasForeignKey(tr=>tr.ReviewId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
