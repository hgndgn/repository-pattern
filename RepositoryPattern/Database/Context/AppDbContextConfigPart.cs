using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RepositoryPattern.Database.Context
{
    public partial class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            base.Database.EnsureDeleted();
            base.Database.EnsureCreated();
        }

        partial void OnConfiguringPartial(DbContextOptionsBuilder optionsBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var msg = $"[{nameof(AppDbContext)}] ---------------- OnConfiguring ----------------";
            Log.Information(msg);

            optionsBuilder.UseSqlite(_configuration["Database:Sqlite:Url"]);
            base.OnConfiguring(optionsBuilder);
            OnConfiguringPartial(optionsBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var msg = $"[{nameof(AppDbContext)}] ---------------- OnModelCreating ----------------";
            Log.Information(msg);

            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }
    }
}