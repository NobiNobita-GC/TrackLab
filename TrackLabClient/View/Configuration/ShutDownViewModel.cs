using System.Windows;
using UI.Common;

namespace TrackLabClient.View.Configuration
{
    public class ShutDownViewModel : ViewModelBase
    {
        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}
