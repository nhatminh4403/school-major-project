using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;

namespace school_major_project.GlobalServices
{
    public class ExpiredItemCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredItemCleanupService> _logger; // Inject a logger

        public ExpiredItemCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredItemCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger; // Initialize the logger
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try // Start of the try-catch block
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        _logger.LogInformation("ExpiredItemCleanupService is running.");

                        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                        var itemsToDelete = await dbContext.Promotions
                            .Where(i => i.EndDate < sevenDaysAgo)
                            .ToListAsync(stoppingToken); // Pass the cancellation token

                        if (itemsToDelete.Any())
                        {
                            dbContext.Promotions.RemoveRange(itemsToDelete);
                            await dbContext.SaveChangesAsync(stoppingToken); // Pass the cancellation token
                            _logger.LogInformation("Successfully deleted {Count} expired promotions.", itemsToDelete.Count);
                        }
                        else
                        {
                            _logger.LogInformation("No expired promotions to delete.");
                        }
                    }
                }
                catch (Exception ex) // Catch any exception that occurs
                {
                    // Log the error instead of letting it crash the application
                    _logger.LogError(ex, "An error occurred in ExpiredItemCleanupService.");
                }

                // Wait for the next scheduled run
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}