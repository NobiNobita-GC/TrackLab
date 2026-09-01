using Caliburn.Micro;
using System.Diagnostics;

namespace TrackLab.UI.Common
{
    public class ViewModelBase : Screen
    {
        public virtual void Active()
        {
            Debug.WriteLine($"{GetType().Name} Active");
        }

        public virtual void Deactivate()
        {
            Debug.WriteLine($"{GetType().Name} Deactivate");
        }
    }
}
