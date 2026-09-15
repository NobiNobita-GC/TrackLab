using Core.Config;
using Core.Language;
using UI.Common;

namespace TrackLabClient.View.Configuration
{
    public class LanguageViewModel : ViewModelBase
    {
        public void SetLanguage(string language)
        {
            LanguageManager.ChangeLanguage(language);

            ConfigManager.Instance.SetConfig("System.Language", language);
        }
    }
}
