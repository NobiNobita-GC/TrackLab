using Core.Language;
using UI.Common;

namespace TrackLabClient.View.Configuration
{
    public class LanguageViewModel : ViewModelBase
    {
        public void SetLanguage(string language)
        {
            LanguageManager.ChangeLanguage(language);
        }
    }
}
