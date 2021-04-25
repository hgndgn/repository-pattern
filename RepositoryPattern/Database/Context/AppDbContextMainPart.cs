using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Database.DataSource;
using RepositoryPattern.Models;
using Serilog;

namespace RepositoryPattern.Database.Context
{
    public partial class AppDbContext : IDataSource
    {
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<DeveloperProjectRelation> DevelopersProjectsRelations { get; set; }

        partial void OnConfiguringPartial(DbContextOptionsBuilder optionsBuilder)
        {
            var msg = $"[{nameof(AppDbContext)}] ---------------- OnConfiguringPartial";
            Log.Information(msg);

            base.OnConfiguring(optionsBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            var msg = $"[{nameof(AppDbContext)}] ---------------- OnModelCreatingPartial";
            Log.Information(msg);

            modelBuilder.Entity<DeveloperProjectRelation>().HasKey(dp => new {dp.DeveloperId, dp.ProjectId});

            modelBuilder.Entity<DeveloperProjectRelation>()
                .HasOne(bc => bc.Developer)
                .WithMany(b => b.DeveloperProjectRelations)
                .HasForeignKey(bc => bc.DeveloperId);

            modelBuilder.Entity<DeveloperProjectRelation>()
                .HasOne(bc => bc.Project)
                .WithMany(c => c.DeveloperProjectRelations)
                .HasForeignKey(bc => bc.ProjectId);

            AddSampleData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void AddSampleData(ModelBuilder modelBuilder)
        {
            var dev1 = new Developer()
            {
                Id = Guid.Parse("15c011fe-428b-45c7-ae41-02f54bf35eb1"),
                Username = "hgndgn",
                Email = "hueseyin.guendogan@gmail.com",
                CreatedBy = "root",
                CreatedOn = DateTime.Now,
                LastModifiedBy = "root",
                LastModifiedOn = DateTime.Now,
            };

            var dev2 = new Developer()
            {
                Id = Guid.Parse("15c011fe-428b-45c7-ae41-02f54bf35eb2"),
                Username = "test",
                Email = "test@example.com",
                CreatedBy = "test",
                CreatedOn = DateTime.Now,
                LastModifiedBy = "test",
                LastModifiedOn = DateTime.Now,
            };

            modelBuilder.Entity<Developer>().HasData(dev1, dev2);

            var project = new Project()
            {
                Id = Guid.Parse("15c011fe-428b-45c7-ae41-02f54bf35eb3"),
                Title = "Project X",
                Description = "Project for doing X",
                CreatedBy = "root",
                CreatedOn = DateTime.Now,
                LastModifiedBy = "root",
                LastModifiedOn = DateTime.Now
            };

            modelBuilder.Entity<Project>().HasData(project);
        }

        public IEnumerable<T> FindAll<T>() where T : BaseEntity
        {
            return Set<T>().Where(e => e.IsActive).AsNoTracking();
        }

        public IEnumerable<T> FindByCondition<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return Set<T>().Where(e => e.IsActive).AsNoTracking().Where(expression).AsNoTracking();
        }

        public T FindFirstByCondition<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return Set<T>().Where(e => e.IsActive).AsNoTracking().Where(expression).FirstOrDefault();
        }

        public T FindById<T>(Guid id) where T : BaseEntity
        {
            var entity = Find<T>(id);

            if (entity == null) return null;

            return entity.IsActive ? entity : null;
        }

        public new T Add<T>(T entity) where T : BaseEntity
        {
            Set<T>().Add(entity);
            return entity;
        }

        public new T Update<T>(T entity) where T : BaseEntity
        {
            Set<T>().Update(entity);
            return entity;
        }

        public new T Remove<T>(T entity) where T : BaseEntity
        {
            entity.IsActive = false;
            Update(entity);
            return entity;
        }

        public new bool SaveChanges()
        {
            var affectedRows = base.SaveChanges();
            return affectedRows > 0;
        }
    }
}