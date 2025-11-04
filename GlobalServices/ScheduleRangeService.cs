using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Models;
using System;

namespace school_major_project.GlobalServices
{
    public class ScheduleRangeService : BackgroundService
    {
        private readonly ILogger<ScheduleRangeService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private const int MinDays = 3;
        private const int MaxDays = 5;

        public ScheduleRangeService(ILogger<ScheduleRangeService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try // ADDED: Start of the try-catch block for resilience
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        _logger.LogInformation("ScheduleRangeService is running.");

                        // Fetch data asynchronously
                        var cinemas = await dbContext.Cinemas.ToListAsync(stoppingToken);
                        var films = await dbContext.Films.ToListAsync(stoppingToken);

                        // 1. Asynchronously delete past schedules
                        var oldSchedules = await dbContext.Schedules
                            .Where(s => s.ScheduleTime < DateTime.Today)
                            .ToListAsync(stoppingToken);

                        if (oldSchedules.Any())
                        {
                            dbContext.Schedules.RemoveRange(oldSchedules);
                        }

                        // 2. Asynchronously get existing future schedule dates
                        var scheduleDays = await dbContext.Schedules
                            .Where(s => s.ScheduleTime >= DateTime.Today)
                            .Select(s => s.ScheduleTime.Date)
                            .Distinct()
                            .OrderBy(d => d)
                            .ToListAsync(stoppingToken);

                        // 3. Add new schedules if below the minimum
                        int daysToAdd = MinDays - scheduleDays.Count;
                        if (daysToAdd > 0)
                        {
                            DateTime lastDay = scheduleDays.Any() ? scheduleDays.Max() : DateTime.Today.AddDays(-1);
                            for (int i = 1; i <= daysToAdd; i++)
                            {
                                var targetDate = lastDay.AddDays(i);
                                await AddSchedulesForDateAsync(dbContext, cinemas, films, targetDate, stoppingToken);
                            }
                        }

                        // 4. Remove schedules if above the maximum
                        if (scheduleDays.Count > MaxDays)
                        {
                            var daysToRemove = scheduleDays.Skip(MaxDays).ToList();
                            var schedulesToRemove = await dbContext.Schedules
                                .Where(s => daysToRemove.Contains(s.ScheduleTime.Date))
                                .ToListAsync(stoppingToken);
                            dbContext.Schedules.RemoveRange(schedulesToRemove);
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("ScheduleRangeService completed successfully.");
                    }
                }
                catch (Exception ex) // ADDED: Catch block to log errors without crashing
                {
                    _logger.LogError(ex, "An error occurred in ScheduleRangeService.");
                }

                // Wait for the next daily run
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task AddSchedulesForDateAsync(ApplicationDbContext dbContext,
            List<Cinema> cinemas, List<Film> films, DateTime targetDate, CancellationToken cancellationToken)
        {
            if (!films.Any())
            {
                _logger.LogWarning("No films available to create schedules for date: {TargetDate}", targetDate);
                return;
            }

            var openTime = new TimeSpan(8, 0, 0);
            var closeTime = new TimeSpan(23, 0, 0);
            var breakTime = new TimeSpan(0, 15, 0);

            foreach (var cinema in cinemas)
            {
                var rooms = await dbContext.Rooms.Where(r => r.CinemaId == cinema.Id).ToListAsync(cancellationToken);

                foreach (var room in rooms)
                {
                    var currentTime = openTime;
                    int filmIndex = 0;

                    while (currentTime < closeTime)
                    {
                        var film = films[filmIndex % films.Count];
                        var showDateTime = targetDate.Add(currentTime);

                        // Check for schedule conflicts asynchronously
                        bool exists = await dbContext.Schedules.AnyAsync(s =>
                            s.RoomId == room.Id &&
                            s.ScheduleTime == showDateTime, cancellationToken);

                        if (!exists)
                        {
                            dbContext.Schedules.Add(new Schedule
                            {
                                FilmId = film.Id,
                                RoomId = room.Id,
                                ScheduleTime = showDateTime
                            });
                        }

                        currentTime = currentTime
                            .Add(TimeSpan.FromMinutes(film.FilmDuration))
                            .Add(breakTime);

                        filmIndex++;
                    }
                }
            }
        }
    }
}