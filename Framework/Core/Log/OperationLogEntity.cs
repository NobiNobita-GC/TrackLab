namespace Core.Log
{
    public class OperationLogEntity
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string User { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;
    }
}