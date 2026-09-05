using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Modules;
using UI.Common;

namespace TrackLabClient.View.Status
{
    public class ModuleDetailViewModel : ViewModelBase
    {
        public ModuleBase Module { get; }

        public ModuleDetailViewModel(ModuleBase module)
        {
            Module = module;
            DisplayName = module.Name;
        }
    }
}
