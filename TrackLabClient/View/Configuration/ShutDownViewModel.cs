using System.Diagnostics;
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

        public void Restart()
        {
            string? processPath = Environment.ProcessPath;

            if (string.IsNullOrEmpty(processPath))
                return;

            Process.Start(processPath);
            Application.Current.Shutdown();
        }
    }
}
