﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_SystemUsage.Models
{
    public class CpuMnt
    {
        public Guid Id { get; set; }

        public int Percent { get; set; }

        public DateTime CDate { get; set; }
    }
}
