using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Dashboard_Web.Models
{
    public class DiskMnt
    {
        public Guid Id { get; set; }

        public double MBPerSecond { get; set; }

        public DateTime CDate { get; set; }
    }
}
