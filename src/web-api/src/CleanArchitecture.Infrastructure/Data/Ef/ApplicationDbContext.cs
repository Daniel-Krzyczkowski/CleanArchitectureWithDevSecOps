using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Ef
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                                                                                : base(options)
        {
        }

        public DbSet<LessonTopicCategory> LessonTopicCategories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Maths"
            });

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Physics"
            });

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Programming"
            });

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Biology"
            });

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "History"
            });

            modelBuilder.Entity<LessonTopicCategory>().HasData(new LessonTopicCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Mechanics"
            });
        }
    }
}
