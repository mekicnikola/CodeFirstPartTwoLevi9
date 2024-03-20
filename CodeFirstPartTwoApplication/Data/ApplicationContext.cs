using System.Data.Entity;
using CodeFirstPartTwoApp.Models;

namespace CodeFirstPartTwoApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(string? connectionString = null)
            : base(connectionString ?? GetConnectionString())
        {
        }
        public virtual DbSet<Car> Cars { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<EngineType> EngineTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasRequired(c => c.Engine)
                .WithRequiredPrincipal(e => e.Car);

            modelBuilder.Entity<Engine>()
                .HasRequired<EngineType>(e => e.EngineType)
                .WithMany(et => et.Engines)
                .HasForeignKey(e => e.EngineTypeId);

            base.OnModelCreating(modelBuilder);
        }

        private static string GetConnectionString()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            const string relativePath = @"Data\connectionString.txt";
            var filePath = Path.Combine(baseDirectory, relativePath);
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Could not read the connection string file: {ex.Message}");
                return string.Empty;
            }
        }

    }
}
