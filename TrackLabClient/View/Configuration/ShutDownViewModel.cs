using Core.Language;
using System.Diagnostics;
using System.Windows;
using UI.Common;

namespace TrackLabClient.View.Configuration
{
    public class ShutDownViewModel : ViewModelBase
    {
        public void Shutdown()
        {
            MessageBoxResult result = MessageBox.Show(
                LanguageManager.GetString("ShutdownConfirmation"),
                LanguageManager.GetString("ShutDown"),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Application.Current.Shutdown();
        }

        public void Restart()
        {
            MessageBoxResult result = MessageBox.Show(
                LanguageManager.GetString("RestartConfirmation"),
                LanguageManager.GetString("Restart"),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            string? processPath = Environment.ProcessPath;

            if (string.IsNullOrEmpty(processPath))
                return;

            Process.Start(processPath);
            Application.Current.Shutdown();
        }
    }
}
