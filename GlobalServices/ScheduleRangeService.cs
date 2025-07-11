using Microsoft.Extensions.Hosting;
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

        public ScheduleRangeService(ILogger<ScheduleRangeService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var cinemas = dbContext.Cinemas.ToList();
                    var films = dbContext.Films.ToList();

                    // 1. Xóa lịch chiếu đã qua
                    var oldSchedules = dbContext.Schedules
                        .Where(s => s.ScheduleTime < DateTime.Today)
                        .ToList();
                    dbContext.Schedules.RemoveRange(oldSchedules);

                    // 2. Lấy các ngày đã có lịch chiếu (từ hôm nay trở đi)
                    var scheduleDays = dbContext.Schedules
                        .Where(s => s.ScheduleTime >= DateTime.Today)
                        .Select(s => s.ScheduleTime.Date)
                        .Distinct()
                        .OrderBy(d => d)
                        .ToList();

                    // 3. Nếu số ngày < MinDays, tạo thêm lịch cho đủ MinDays
                    int daysToAdd = MinDays - scheduleDays.Count;
                    DateTime lastDay = scheduleDays.Any() ? scheduleDays.Max() : DateTime.Today.AddDays(-1);

                    for (int i = 1; i <= daysToAdd; i++)
                    {
                        var targetDate = lastDay.AddDays(i);
                        AddSchedulesForDate(dbContext, cinemas, films, targetDate);
                    }

                    // 4. Nếu số ngày > MaxDays, xóa lịch chiếu ở các ngày xa nhất
                    if (scheduleDays.Count > MaxDays)
                    {
                        var daysToRemove = scheduleDays.Skip(MaxDays).ToList();
                        var schedulesToRemove = dbContext.Schedules
                            .Where(s => daysToRemove.Contains(s.ScheduleTime.Date))
                            .ToList();
                        dbContext.Schedules.RemoveRange(schedulesToRemove);
                    }

                    dbContext.SaveChanges();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Lặp lại mỗi ngày
            }
        }
        private void AddSchedulesForDate(ApplicationDbContext dbContext,
            List<Cinema> cinemas, List<Film> films, DateTime targetDate)
        {
            var openTime = new TimeSpan(8, 0, 0);
            var closeTime = new TimeSpan(23, 0, 0);
            var breakTime = new TimeSpan(0, 15, 0);

            foreach (var cinema in cinemas)
            {
                var rooms = dbContext.Rooms.Where(r => r.CinemaId == cinema.Id).ToList();

                foreach (var room in rooms)
                {
                    var currentTime = openTime;
                    int filmIndex = 0;

                    while (currentTime < closeTime)
                    {
                        var film = films[filmIndex % films.Count];
                        var showDateTime = targetDate.Add(currentTime);

                        // Kiểm tra trùng lịch
                        bool exists = dbContext.Schedules.Any(s =>
                            s.RoomId == room.Id &&
                            s.ScheduleTime == showDateTime);

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
