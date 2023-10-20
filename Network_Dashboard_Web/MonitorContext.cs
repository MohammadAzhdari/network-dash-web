using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Network_Dashboard_Web.Models;
using Network_Dashboard_Web.Models.Authentication;

namespace Network_Dashboard_Web
{
    public class MonitorContext : IdentityDbContext<ApplicationUser>
    {
        public MonitorContext(DbContextOptions<MonitorContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<CpuMnt> CpuMnts { get; set; }
        public DbSet<RamMnt> RamMnts { get; set; }
        public DbSet<DiskMnt> DiskMnts { get; set; }
        public DbSet<NetworkMnt> NetworkMnts { get; set; }
        public MonitorContext()
        {
            
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Data Source=.;Initial Catalog=Mnt;Integrated Security=True;TrustServerCertificate=True;");
    }

   
}
