namespace Core.Alarm
{
    public class AlarmItem
    {
        public DateTime Time { get; set; }
        public string Module { get; set; } = "";
        public string Level { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
