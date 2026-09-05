using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Menu
{
    public class MenuItemEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? ViewModelType { get; set; }
        public List<MenuItemEntity>? SubMenuItems { get; set; }

        [JsonIgnore]
        public object? View {  get; set; }
    }
}
