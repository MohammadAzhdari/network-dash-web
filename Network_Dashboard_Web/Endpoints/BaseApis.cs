using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Network_Dashboard_Web.Endpoints
{
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
            var data = await _dbContext.CpuMnts.Select(x => x).Take(10).OrderByDescending(x => x.CDate).ToListAsync();

            return Ok(data.OrderBy(x => x.CDate).ToList());
        }
    }
}
