using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Network_Dashboard_Web.Endpoints
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("api")]
    public class BaseApis : Controller
    {
        private readonly MonitorContext _dbContext;

        public BaseApis(MonitorContext monitorContext)
        {
            _dbContext = monitorContext;
        }

        [HttpGet("GetCpuUsage")]
        public async Task<IActionResult> GetCpuUsageAsync()
        {
            var data = await _dbContext.CpuMnts.Select(x => x).Take(16).OrderByDescending(x => x.CDate).ToListAsync();

            return Ok(data.OrderBy(x => x.CDate).ToList());
        }

        [HttpGet("GetRamUsage")]
        public async Task<IActionResult> GetRamUsageAsync()
        {
            var data = await _dbContext.RamMnts.Select(x => x).Take(10).OrderByDescending(x => x.CDate).ToListAsync();

            return Ok(data.OrderBy(x => x.CDate).ToList());
        }

        [HttpGet("GetDiskUsage")]
        public async Task<IActionResult> GetDiskUsageAsync()
        {
            var data = await _dbContext.DiskMnts.Select(x => x).Take(10).OrderByDescending(x => x.CDate).ToListAsync();

            return Ok(data.OrderBy(x => x.CDate).ToList());
        }

        [HttpGet("GetNetworkUsage")]
        public async Task<IActionResult> GetNetworkUsageAsync()
        {
            var data = await _dbContext.NetworkMnts.Select(x => x).Take(10).OrderByDescending(x => x.CDate).ToListAsync();

            return Ok(data.OrderBy(x => x.CDate).ToList());
        }
    }
}
