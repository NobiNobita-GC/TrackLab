using Core.Log;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public class TrackLabDbContext(DbContextOptions<TrackLabDbContext> options)
        : DbContext(options)
    {
        public DbSet<OperationLogEntity> OperationLogs { get; set; }
    }
}