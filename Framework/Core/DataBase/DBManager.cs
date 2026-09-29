using Core.Config;
using Core.Log;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public sealed class DBManager
    {
        public static DBManager Instance { get; } = new();

        private readonly DbContextOptions<TrackLabDbContext> _options;

        private DBManager()
        {
            string? dbConnection =
                ConfigManager.Instance.GetConfig("System.DBConnection");

            if (string.IsNullOrWhiteSpace(dbConnection))
            {
                throw new InvalidOperationException(
                    "System.DBConnection is not configured.");
            }

            string connectionString =
                $"{dbConnection.Trim().TrimEnd(';')};Database=TrackLab;AllowUserVariables=True;";

            _options = new DbContextOptionsBuilder<TrackLabDbContext>()
                .UseMySQL(connectionString)
                .Options;
        }

        public TrackLabDbContext CreateContext()
        {
            return new TrackLabDbContext(_options);
        }

        public bool CanConnect()
        {
            using TrackLabDbContext context = CreateContext();

            return context.Database.CanConnect();
        }

        public void AddOperationLog(OperationLogEntity log)
        {
            using TrackLabDbContext context = CreateContext();
            context.OperationLogs.Add(log);
            context.SaveChanges();
        }
    }
}