using Microsoft.EntityFrameworkCore;
using Network_Dashboard_Web.Models;

namespace Network_Dashboard_Web
{
    public class MonitorContext : DbContext
    {
       
        public DbSet<CpuMnt> CpuMnts { get; set; }
        public MonitorContext()
        {
            
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Data Source=.;Initial Catalog=Mnt;Integrated Security=True;TrustServerCertificate=True;");
    }

   
}
