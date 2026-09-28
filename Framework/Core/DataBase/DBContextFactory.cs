using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core.DataBase
{
    public class DBContextFactory : IDesignTimeDbContextFactory<TrackLabDbContext>
    {
        public TrackLabDbContext CreateDbContext(string[] args)
        {
            string? dbConnection =
                Config.ConfigManager.Instance.GetConfig("System.DBConnection");

            if (string.IsNullOrWhiteSpace(dbConnection))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }
            string connectionString = $"{dbConnection.Trim().TrimEnd(';')};Database=TrackLab;AllowUserVariables=True;";

            DbContextOptions<TrackLabDbContext> options =
                new DbContextOptionsBuilder<TrackLabDbContext>()
                    .UseMySQL(connectionString)
                    .Options;

            return new TrackLabDbContext(options);
        }
    }
}