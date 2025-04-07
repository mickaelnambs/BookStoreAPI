using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace API.Controllers;

public class BookImportController(ISchedulerFactory schedulerFactory,
    ILogger<BookImportController> logger) : BaseApiController
{
    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerImport()
    {
        try
        {
            logger.LogInformation("Manual trigger of book import job requested");

            IScheduler scheduler = await schedulerFactory.GetScheduler();

            await scheduler.TriggerJob(new JobKey("BookImportJob"));

            return Ok(new { message = "Book import job triggered successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error triggering book import job");
            return StatusCode(500, new { error = "Failed to trigger job", message = ex.Message });
        }
    }
}
