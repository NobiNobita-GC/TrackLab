using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Alarm
{
    public class AlarmItem
    {
        public string Time { get; set; } = "";
        public string Module { get; set; } = "";
        public string Level { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
