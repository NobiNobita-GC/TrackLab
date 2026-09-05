using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules
{
    public abstract class ProcessModuleBase : ModuleBase
    {
        protected ProcessModuleBase(int index, string name) : base(index, name)
        {
        }

        public bool StartProcess()
        {
            return ChangeState(ModuleState.Running);
        }

        public bool CompleteProcess()
        {
            return ChangeState(ModuleState.Idle);
        }
    }
}
