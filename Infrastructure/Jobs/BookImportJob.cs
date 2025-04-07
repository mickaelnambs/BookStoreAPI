using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class BookImportJob(IBookImportService importService,
    IConfiguration config, ILogger<BookImportJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            string filePath = config["BookImport:FilePath"] ?? throw new Exception("Cannot get book import file path");
            logger.LogInformation("Starting book import from {FilePath} at {Time}", filePath, DateTime.Now);

            await importService.ImportBooksFromJsonAsync(filePath);

            logger.LogInformation("Book import completed at {Time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing books: {Message}", ex.Message);
            throw;
        }
    }
}
