using Cheque_batch_tracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace Cheque_batch_tracker.DAL
{
    public class ChequeBatchContext : DbContext
    {
        public ChequeBatchContext(DbContextOptions<ChequeBatchContext> options) : base(options)  
        {
        }
        public DbSet<Batch> Batch { get; set; }
        public DbSet<UsedBatches> UsedBatches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AppDb");
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
           
        }
    }

}
