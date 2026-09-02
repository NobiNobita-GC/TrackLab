using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackLab.Core.Modules;
using TrackLab.UI.Common;

namespace TrackLab.Client.View
{
    public class CoolPlateModuleViewModel : ViewModelBase
    {
        public ModuleBase Module { get; }
        public CoolPlateModuleViewModel(ModuleBase module)
        {
            Module = module;
            DisplayName = module.Name;
        }
    }
}
