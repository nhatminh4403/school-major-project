using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;

namespace school_major_project.GlobalServices
{
    public class ExpiredItemCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpiredItemCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                    var itemsToDelete = await dbContext.Promotions
                        .Where(i => i.EndDate < sevenDaysAgo)
                        .ToListAsync();

                    dbContext.Promotions.RemoveRange(itemsToDelete);
                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
