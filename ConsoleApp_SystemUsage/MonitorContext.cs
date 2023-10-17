using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_SystemUsage
{
    using ConsoleApp_SystemUsage.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class MonitorContext : DbContext
    {

        public DbSet<CpuMnt> CpuMnts { get; set; }
        public DbSet<RamMnt> RamMnts { get; set; }
        public MonitorContext()
        {

        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Data Source=.;Initial Catalog=Mnt;Integrated Security=True;TrustServerCertificate=True;");
    }


}
